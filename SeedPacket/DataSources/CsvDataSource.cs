
using SeedPacket.Exceptions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using WildHare.Extensions;

namespace SeedPacket.DataSources
{
    public class CsvDataSource : IDataSource
    {
        private ListOfList<string,string> sourceData;
        private readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

        private const string itemSeparator = ".";

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
                using (var stream = a.GetManifestResourceStream(GetDefaultCsvResource()))
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
            //throw new NotImplementedException();

            //--------------------------------------------------------------------------------------
            // TODO - Currently this code only does one item and is not in a loop. Need to fix that.
            //--------------------------------------------------------------------------------------
            // ALSO - Consider using WildHare GetStart and GetEnd
            // ALSO - Implement null for empty cell
            // ALSO - Tests around quoted and not quoted values particularily when they contain quotes
            // ALSO - Once they are working. Refactor ListOfList ??

            var list = new List<T>();

            if (sourceData == null || identifier.IsNullOrEmpty())
                return list;

            var objectList = sourceData.Where(w => w.Key.StartsWith($"{identifier}{itemSeparator}"));
            var maxRows = objectList.Max(m => m.Value.Count);

            for (int i = 0; i <= maxRows-1; i++)
            {
                var row = new T();
                foreach (var property in row.GetMetaProperties())
                {
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var itemValuesList = objectList.SingleOrDefault(w => w.Key.EndsWith($"{itemSeparator}{property.Name}")).Value;
                    var value = itemValuesList?.ElementAtOrDefault(i);
                    try
                    {
                        if (value != null)
                        {
                            if (property.CanWrite)
                            {
                                property.SetInstanceValue(Convert.ChangeType(value, propertyType));
                            }
                        }
                    }
                    catch (Exception ex) // If Error let the value remains default for that property type
                    {
                        Debug.WriteLine($"Not able to parse CSV value {value} for type '{property.PropertyType}'" +
                                        $" for property {property.Name}. Ex: {ex.Message}");
                    }
                }
                list.Add(row);
            }
            return list;
        }

        private string GetDefaultCsvResource()
        {
            string sourceName;

            switch (currentCulture.Name)
            {
                case "en-US": sourceName = "SeedPacket.Source.CsvGeneratorSource.csv"; break;

                // Potentially other languages as
                // case "en-GB":  sourceName = "SeedPacket.Source.JsonGeneratorSource.json"; break;

                default: sourceName = "SeedPacket.Source.CsvGeneratorSource.csv"; break;
            }
            return sourceName;
        }



// source: https ://stackoverflow.com/questions/2081418/parsing-csv-files-in-c-with-header
// source: https: //stackoverflow.com/questions/1596530/multi-dimensional-arraylist-or-list-in-c
// ONLINE XML to CSV Converter: https: //json-csv.com/xml

    private ListOfList<string,string> CSVHelper(string csv, char separator = ',')
        {
            string[] removeArray = {"\"", "\n\r", "\n", "\"\r" };
            var multi = new ListOfList<string,string>();

            string[] rows = csv.Split('\n').Where(w => !w.IsNullOrEmpty()).ToArray();

            string[] cols = rows[0].Split(separator).Select(s => s.RemoveStartEnd(removeArray) ).ToArray(); // First Line Has Column Headers

            for (int i = 1; i < rows.Length; i++)
            {
                string[] dataList = rows[i].Split(separator);
                for (int x = 0; x < dataList.Length; x++)
                {
                    string name = cols[x].RemoveStartEnd(removeArray);
                    string data = dataList[x].RemoveStartEnd(removeArray);

                    if (!name.IsNullOrEmpty() && !data.IsNullOrEmpty())
                    {
                        if (data.ToLower() == "null")
                            data = "";

                        multi.Add(name, data);
                    }
                }
            }
            return multi;
        }

    }

    public class ListOfList<K, T> : Dictionary<K, List<T>>
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
