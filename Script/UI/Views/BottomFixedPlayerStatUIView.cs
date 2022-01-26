using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Manager.Player;
using Script.UI.ExtraUI;
using Script.UI.FixedUISetters;
using Script.UI.Models;
using Script.Util;
using Script.Client.Util;
using UniRx;
using UnityEngine;
using Script.Inject;

namespace Script.UI.Views
{
    public class BottomFixedPlayerStatUIView : View<BottomFixedPlayerStatUIModel>
    {
        [SerializeField] private SerializableDictionary<MoneyType, PlayerMoneyStatUI> _playerMoneyStatUIDic;

        private PlayerMoneyHolder _moneyHolder;
        
        public override void Initialize()
        {
            base.Initialize();

            AddInputKey(KeyCode.Alpha1, () => AddMoneyTest(MoneyType.COIN));
            AddInputKey(KeyCode.Alpha2, () => AddMoneyTest(MoneyType.DIAMOND));

            foreach(var statUI in _playerMoneyStatUIDic.Values)
            {
                statUI.InitData();
            }

            _model.SetUISetter
                .Subscribe(SetUISetter)
                .AddTo(_disposable);

            _model.UpdateNewValue
                .Subscribe(UpdateStatusUI)
                .AddTo(_disposable);

            _moneyHolder
                .SetMoneyValue
                .Subscribe(newValue => UpdateStatusUI(newValue.type))
                .AddTo(_disposable);
        }
        public override void Inject(Model model)
        {
            base.Inject(model);

            _moneyHolder = Injector.GetInstance<PlayerStatus>().GetHolder<PlayerMoneyHolder>();
        }

        private void AddMoneyTest(MoneyType type)
        {
            _model.UpdateNewValue.Execute(type);
        }

        private void UpdateStatusUI(MoneyType type)
        {
            var statusUI = _playerMoneyStatUIDic[type];
            
            if(statusUI.gameObject.activeSelf == false)
            {
                return;
            }

            statusUI.UpdateValue();
        }

        private void SetUISetter(BottomFixedPlayerStatUISetter uiSetter)
        {
            foreach(var statUI in _playerMoneyStatUIDic.Values)
            {
                statUI.gameObject.SetActive(false);
            }

            if (uiSetter.ShowCoin)
            {
                ActivePlayerMoneyStat(MoneyType.COIN);
            }

            if(uiSetter.ShowDiamond)
            {
                ActivePlayerMoneyStat(MoneyType.DIAMOND);
            }
        }

        private void ActivePlayerMoneyStat(MoneyType type)
        {
            _playerMoneyStatUIDic[type].gameObject.SetActive(true);
        }
    }
}