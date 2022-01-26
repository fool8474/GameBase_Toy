using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using System;
using System.Collections.Generic;

namespace Script.Manager.Player
{
    [Serializable]
    public class PlayerStatus : ScriptMgr
    {
        private Dictionary<Type, IPlayerHolder> _holderDic;

        public override void Initialize()
        {
            RegisterHolder();
        }

        public T GetHolder<T>() where T : class, IPlayerHolder
        {
            if(_holderDic.TryGetValue(typeof(T), out var holder) == false)
            {
                Log.EF(LogCategory.STORAGE, "No holder type in dic {0}", typeof(T));
                return null;
            }

            return holder as T;
        }

        private void RegisterHolder()
        {
            _holderDic = new Dictionary<Type, IPlayerHolder>();
            RegisterHolder<PlayerMoneyHolder>();
        }

        private void RegisterHolder<T>() where T : class, IPlayerHolder, new()
        {
            if(_holderDic.ContainsKey(typeof(T)))
            {
                Log.WF(LogCategory.STORAGE, "Cannot add holder to holderDic {0}", typeof(T));
                return;
            }

            var holder = new T();
            holder.Initialize();
            holder.LoadData();
            _holderDic.Add(typeof(T), holder);
        }
    }
}