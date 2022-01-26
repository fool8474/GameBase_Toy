using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Table;
using UnityEngine;
using UnityEngine.Networking;

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
                var fileList = Resources.LoadAll<TextAsset>(TableUtil.TABLE_DESTINATION);

                foreach (var currFile in fileList)
                {
                    if (_tblTypeDic.TryGetValue(currFile.name, out var tblTypeFunc) == false)
                    {
                        Log.EF("Table", "Cannot find fileName from TypeDic, {0}", currFile.name);
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

        public static Dictionary<int, DefBase> GetDefDic<TDef>() where TDef : DefBase
        {
            var defDic = _defListDic[typeof(TDef)];

            if (defDic == null)
            {
                Log.WF(LogCategory.TABLE, "No DefList from type {0}", typeof(TDef).Name);
                return null;
            }

            return defDic;
        }

        // TDef 타입의 가공된 Dictionary형 테이블 데이터를 가져옴
        public static bool TryGetDefDic<TDef>(out Dictionary<int, DefBase> baseDic)
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
        public static bool TryGetDef<TDef>(int id, out TDef rtnDef)
            where TDef : DefBase
        {
            rtnDef = null;

            if(TryGetDefDic<TDef>(out var baseDic) == false)
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
            RegisterTableContent<TblPerson, DefPerson>();
            RegisterTableContent<TblAddress, DefAddress>();
            RegisterTableContent<TblBlockNormal, DefBlockNormal>();
            RegisterTableContent<TblBlockObstacle, DefBlockObstacle>();
            RegisterTableContent<TblBlockTypeSet, DefBlockTypeSet>();
            RegisterTableContent<TblStage, DefStage>();
            RegisterTableContent<TblStageBlockInfo, DefStageBlockInfo>();
            RegisterTableContent<TblPurchase, DefPurchase>();
            RegisterTableContent<TblMoneyType, DefMoneyType>();
            RegisterTableContent<TblReward, DefReward>();
            RegisterTableContent<TblRewardMoney, DefRewardMoney>();
        }

        private void RegisterTableContent<TTbl, TDef>() 
            where TTbl : TblBase, new()
            where TDef : DefBase
        {
            var tblTypeName = typeof(TTbl).Name;
            _tblTypeDic.Add(tblTypeName, (() => CsvReader<TTbl>.Read(tblTypeName), typeof(TDef)));
            _defListDic.Add(typeof(TDef), new Dictionary<int, DefBase>());
        }
    }
}