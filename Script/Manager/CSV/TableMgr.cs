using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Script.Table;
using UnityEngine;

namespace Script.Manager.CSV
{
    public class TableMgr : ScriptMgr
    {
        private static Dictionary<string, (Func<IEnumerable> func, Type type)> _tblTypeDic;
        private static Dictionary<Type, Dictionary<int, DefBase>> _defListDic;
        
        public override void Inject()
        {

        }
        
        public override void Initialize()
        {
            var validList = new List<(IEnumerable list, Type type)>();

            _tblTypeDic = new Dictionary<string, (Func<IEnumerable> func, Type type)>();
            _defListDic = new Dictionary<Type, Dictionary<int, DefBase>>();
            
            RegisterTable();

            var fileList = Directory.GetFiles(Application.streamingAssetsPath + TableUtil.TABLE_DESTINATION);

            foreach (var currFile in fileList)
            {
                var ext = Path.GetExtension(currFile);
                if (ext != ".csv")
                {
                    continue;
                }

                var fileName = Path.GetFileNameWithoutExtension(currFile);
                if (_tblTypeDic.TryGetValue(fileName, out var tblTypeFunc) == false)
                {
                    Debug.LogErrorFormat("Cannot find fileName from TypeDic, {0}", fileName);
                    continue;
                }
                
                var enumerable = tblTypeFunc.func.Invoke();
                validList.Add((enumerable, tblTypeFunc.type));
            }

            foreach (var currDef in validList)
            {
                foreach (var defData in currDef.list)
                {
                    if(defData is TblBase tblBaseData)
                    {
                        var rtnValue = tblBaseData.Build();
                        _defListDic[currDef.type].Add(rtnValue.id, rtnValue.def); 
                    }
                }
            }
            
            foreach (var defBase in _defListDic.Values.SelectMany(defs => defs))
            {
                defBase.Value.Build();
            }
        }

        public static bool TryGetDef<TDef>(out Dictionary<int, DefBase> baseDic)
            where TDef : DefBase
        {
            if(_defListDic.TryGetValue(typeof(TDef), out var result) == false)
            {
                Debug.LogWarningFormat("No DefList from type {0}", typeof(TDef).Name);
                baseDic = null;
                return false;
            }

            baseDic = result;
            return true;
        }
        
        public static bool TryGetDefData<TDef>(int id, out TDef rtnDef)
            where TDef : DefBase
        {
            if(TryGetDef<TDef>(out var baseDic) == false)
            {
                rtnDef = null;
                return false;
            }

            if (baseDic.TryGetValue(id, out DefBase def) == false)
            {
                Debug.LogWarningFormat("No DefItem from dic Type {0}, id {1}", typeof(TDef).Name, id);
                rtnDef = null;
                return false;
            }

            if (def is TDef == false)
            {
                Debug.LogWarningFormat("DefItem is not Type {0}, id {1}", typeof(TDef).Name, id);
            }
            
            rtnDef = (TDef)def;
            return true;
        }
            
        private void RegisterTable()
        {
            _tblTypeDic.Add("TblPerson", (() => CsvReader<TblPerson>.Read("TblPerson.csv"), typeof(DefPerson)));
            _tblTypeDic.Add("TblAddress", (() => CsvReader<TblAddress>.Read("TblAddress.csv"), typeof(DefAddress)));
            
            _defListDic.Add(typeof(DefPerson), new Dictionary<int, DefBase>());
            _defListDic.Add(typeof(DefAddress), new Dictionary<int, DefBase>());
        }
    }
}