# CreateComplexObject Function

## Overview
The `CreateComplexObject` function provides **automatic population of complex objects** by iterating through their public properties and applying existing rules. It supports nested objects, caching for performance, and recursion protection.

**✨ NEW**: With the `typeof(object)` rule now included in `AddCommonRules`, complex objects are **automatically populated** when using `MultiGenerator` - no manual rule setup required!

## Features
- ✅ **Automatic Property Population**: Uses existing rules to populate all properties
- ✅ **Nested Object Support**: Recursively populates complex properties
- ✅ **Caching**: Generates and caches objects for reuse (default: 10 per type)
- ✅ **Recursion Protection**: Configurable maximum depth (default: 5)
- ✅ **Circular Reference Handling**: Prevents infinite loops
- ✅ **Zero Configuration**: Works automatically with `MultiGenerator`

## Quick Start - Automatic (Recommended)

### Simplest Usage - Just Seed!
```csharp
var generator = new MultiGenerator();

// That's it! Complex objects are automatically populated
var employees = new List<Employee>().Seed(1, 100, generator);

// All properties are populated including:
// - Employee.FirstName, LastName, Email
// - Employee.Manager (nested Person object with all properties)
// - Employee.Employer (nested Company object with all properties)
```

### How It Works
When you use `MultiGenerator` (or any generator with `AddCommonRules()`), the `typeof(object)` catch-all rule is automatically included. This rule:
1. Matches any class type that doesn't have a more specific rule
2. Calls `CreateComplexObject()` to populate it
3. Recursively handles nested objects
4. Caches objects for performance

## Usage

### Basic Usage - Direct Function Call
```csharp
var generator = new MultiGenerator();

// Create a single populated object
var person = Funcs.CreateComplexObjectTyped<Person>(generator);

// All properties are automatically populated using existing rules
Console.WriteLine($"{person.FirstName} {person.LastName}");
Console.WriteLine(person.Email);
```

### Advanced Usage - Custom Parameters
```csharp
var generator = new MultiGenerator();

// Create with custom recursion depth and cache size
var employee = Funcs.CreateComplexObjectTyped<Employee>(
    generator, 
    maxDepth: 3,      // Maximum recursion depth
    cacheSize: 20     // Number of objects to cache
);
```

### Usage with SeedList - Automatic (Recommended)
Complex objects are automatically populated with `MultiGenerator`:

```csharp
var generator = new MultiGenerator();

// No manual rules needed! The typeof(object) rule handles it automatically
var employees = new List<Employee>().Seed(1, 100, generator);
var departments = new List<Department>().Seed(1, 50, generator);
var people = new List<Person>().Seed(1, 200, generator);

// All complex objects and their nested properties are fully populated
```

### Advanced - Manual Rule Addition (Optional)
If you want to override the default behavior or use `BasicGenerator`, you can still add rules manually:

```csharp
var generator = new BasicGenerator(); // or new MultiGenerator()

// Add rules for specific complex types
generator.Rules.Add(new Rule(
    typeof(Person), 
    "", 
    g => g.CreateComplexObject(), 
    "PersonComplexObject"
));

generator.Rules.Add(new Rule(
    typeof(Company), 
    "", 
    g => g.CreateComplexObject(), 
    "CompanyComplexObject"
));

var employees = new List<Employee>().Seed(1, 100, generator);
```

### With Custom Rules
You can override specific properties with custom rules while still benefiting from automatic complex object population:

```csharp
var generator = new MultiGenerator();

// Add a custom rule for FirstName
generator.Rules.Add(new Rule(
    typeof(string), 
    "firstname", 
    g => "CustomName", 
    "CustomFirstNameRule"
));

// Complex objects are still automatically populated
var people = new List<Person>().Seed(1, 100, generator);
// All Person.FirstName will be "CustomName"
// Other properties use default rules
// Nested objects are still auto-populated
```

### Overriding the Default Complex Object Rule
If you want different behavior for a specific type:

```csharp
var generator = new MultiGenerator();

// Override the default for a specific type
generator.Rules.Add(new Rule(
    typeof(Person), 
    "", 
    g => new Person { FirstName = "Override", LastName = "Name" }, 
    "CustomPersonRule",
    overwrite: true  // Replace the typeof(object) rule for this type
));

var employees = new List<Employee>().Seed(1, 100, generator);
// Employee.Manager will use the custom Person rule
// Employee.Employer will use the default typeof(object) rule
```

## Example Models

### Simple Object
```csharp
public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }  // Auto-populated from "FirstName" rule
    public string LastName { get; set; }    // Auto-populated from "LastName" rule
    public string Email { get; set; }       // Auto-populated from "Email" rule
    public DateTime Created { get; set; }   // Auto-populated from "DateTime" rule
}

// Usage - automatic with MultiGenerator
var generator = new MultiGenerator();
var people = new List<Person>().Seed(1, 100, generator);  // Done!
```

### Nested Objects - Automatic!
```csharp
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Person Manager { get; set; }     // Auto-populated via typeof(object) rule
    public Company Employer { get; set; }   // Auto-populated via typeof(object) rule
}

// Usage - automatic with MultiGenerator
var generator = new MultiGenerator();
var employees = new List<Employee>().Seed(1, 100, generator);
// Everything is populated including nested Manager and Employer!
```

## How the typeof(object) Catch-All Rule Works

The `typeof(object)` rule in `AddCommonRules` acts as a smart fallback:

1. **Rule Processing Order**: Rules are checked from last to first
2. **typeof(object) is First**: Added at the start of the list, checked last
3. **Specific Rules Win**: If a more specific rule exists (e.g., `typeof(Person)`), it's used instead
4. **Class Types Only**: Only matches class types (not value types, not strings)
5. **Smart Matching**: Processes complex objects without specific rules

```csharp
// Internally in AddCommonRules.cs:
var commonRules = new List<Rule>() {
    new (typeof(object), "", g => g.CreateComplexObject(), "ComplexObject", "..."),
    new (typeof(string), "", g => g.GetElementRandom(), "String", "..."),
    new (typeof(string), "%email%", g => g.RandomEmail(), "Email", "..."),
    // ... more specific rules
};
```

When `SeedPacket` processes an `Employee.Manager` property of type `Person`:
1. Checks for more specific rules first
2. Eventually reaches `typeof(object)` rule → **matches!**
3. Calls `CreateComplexObject()` which populates the Person
4. Person's properties are then populated using their specific rules (FirstName, LastName, Email, etc.)

## Caching Behavior
The function caches generated objects by type to improve performance:

```csharp
var generator = new MultiGenerator();

// First call generates and caches 10 Person objects
var person1 = Funcs.CreateComplexObjectTyped<Person>(generator);

// Subsequent calls return random objects from cache (no regeneration)
var person2 = Funcs.CreateComplexObjectTyped<Person>(generator);
var person3 = Funcs.CreateComplexObjectTyped<Person>(generator);

// person1, person2, and person3 are randomly selected from the same cache of 10 objects
```

## Recursion Protection
Maximum depth prevents infinite loops with circular references:

```csharp
public class Node
{
    public int NodeId { get; set; }
    public string NodeName { get; set; }
    public Node Parent { get; set; }  // Circular reference
}

var generator = new MultiGenerator();
generator.Rules.Add(new Rule(typeof(Node), "", g => g.CreateComplexObject(), "NodeRule"));

// maxDepth prevents infinite recursion
var node = Funcs.CreateComplexObjectTyped<Node>(generator, maxDepth: 3);

// node.Parent might be populated
// node.Parent.Parent might be populated
// node.Parent.Parent.Parent might be populated
// node.Parent.Parent.Parent.Parent will be null (exceeded maxDepth)
```

## Why Automatic with MultiGenerator?

✅ **With MultiGenerator** (includes `AddCommonRules`):
- `typeof(object)` catch-all rule is automatically added
- Complex objects just work - no setup needed
- Nested objects recursively populated
- Perfect for rapid prototyping and simple use cases

⚙️ **With BasicGenerator** (or custom generators):
- Only basic type rules are included
- Complex objects need manual rules
- More control over what gets populated
- Better for specific/controlled scenarios

```csharp
// MultiGenerator - Automatic
var multi = new MultiGenerator();
var employees = new List<Employee>().Seed(1, 100, multi);  // ✅ Just works!

// BasicGenerator - Manual control
var basic = new BasicGenerator();
basic.Rules.Add(new Rule(typeof(Person), "", g => g.CreateComplexObject(), "Person"));
basic.Rules.Add(new Rule(typeof(Employee), "", g => g.CreateComplexObject(), "Employee"));
var employees2 = new List<Employee>().Seed(1, 100, basic);  // ✅ Explicit control
```

## Best Practices

1. **Add rules at generator setup time** before seeding lists
2. **Use appropriate cache sizes** based on your data variety needs
3. **Set maxDepth appropriately** for your object graph depth
4. **Consider custom rules** for properties that need specific patterns
5. **Test with your data models** to ensure expected behavior

## Performance Considerations

- Objects are cached per type for reuse
- First call per type generates all cache entries at once
- Subsequent calls are very fast (just random selection from cache)
- Nested objects share caches across parent objects
- Consider smaller cache sizes for very large object graphs

## Troubleshooting

### Nested objects are null
Make sure you've added rules for each nested complex type:
```csharp
generator.Rules.Add(new Rule(typeof(NestedType), "", g => g.CreateComplexObject(), "RuleName"));
```

### Stack overflow or deep recursion
Reduce the maxDepth parameter:
```csharp
Funcs.CreateComplexObjectTyped<T>(generator, maxDepth: 2);
```

### Same objects returned every time
This is expected behavior - objects are cached and randomly reused. Increase cacheSize for more variety:
```csharp
Funcs.CreateComplexObjectTyped<T>(generator, cacheSize: 50);
```
