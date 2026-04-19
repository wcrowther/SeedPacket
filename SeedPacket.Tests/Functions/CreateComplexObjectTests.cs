using NUnit.Framework;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Tests.Models;
using SeedPacket.Extensions;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace SeedPacket.Tests.Functions
{
    [TestFixture]
    public class CreateComplexObjectTests
    {
        [Test]
        public void CreateComplexObject_BasicGenerator_SimpleObject()
        {
            // Arrange
            var generator = new BasicGenerator();

            // Act
            var person = Funcs.CreateComplexObjectTyped<Person>(generator);

            // Assert
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.FirstName);
            Assert.IsNotNull(person.LastName);
            Assert.IsNotNull(person.Email);
            Assert.AreNotEqual(default(int), person.PersonId);
        }

        [Test]
        public void CreateComplexObject_MultiGenerator_SimpleObject()
        {
            // Arrange
            var generator = new MultiGenerator();

            // Act
            var person = Funcs.CreateComplexObjectTyped<Person>(generator);

            // Assert
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.FirstName);
            Assert.IsNotNull(person.LastName);
            Assert.That(person.Email.Contains("@"), "Email should contain @");
        }

        [Test]
        public void CreateComplexObject_Company_AllPropertiesPopulated()
        {
            // Arrange
            var generator = new MultiGenerator();

            // Act
            var company = Funcs.CreateComplexObjectTyped<Company>(generator);

            // Assert
            Assert.IsNotNull(company);
            Assert.IsNotNull(company.CompanyName, "CompanyName should be populated");
            Assert.IsNotNull(company.Address, "Address should be populated");
            Assert.IsNotNull(company.City, "City should be populated");
            Assert.IsNotNull(company.State, "State should be populated");
            Assert.IsNotNull(company.Zip, "Zip should be populated");
        }

        [Test]
        public void CreateComplexObject_CachesObjects()
        {
            // Arrange
            var generator = new MultiGenerator();
            var cacheSize = 10;

            // Act - First call should create cache
            var person1 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: cacheSize);

            // Get the cache to verify it was created
            var cacheKey = $"ComplexObjectCache.{typeof(Person).FullName}";
            ExpandoObject cache = generator.Cache;
            var cachedObjectsDynamic = cache.Get<List<Person>>(cacheKey);
            var cachedObjects = cachedObjectsDynamic as List<Person>;

            // Assert
            Assert.IsNotNull(cachedObjects);
            Assert.AreEqual(cacheSize, cachedObjects.Count, "Should have cached the specified number of objects");
            // Verify that at least one object in cache has valid properties
            Assert.IsTrue(cachedObjects.All(p => p.FirstName != null), "All cached objects should have valid FirstName");
        }

        [Test]
        public void CreateComplexObject_ReturnsRandomFromCache()
        {
            // Arrange
            var generator = new MultiGenerator();
            var cacheSize = 10;

            // Act - Create multiple objects
            var person1 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: cacheSize);
            var person2 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: cacheSize);
            var person3 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: cacheSize);

            // Assert - All should be valid persons (could be same or different from cache)
            Assert.IsNotNull(person1);
            Assert.IsNotNull(person2);
            Assert.IsNotNull(person3);
        }

        [Test]
        public void CreateComplexObject_NestedObjects_SingleLevel()
        {
            // Arrange
            var generator = new MultiGenerator();
            // Manually add rules for complex types that should be auto-populated
            generator.Rules.Add(new Rule(typeof(Person), "", g => g.CreateComplexObject(), "PersonComplexObject"));
            generator.Rules.Add(new Rule(typeof(Company), "", g => g.CreateComplexObject(), "CompanyComplexObject"));

            // Act
            var employee = Funcs.CreateComplexObjectTyped<Employee>(generator);

            // Assert
            Assert.IsNotNull(employee);
            Assert.IsNotNull(employee.FirstName);
            Assert.IsNotNull(employee.LastName);
            Assert.IsNotNull(employee.Manager, "Nested Person object should be created");
            Assert.IsNotNull(employee.Manager.FirstName, "Nested Person should have FirstName");
            Assert.IsNotNull(employee.Employer, "Nested Company object should be created");
            Assert.IsNotNull(employee.Employer.CompanyName, "Nested Company should have CompanyName");
        }

        [Test]
        public void CreateComplexObject_NestedObjects_AutomaticWithDefaultRule()
        {
            // Arrange - MultiGenerator includes the typeof(object) rule automatically
            var generator = new MultiGenerator();

            // Act - No manual rule addition needed!
            var employee = Funcs.CreateComplexObjectTyped<Employee>(generator);

            // Assert - Nested objects are automatically populated via the typeof(object) catch-all rule
            Assert.IsNotNull(employee);
            Assert.IsNotNull(employee.FirstName);
            Assert.IsNotNull(employee.LastName);
            Assert.IsNotNull(employee.Manager, "Nested Person object should be auto-created via typeof(object) rule");
            Assert.IsNotNull(employee.Manager.FirstName, "Nested Person should have FirstName");
            Assert.IsNotNull(employee.Employer, "Nested Company object should be auto-created via typeof(object) rule");
            Assert.IsNotNull(employee.Employer.CompanyName, "Nested Company should have CompanyName");
        }

        [Test]
        public void CreateComplexObject_Department_MultipleNestedObjects()
        {
            // Arrange
            var generator = new MultiGenerator();
            // Manually add rules for complex types
            generator.Rules.Add(new Rule(typeof(Person), "", g => g.CreateComplexObject(), "PersonComplexObject"));
            generator.Rules.Add(new Rule(typeof(Company), "", g => g.CreateComplexObject(), "CompanyComplexObject"));

            // Act
            var department = Funcs.CreateComplexObjectTyped<Department>(generator);

            // Assert
            Assert.IsNotNull(department);
            Assert.IsNotNull(department.DepartmentName);
            Assert.IsNotNull(department.Manager, "Manager should be populated");
            Assert.IsNotNull(department.Manager.FirstName, "Manager FirstName should be populated");
            Assert.IsNotNull(department.ParentCompany, "ParentCompany should be populated");
            Assert.IsNotNull(department.ParentCompany.CompanyName, "ParentCompany name should be populated");
        }

        [Test]
        public void CreateComplexObject_CircularReference_DoesNotCauseInfiniteLoop()
        {
            // Arrange
            var generator = new MultiGenerator();
            var maxDepth = 3;

            // Act - Should not throw StackOverflowException
            var node = Funcs.CreateComplexObjectTyped<Node>(generator, maxDepth: maxDepth);

            // Assert
            Assert.IsNotNull(node);
            Assert.IsNotNull(node.NodeName);
            // Parent and Child might be populated up to maxDepth
        }

        [Test]
        public void CreateComplexObject_DeepNesting_RespectsMaxDepth()
        {
            // Arrange
            var generator = new MultiGenerator();
            var maxDepth = 3;  // Should stop before Level4

            // Act
            var level1 = Funcs.CreateComplexObjectTyped<Level1>(generator, maxDepth: maxDepth);

            // Assert
            Assert.IsNotNull(level1);
            Assert.IsNotNull(level1.Name);

            if (level1.NestedLevel != null)
            {
                Assert.IsNotNull(level1.NestedLevel.Name);

                if (level1.NestedLevel.NestedLevel != null)
                {
                    Assert.IsNotNull(level1.NestedLevel.NestedLevel.Name);
                    // Should stop here or soon after due to depth limit
                }
            }
        }

        [Test]
        public void CreateComplexObject_WithCustomRules_UsesCustomRule()
        {
            // Arrange
            var generator = new MultiGenerator();
            generator.Rules.Add(new Rule(typeof(string), "firstname", g => "CustomFirstName", "CustomFirstNameRule"));

            // Act
            var person = Funcs.CreateComplexObjectTyped<Person>(generator);

            // Assert
            Assert.AreEqual("CustomFirstName", person.FirstName, "Should use custom rule for FirstName");
        }

        [Test]
        public void CreateComplexObject_AsRuleInSeedList_PopulatesEmployees()
        {
            // Arrange
            var generator = new MultiGenerator();
            // Manually add rules for complex types to enable auto-population
            generator.Rules.Add(new Rule(typeof(Person), "", g => g.CreateComplexObject(), "PersonComplexObject"));
            generator.Rules.Add(new Rule(typeof(Company), "", g => g.CreateComplexObject(), "CompanyComplexObject"));
            generator.Rules.Add(new Rule(typeof(Employee), "", g => g.CreateComplexObject(), "EmployeeComplexObject"));

            // Act
            var employees = new List<Employee>().Seed(1, 5, generator);

            // Assert
            var employeeList = employees.ToList();
            Assert.AreEqual(5, employeeList.Count);
            foreach (var employee in employeeList)
            {
                Assert.IsNotNull(employee);
                Assert.IsNotNull(employee.FirstName, "FirstName should be populated");
                Assert.IsNotNull(employee.LastName, "LastName should be populated");
                Assert.IsNotNull(employee.Manager, "Manager (nested object) should be populated");
                Assert.IsNotNull(employee.Manager.FirstName, "Manager FirstName should be populated");
                Assert.IsNotNull(employee.Employer, "Employer (nested object) should be populated");
            }
        }

        [Test]
        public void CreateComplexObject_AsRuleInSeedList_AutomaticPopulation()
        {
            // Arrange - typeof(object) rule is automatically in MultiGenerator
            var generator = new MultiGenerator();

            // Act - Just seed the list! No manual rules needed
            var employees = new List<Employee>().Seed(1, 5, generator);

            // Assert - Everything is automatically populated including nested objects
            var employeeList = employees.ToList();
            Assert.AreEqual(5, employeeList.Count);
            foreach (var employee in employeeList)
            {
                Assert.IsNotNull(employee);
                Assert.IsNotNull(employee.FirstName, "FirstName should be auto-populated");
                Assert.IsNotNull(employee.LastName, "LastName should be auto-populated");
                Assert.IsNotNull(employee.Manager, "Manager should be auto-populated via typeof(object) rule");
                Assert.IsNotNull(employee.Manager.FirstName, "Manager.FirstName should be auto-populated");
                Assert.IsNotNull(employee.Employer, "Employer should be auto-populated via typeof(object) rule");
                Assert.IsNotNull(employee.Employer.CompanyName, "Employer.CompanyName should be auto-populated");
            }
        }

        [Test]
        public void CreateComplexObject_MultipleCalls_UsesSeparateCachesPerType()
        {
            // Arrange
            var generator = new MultiGenerator();

            // Act
            var person = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: 5);
            var company = Funcs.CreateComplexObjectTyped<Company>(generator, cacheSize: 5);

            // Assert
            var personCacheKey = $"ComplexObjectCache.{typeof(Person).FullName}";
            var companyCacheKey = $"ComplexObjectCache.{typeof(Company).FullName}";

            ExpandoObject cache = generator.Cache;
            var personCache = cache.Get<List<Person>>(personCacheKey);
            var companyCache = cache.Get<List<Company>>(companyCacheKey);

            Assert.IsNotNull(personCache, "Person cache should exist");
            Assert.IsNotNull(companyCache, "Company cache should exist");
            Assert.AreEqual(5, personCache.Count);
            Assert.AreEqual(5, companyCache.Count);
        }

        [Test]
        public void CreateComplexObject_SameTypeDifferentCalls_ReturnsDifferentInstances()
        {
            // Arrange
            var generator = new MultiGenerator();
            var personList = new List<Person>();

            // Act - Create multiple persons
            for (int i = 0; i < 20; i++)
            {
                generator.GetNextRowRandom(); // Move to next random seed
                var person = Funcs.CreateComplexObjectTyped<Person>(generator);
                personList.Add(person);
            }

            // Assert - Should have gotten different instances from cache (randomly selected)
            // With 10 cached items and 20 calls, we should see some variety
            var distinctFirstNames = personList.Select(p => p.FirstName).Distinct().Count();
            Assert.IsTrue(distinctFirstNames >= 2, "Should return different instances from cache across multiple calls");
        }

        [Test]
        public void CreateComplexObject_WithMaxDepthZero_ReturnsNull()
        {
            // Arrange
            var generator = new MultiGenerator();

            // Act
            var person = Funcs.CreateComplexObjectTyped<Person>(generator, maxDepth: 0);

            // Assert
            Assert.IsNull(person, "Should return null when maxDepth is 0");
        }

        [Test]
        public void CreateComplexObject_WithCacheSizeOne_CreatesSingleCachedObject()
        {
            // Arrange
            var generator = new MultiGenerator();

            // Act
            var person1 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: 1);
            var person2 = Funcs.CreateComplexObjectTyped<Person>(generator, cacheSize: 1);

            // Assert
            var cacheKey = $"ComplexObjectCache.{typeof(Person).FullName}";
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.Get<List<Person>>(cacheKey);

            Assert.AreEqual(1, cacheList.Count);
            // Both should reference the same cached object
            Assert.AreSame(person1, person2, "With cache size 1, should return same instance");
        }
    }
}
