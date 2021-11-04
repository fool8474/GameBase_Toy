using System;
using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager;
using UnityEngine;

namespace Script.UI
{
    public interface IController
    {
        void Initialize();
        void InitializeWithVisible();
        void InitializeWithData(UIInitData data);
        void SetVisible(bool isVisible);
        void OnQuit();
        UIData GetUIData();
        bool IsVisible();
    }
    
    public class Controller : IController
    {
        protected IView _view;
        protected Model _model;
        protected bool _isVisible;

        private PrefabMgr _prefabMgr;
        
        protected string prefabId;
        
        protected Controller(string id, Model model)
        {
            prefabId = id;
            _model = model;
            
            Inject();
            Initialize();
        }

        public virtual void Initialize()
        {
            var loadTask = _prefabMgr.InstantiateObjId<View>(prefabId).GetAwaiter();

            loadTask.OnCompleted(() =>
            {
                 _view = loadTask.GetResult();
                 _view.BindStart(_model);
                 _view.Initialize();
            });
        }
        
        public virtual void Inject()
        {
            _prefabMgr = Injector.GetInstance<PrefabMgr>();
        }
        
        public virtual void InitializeWithVisible()
        {
            
        }

        public virtual void InitializeWithData(UIInitData data)
        {
            
        }

        public virtual void SetVisible(bool isVisible)
        {
            _view.SetVisible(isVisible).Forget();
        }

        public virtual void OnQuit()
        {
               
        }
        
        public virtual bool IsVisible()
        {
            return _isVisible;
        }

        public UIData GetUIData()
        {
            return _model.UIData;
        }
    }
}