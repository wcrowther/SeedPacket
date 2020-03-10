
using SeedPacket.Exceptions;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WildHare.Extensions;

namespace SeedPacket.DataSources
{
    public class CsvDataSource : IDataSource
    {
        private MultiDimensionalList<string,string> sourceData;
        private const string defaultCsv = "SeedPacket.Source.CsvGeneratorSource.csv";

        public CsvDataSource()
        {
            LoadDefaultData();
        }

        public void Parse(string csv, string source = null)
        {
            //try
            //{
                sourceData = CSVHelper(csv);
            //}
            //catch(Exception innerEx)
            //{
             //   throw new InvalidSourceException("csv", source, innerEx);
            //}
        }

        public void Load(string sourceFilePath)
        {
            string csvString;

            if (sourceFilePath.StartsWith("~"))
            {
                throw new InvalidTildePathException(sourceFilePath);
            };

            try
            {
                csvString = File.ReadAllText(sourceFilePath);
            }
            catch(Exception innerEx)
            {
                throw new InvalidFilePathException(sourceFilePath, innerEx);
            }

            Parse(csvString, "file");
        }

        public void LoadDefaultData()
        {
            try
            {
                var a = Assembly.GetExecutingAssembly();
                using (var stream = a.GetManifestResourceStream(defaultCsv))
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Parse(result);
                };
            }
            catch
            {
                throw new InvalidDefaultDataException();
            }
        }

        public List<string> GetElementList(string identifier)
        {
            if (sourceData != null && !identifier.IsNullOrEmpty())
            {
                return sourceData[identifier];
            }
            return new List<string>();
        }

        public List<T> GetObjectList<T>(string identifier) where T : class, new()
        {
            throw new NotImplementedException();

            //--------------------------------------------------------------------------------------
            // TODO - Currently this code only does one item and is not in a loop. Need to fix that.
            //--------------------------------------------------------------------------------------
            // ALSO - Consider using WildHare GetStart and GetEnd
            // ALSO - Implement * for empty cell
            // ALSO - Tests around quoted and not quoted values particularily when they contain quotes
            // ALSO - Once they are working. Refactor MultiDimensionalList ??

            //if (sourceData != null && !identifier.IsNullOrEmpty())
            //{
            //    var itemObjectList = sourceData.Where(w => w.Key.StartsWith($"{identifier}_"));

            //    var instance = new T();
            //    foreach (var property in typeof(T).GetMetaProperties())
            //    {
            //        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            //        var itemValuesList = itemObjectList.SingleOrDefault(w => w.Key.EndsWith($"_{property.Name}")).Value;
            //        var value = itemValuesList?.ElementAtOrDefault(0);
            //        try
            //        {
            //            if (value != null)
            //            {
            //                if (property.CanWrite)
            //                {
            //                    property.SetInstanceValue(instance, Convert.ChangeType(value, propertyType));
            //                }
            //            }
            //        }
            //        catch (Exception ex) // If Error let the value remains default for that property type
            //        {
            //            Debug.WriteLine($"Not able to parse CSV value {value} for type '{property.PropertyType}'" +
            //                            $" for property {property.Name}. Ex: {ex.Message}");
            //        }
            //    }
            //}
            //return new List<T>();
        }

        // source: https ://stackoverflow.com/questions/2081418/parsing-csv-files-in-c-with-header
        // source: https: //stackoverflow.com/questions/1596530/multi-dimensional-arraylist-or-list-in-c
        // ONLINE XML to CSV Converter: https: //json-csv.com/xml

        private MultiDimensionalList<string,string> CSVHelper(string csv, char separator = ',')
        {
            string[] removeArray = {"\"", "\n", "\n\r", "\"\r" };
            var multi = new MultiDimensionalList<string,string>();

            string[] rows = csv.Split('\n').Where(w => !w.IsNullOrEmpty()).ToArray();

            string[] cols = rows[0].Split(separator).Select(s => s.RemoveStartEnd(removeArray) ).ToArray(); // First Line Has Column Headers

            for (int i = 1; i < rows.Count(); i++)
            {
                string[] dataList = rows[i].Split(separator);
                for (int x = 0; x < dataList.Count(); x++)
                {
                    string name = cols[x].RemoveStartEnd(removeArray);
                    string data = dataList[x].RemoveStartEnd(removeArray);

                    if (!name.IsNullOrEmpty() && !data.IsNullOrEmpty())
                    {
                        multi.Add(name, data);
                    }
                }
            }
            return multi;
        }
    }

    public class MultiDimensionalList<K, T> : Dictionary<K, List<T>>
    {
        public void Add(K key, T addObject, bool makeDistinct = false)
        {
            if (!ContainsKey(key))
            {
                Add(key, new List<T>());
            }

            if (base[key].Contains(addObject) && makeDistinct)
                return;

            base[key].Add(addObject);
        }
    }
}
