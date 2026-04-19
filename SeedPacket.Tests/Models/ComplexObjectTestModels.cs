using System;

namespace SeedPacket.Tests.Models
{
    // Test models for CreateComplexObject functionality

    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Person Manager { get; set; }  // Nested complex object
        public Company Employer { get; set; }  // Another nested complex object
    }

    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Person Manager { get; set; }
        public Company ParentCompany { get; set; }
    }

    // For testing circular reference handling
    public class Node
    {
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public Node Parent { get; set; }  // Self-referencing type
        public Node Child { get; set; }   // Self-referencing type
    }

    // For testing deep nesting
    public class Level1
    {
        public string Name { get; set; }
        public Level2 NestedLevel { get; set; }
    }

    public class Level2
    {
        public string Name { get; set; }
        public Level3 NestedLevel { get; set; }
    }

    public class Level3
    {
        public string Name { get; set; }
        public Level4 NestedLevel { get; set; }
    }

    public class Level4
    {
        public string Name { get; set; }
        public Level5 NestedLevel { get; set; }
    }

    public class Level5
    {
        public string Name { get; set; }
        public Level6 NestedLevel { get; set; }
    }

    public class Level6
    {
        public string Name { get; set; }
        public string DeepValue { get; set; }
    }
}
