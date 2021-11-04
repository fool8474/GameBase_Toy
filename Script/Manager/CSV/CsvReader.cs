using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Script.Manager.CSV
{
    public static class CsvReader<T> where T : TblBase, new()
    {
        public static IEnumerable<T> Read(string filePath)
        {
            var objects = new List<T>();
            var ignoreSet = new HashSet<int>();

            var fullPath = Application.streamingAssetsPath + TableUtil.TABLE_DESTINATION + filePath;

            using (var sr = new StreamReader(fullPath))
            {
                var headersRead = false;

                while(true)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    
                    var propertyValues = line.Split(',').ToList();

                    // ID 처리
                    propertyValues.Add(propertyValues.ElementAt(0));
                    propertyValues.RemoveAt(0);
                    
                    if (headersRead)
                    {
                        var obj = new T();

                        obj.AssignValuesFromCsv(propertyValues, ignoreSet);
                        objects.Add(obj);
                    }

                    else
                    {
                        for (var i = 0; i < propertyValues.Count; i++)
                        {
                            if (propertyValues[i].Contains("Memo"))
                            {
                                ignoreSet.Add(i);
                            }
                        }
                        
                        headersRead = true;
                    }
                }
            }

            return objects;
        }
    }
}