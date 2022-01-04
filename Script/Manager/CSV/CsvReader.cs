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
                var isData = false;

                while(true)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    
                    ReadRow(line, isData);
                    isData = true; // 첫 열 이후에는 데이터
                }
            }

            return objects;

            void ReadRow(string data, bool isHeader)
            {
                var propertyValues = data.Split(',').ToList();

                // ID 처리 (*모든 테이블에는 ID가 0번쨰로 존재하도록 한다)
                propertyValues.Add(propertyValues.ElementAt(0));
                propertyValues.RemoveAt(0);

                // data case
                if (isHeader)
                {
                    var obj = new T();

                    obj.AssignValuesFromCsv(propertyValues, ignoreSet);
                    objects.Add(obj);
                }

                // header case
                else
                {
                    ReadHeader(propertyValues);
                }
            }
            
            void ReadHeader(IReadOnlyList<string> propertyValues)
            {
                for (var i = 0; i < propertyValues.Count; i++)
                {
                    // 이름에 memo가 포함된 열의 경우 무시하도록 한다.
                    if (propertyValues[i].Contains("Memo"))
                    {
                        ignoreSet.Add(i);
                    }
                }
            }
        }
    }
}