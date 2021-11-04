using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Manager.CSV
{
    public class TblBase
    {
        public int Id { get; set; }
        
        public void AssignValuesFromCsv(List<string> propertyValues, HashSet<int> ignoreList)
        {
            var properties = GetType().GetProperties();
            var classPropertyCnt = 0;

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
                catch (FormatException e)
                {
                    Debug.LogErrorFormat("FormatException occured at {0}, {1}", propertyInfo.Name, value);
                }

                classPropertyCnt++;
            }
        }

        public virtual (int id, DefBase def) Build()
        {
            return (0, null);
        }
    }
}