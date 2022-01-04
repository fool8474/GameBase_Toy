using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Table;
using UnityEngine;

namespace Script.Manager.CSV
{
    public class TableMgr : ScriptMgr
    {
        private static Dictionary<string, (Func<IEnumerable> readFunc, Type type)> _tblTypeDic;
        private static Dictionary<Type, Dictionary<int, DefBase>> _defListDic;
        
        public override void Initialize()
        {
            var dataList = new List<(IEnumerable list, Type type)>();

            _tblTypeDic = new Dictionary<string, (Func<IEnumerable> readFunc, Type type)>();
            _defListDic = new Dictionary<Type, Dictionary<int, DefBase>>();
            
            RegisterTable();
            ReadCSV();
            
            // Tbl의 build가 끝난 뒤 Def에서 Tbl의 데이터를 가져오기 때문에, Tbl과 Def를 나누어 루프를 2번 돈다.
            BuildTbl();
            BuildDef();
            
            void ReadCSV()
            {
                var fileList = Directory.GetFiles(Application.streamingAssetsPath + TableUtil.TABLE_DESTINATION);

                foreach (var currFile in fileList)
                {
                    // meta file이 함께 들어오므로
                    var ext = Path.GetExtension(currFile);
                    if (ext != ".csv")
                    {
                        continue;
                    }

                    var fileName = Path.GetFileNameWithoutExtension(currFile);
                    if (_tblTypeDic.TryGetValue(fileName, out var tblTypeFunc) == false)
                    {
                        Log.EF("Table", "Cannot find fileName from TypeDic, {0}", fileName);
                        continue;
                    }
                
                    var readList = tblTypeFunc.readFunc.Invoke();
                    dataList.Add((readList, tblTypeFunc.type));
                }
            }

            void BuildTbl()
            {
                foreach (var (list, type) in dataList)
                {
                    foreach (var defData in list)
                    {
                        if (!(defData is TblBase tblBaseData))
                        {
                            continue;
                        }
                        
                        var (id, def) = tblBaseData.Build();
                        _defListDic[type].Add(id, def);
                    }
                }
            }

            void BuildDef()
            {
                foreach (var defBase in _defListDic.Values.SelectMany(defs => defs))
                {
                    defBase.Value.Build();
                }
            }
        }

        // TDef 타입의 가공된 Dictionary형 테이블 데이터를 가져옴
        public static bool TryGetDef<TDef>(out Dictionary<int, DefBase> baseDic)
            where TDef : DefBase
        {
            if(_defListDic.TryGetValue(typeof(TDef), out var result) == false)
            {
                Log.WF(LogCategory.TABLE, "No DefList from type {0}", typeof(TDef).Name);
                baseDic = null;
                return false;
            }

            baseDic = result;
            return true;
        }
        
        // TDef 타입의 id에 부합하는 가공된 테이블 데이터를 가져옴
        public static bool TryGetDefData<TDef>(int id, out TDef rtnDef)
            where TDef : DefBase
        {
            rtnDef = null;

            if(TryGetDef<TDef>(out var baseDic) == false)
            {
                return false;
            }

            if (baseDic.TryGetValue(id, out DefBase def) == false)
            {
                Log.WF(LogCategory.TABLE, "No DefItem from dic Type {0}, id {1}", typeof(TDef).Name, id);
                return false;
            }

            if (def is TDef == false)
            {
                Log.WF(LogCategory.TABLE, "DefItem is not Type {0}, id {1}", typeof(TDef).Name, id);
                return false;
            }
            
            rtnDef = (TDef)def;
            return true;
        }
            
        // ** Tbl을 클래스 추가 시 반드시 Register해줘야 함
        private void RegisterTable()
        {
            _tblTypeDic.Add("TblPerson", (() => CsvReader<TblPerson>.Read("TblPerson.csv"), typeof(DefPerson)));
            _tblTypeDic.Add("TblAddress", (() => CsvReader<TblAddress>.Read("TblAddress.csv"), typeof(DefAddress)));
            
            _defListDic.Add(typeof(DefPerson), new Dictionary<int, DefBase>());
            _defListDic.Add(typeof(DefAddress), new Dictionary<int, DefBase>());
        }
    }
}