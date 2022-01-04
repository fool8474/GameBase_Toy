using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Manager.Util.Log;

namespace Script.Manager.CSV
{
    public class TblBase
    {
        public int Id { get; set; }
        
        // 받아온 row 데이터를 reflection을 통해 인스턴스에 대입  
        public void AssignValuesFromCsv(List<string> propertyValues, HashSet<int> ignoreList)
        {
            var properties = GetType().GetProperties();
            var classPropertyCnt = 0;

            // 열을 무시하는 케이스가 있어 (memo etc..), 루프 조건을 양쪽 다 두었음
            for (var i = 0; i < propertyValues.Count && classPropertyCnt < properties.Length; i++)
            {
                if (ignoreList.Contains(i))
                {
                    continue;
                }
                
                var propertyInfo = properties[classPropertyCnt];
                
                var type = properties[classPropertyCnt].PropertyType.Name;
                var value = propertyValues[i];

                try
                {
                    SetPropertyValue(propertyInfo, type, value);
                }
                catch (FormatException)
                {
                    Log.EF("Table", "FormatException occured at {0}, {1}", propertyInfo.Name, value);
                }

                // ignoreList가 아닌 케이스
                classPropertyCnt++;
            }
        }

        private void SetPropertyValue(PropertyInfo propertyInfo, string type, string value)
        {
            switch (type)
            {
                case "int":
                case "Int32":
                {
                    propertyInfo.SetValue(this, int.Parse(value));
                }
                    break;
                case "float":
                {
                    propertyInfo.SetValue(this, float.Parse(value));
                }
                    break;
                case "char":
                case "Char":
                {
                    propertyInfo.SetValue(this, char.Parse(value));
                }
                    break;
                default:
                {
                    propertyInfo.SetValue(this, value);
                }
                    break;
            }
        }
        
        public virtual (int id, DefBase def) Build()
        {
            return (0, null);
        }
    }
}