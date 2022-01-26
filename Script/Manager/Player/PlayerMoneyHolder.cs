using Script.Event;
using Script.Manager.Util.Log;
using Script.Util.Const;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Manager.Player
{
    public enum MoneyType
    {
        NONE = 0,
        COIN = 100,
        DIAMOND = 200,
    }
    
    [Serializable]
    public class PlayerMoneyHolder : IPlayerHolder, IDisposable
    {
        private Dictionary<MoneyType, float> _moneyDic;

        public IEventCommand<(MoneyType type, float value)> SetMoneyValue => _setMoneyValue;
        private EventCommand<(MoneyType type, float value)> _setMoneyValue = new EventCommand<(MoneyType type, float value)>();

        public void Initialize()
        {
            _moneyDic = new Dictionary<MoneyType, float>();
        }

        public void LoadData()
        {
            _moneyDic.Add(MoneyType.COIN, PlayerPrefs.GetFloat(PlayerPrefsConst.COIN, 0f));
            _moneyDic.Add(MoneyType.DIAMOND, PlayerPrefs.GetFloat(PlayerPrefsConst.DIAMOND, 0f));
        }

        public float GetMoney(MoneyType type)
        {
            if(_moneyDic.TryGetValue(type, out var value) == false)
            {
                Log.EF(LogCategory.STORAGE, "No MoneyType in dictionary {0}", type);
                return default;
            }

            return value;
        }

        public void SetMoney(MoneyType type, float value)
        {
            if (_moneyDic.ContainsKey(type) == false)
            {
                _moneyDic.Add(type, value);
            }

            _moneyDic[type] = value;
            SetMoneyValue.Execute((type, _moneyDic[type]));
        }

        public void AddMoney(MoneyType type, float calValue)
        {
            if (_moneyDic.ContainsKey(type) == false)
            {
                SetMoney(type, calValue);
                return;
            }

            SetMoney(type, _moneyDic[type] + calValue);
        }

        public void Dispose()
        {
            _setMoneyValue.Dispose();
        }
    }
}