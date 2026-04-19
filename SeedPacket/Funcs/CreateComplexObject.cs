using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        /// <summary>
        /// Creates and populates a complex object with random data by iterating through its public properties
        /// and applying existing rules. Caches generated objects for reuse. Handles nested objects recursively
        /// up to the configured maximum depth. This is a non-generic wrapper for use in Rules.
        /// </summary>
        /// <param name="generator">The generator instance containing rules, cache, and randomization</param>
        /// <returns>A populated instance of the current property type, or null if max recursion depth is exceeded</returns>
        public static dynamic CreateComplexObject(this IGenerator generator)
        {
            // Get the type from the current property
            var propertyType = generator.CurrentProperty?.PropertyType;

            if (propertyType == null || !propertyType.IsClass || propertyType == typeof(string))
            {
                return null;
            }

            // Use reflection to call the generic version
            MethodInfo method = typeof(Funcs).GetMethod(nameof(CreateComplexObjectTyped), BindingFlags.Public | BindingFlags.Static);
            MethodInfo genericMethod = method.MakeGenericMethod(propertyType);

            return genericMethod.Invoke(null, new object[] { generator, 5, 10 });
        }

        /// <summary>
        /// Creates and populates a complex object with random data by iterating through its public properties
        /// and applying existing rules. Caches generated objects for reuse. Handles nested objects recursively
        /// up to the configured maximum depth.
        /// </summary>
        /// <typeparam name="T">The type of complex object to create</typeparam>
        /// <param name="generator">The generator instance containing rules, cache, and randomization</param>
        /// <param name="maxDepth">Maximum recursion depth to prevent infinite loops (default: 5)</param>
        /// <param name="cacheSize">Number of objects to generate and cache for reuse (default: 10)</param>
        /// <returns>A populated instance of type T, or null if max recursion depth is exceeded</returns>
        public static T CreateComplexObjectTyped<T>(this IGenerator generator, int maxDepth = 5, int cacheSize = 10) where T : class, new()
        {
            ExpandoObject cache = generator.Cache;
            string typeName = typeof(T).FullName;

            // Track recursion depth to prevent infinite loops
            string depthKey = $"ComplexObjectDepth.{typeName}";
            int currentDepth = cache.Get<int?>(depthKey) ?? 0;

            // If we've exceeded max depth, return null to break recursion
            if (currentDepth >= maxDepth)
            {
                return null;
            }

            // Check if we have cached objects of this type
            string cacheKey = $"ComplexObjectCache.{typeName}";
            List<T> cachedObjects = cache.Get<List<T>>(cacheKey);

            // If no cache exists, generate and cache objects
            if (cachedObjects == null || cachedObjects.Count == 0)
            {
                cachedObjects = new List<T>();

                // Increment depth counter before generating objects
                if (cache.Get<int?>(depthKey) != null)
                    cache.Remove(depthKey);
                cache.Add(depthKey, currentDepth + 1);

                try
                {
                    // Generate the specified number of objects for the cache
                    for (int i = 0; i < cacheSize; i++)
                    {
                        T newObject = CreateAndPopulateObject<T>(generator);
                        if (newObject != null)
                        {
                            cachedObjects.Add(newObject);
                        }
                    }

                    // Store the cached objects
                    if (cache.Get<List<T>>(cacheKey) != null)
                        cache.Remove(cacheKey);
                    cache.Add(cacheKey, cachedObjects);
                }
                finally
                {
                    // Decrement depth counter after generation
                    if (cache.Get<int?>(depthKey) != null)
                        cache.Remove(depthKey);
                    cache.Add(depthKey, currentDepth);
                }
            }

            // Return a random object from the cache
            if (cachedObjects.Count > 0)
            {
                int index = generator.RowRandom.Next(cachedObjects.Count);
                return cachedObjects[index];
            }

            return null;
        }

        /// <summary>
        /// Internal method that creates a single instance and populates its properties using rules
        /// </summary>
        private static T CreateAndPopulateObject<T>(IGenerator generator) where T : class, new()
        {
            // Get all writable public properties
            var metaProperties = typeof(T).GetMetaProperties();

            // Create new instance
            T newItem = Activator.CreateInstance<T>();

            // Store the previous property to restore it after processing
            var previousProperty = generator.CurrentProperty;

            try
            {
                // Iterate through each property and populate it
                for (int i = 0; i < metaProperties.Count; i++)
                {
                    var property = metaProperties[i];

                    // Skip read-only properties
                    if (!property.CanWrite)
                        continue;

                    // Set current property for rule matching
                    generator.CurrentProperty = property;

                    // Get the appropriate rule for this property
                    Rule rule = generator.Rules.GetRuleByTypeAndName(property.PropertyType, property.Name);

                    // Apply the rule if one was found
                    if (rule != null)
                    {
                        try
                        {
                            dynamic seedValue = rule.ApplyRule(generator);
                            property.SetInstanceValue(seedValue, newItem);
                        }
                        catch
                        {
                            // If rule application fails, leave property at default value
                            // This prevents failures in nested object creation from breaking the whole process
                        }
                    }
                }
            }
            finally
            {
                // Restore the previous property
                generator.CurrentProperty = previousProperty;
            }

            return newItem;
        }
    }
}
