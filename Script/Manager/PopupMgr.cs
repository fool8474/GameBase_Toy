using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.UI;
using Script.UI.Controllers;
using UniRx;

namespace Script.Manager
{
    public class PopupMgr : ScriptMgr
    {
        private List<IController> _showPopupList;
        private Dictionary<Type, List<IController>> _popupDic;

        public IEventCommand BackEvent => _backEvent;
        private EventCommand _backEvent = new EventCommand();

        private CompositeDisposable _disposables = new CompositeDisposable();
        public override void Initialize()
        {
            _showPopupList = new List<IController>();
            _popupDic = new Dictionary<Type, List<IController>>();

            _backEvent
                .Subscribe(async () => await DoBack())
                .AddTo(_disposables);
        }

        private void Dispose()
        {
            _backEvent.Dispose();
        }

        private List<IController> AddListToPopupDictionary(Type type)
        {
            var popupList = new List<IController>();
            _popupDic.Add(type, popupList);

            return popupList;
        }

        public async void ShowPopup<T>() where T : IController, new()
        {
            if(_popupDic.TryGetValue(typeof(T), out var popupList) == false)
            {
                popupList = AddListToPopupDictionary(typeof(T));
            }

            if(popupList.Count == 0)
            {
                popupList.Add(new T());   
            }

            var controller = popupList[0];
            _showPopupList.Add(controller);
            popupList.RemoveAt(0);

            await controller.SetVisible(true);
        }

        public bool HasPopup()
        {
            return _showPopupList.Count > 0;
        }

        private async UniTask DoBack()
        {
            if(HasPopup() == false)
            {
                return;
            }

            var target = _showPopupList.LastOrDefault();
            await target.SetVisible(false);
            ReturnPopup(target);

            _showPopupList.RemoveAt(_showPopupList.Count - 1);
        }

        private void ReturnPopup(IController controller)
        {
            Type type = controller.GetType();
            if (_popupDic.TryGetValue(type, out var popupList) == false)
            {
                popupList = AddListToPopupDictionary(type);
            }

            popupList.Add(controller);
        }

        public async UniTask ClearPopup()
        {
            var anims = new List<UniTask>();
            foreach (var controller in _showPopupList)
            {
                var task = controller.SetVisible(false);
                anims.Add(task);
                ReturnPopup(controller);
            }

            await UniTask.WhenAll(anims);
            _showPopupList.Clear();
        }
    }
}