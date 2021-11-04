using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Inject;
using Script.Manager;
using UnityEngine;
using UnityEngine.Rendering;

namespace Script.UI
{
    public interface IView
    {
        void BindStart(Model model);
        void Initialize();
        UniTask SetVisible(bool isVisible);
    }
    
    public class View : MonoBehaviour, IView
    {
        protected Model _model;
        protected FadeDirector _director;

        protected string _prefabName = "";

        protected ObjPoolMgr _objPoolMgr;

        private void Start()
        {
            SetUIData();
        }
        
        public virtual void BindStart(Model model)
        {
            _model = model;
            _director = GetComponentInChildren<FadeDirector>();
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }
        
        public virtual void Initialize()
        {
        }
        
        public void GetUIPrefab()
        {
        }
        
        public async UniTask SetVisible(bool isVisible)
        {
            if (_director == null || _model.IsVisible == isVisible)
            {
                return;
            }

            _model.IsVisible = isVisible;
            await _director.SetVisible(isVisible);
        }

        public void SetUIData()
        {
            _model.UIData = GetComponent<UIData>();
        }
    }
}