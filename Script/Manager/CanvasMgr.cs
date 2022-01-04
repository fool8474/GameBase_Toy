using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Util;
using UnityEngine;

namespace Script.Manager
{
    public class CanvasMgr : ScriptMgr
    {
        private ResourceMgr _prefabMgr;

        private Canvas _fixedCanvas;    // order : 100, 고정되어 씬 내에서 지속적으로 사용되는 UI
        private Canvas _mainCanvas;     // order : 500, 대부분의 UI
        private Canvas _popupCanvas;    // order : 1000, 일부의 팝업형 childUI나 공용 팝업으로 나타나는 UI
        private Canvas _loadingCanvas;  // order : 1500, 로딩 UI 전용

        public override void Initialize()
        {
            AddCanvas().Forget();
        }

        public override void Inject()
        {
            _prefabMgr = Injector.GetInstance<ResourceMgr>();
        }
        
        public void MoveObjToCanvas(UIType type, GameObject obj)
        {
            switch (type)
            {
                case UIType.MAIN:
                {
                    obj.transform.SetParent(_mainCanvas.transform);
                }
                break;
                case UIType.FIXED:
                {
                    obj.transform.SetParent(_fixedCanvas.transform);
                }
                break;
                case UIType.POPUP:
                {
                    obj.transform.SetParent(_popupCanvas.transform);
                }
                break;
                case UIType.LOADING:
                {
                    obj.transform.SetParent(_loadingCanvas.transform);
                }
                break;
            }
        }

        // 새로운 canvas가 생긴다면 등록해야 함.
        private async UniTaskVoid AddCanvas()
        {
            _mainCanvas = await _prefabMgr.InstantiateObjId<Canvas>(AddressableID.CANVAS_MAIN);
            _fixedCanvas = await _prefabMgr.InstantiateObjId<Canvas>(AddressableID.CANVAS_FIXED);
            _popupCanvas = await _prefabMgr.InstantiateObjId<Canvas>(AddressableID.CANVAS_POPUP);
            _loadingCanvas = await _prefabMgr.InstantiateObjId<Canvas>(AddressableID.CANVAS_LOADING);
        }
    }
}