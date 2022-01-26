using Assets.Script.Android;
using Script.InGame.PuzzleBlockFactory;
using Script.Inject;
using Script.Manager.CSV;
using Script.Manager.Player;
using Script.Manager.Puzzle;
using UnityEngine;

namespace Script.Manager
{
    // 게임에서 필요한 것들의 Inject, 데이터 초기화 등 실시
    public class GameMgr : MonoBehaviour
    {
        [SerializeField] private GameObject goPrefabManager;
        
        private void Start()
        {
            Screen.SetResolution(1920, 1080, true);
            
            SetSRDebugger();
            InitializeInjector();
        }

        private void SetSRDebugger()
        {
#if !UNITY_EDITOR
            SRDebug.Init();
#endif
        }

        private void InitializeInjector()
        {
            InjectFirst();
            InjectScriptable();
            InjectMono();
            Injector.InitializeContainer();
        }

        private void InjectFirst()
        {
            Injector.RegisterTypeMono(goPrefabManager.GetComponent<ResourceMgr>());
            Injector.RegisterTypeMono<ObjPoolMgr>("Manager/ObjectPoolManager");
            Injector.RegisterTypeScript<PlayerStatus>();
        }

        private void InjectScriptable()
        {
            Injector.RegisterTypeScript<AtlasMgr>();
            Injector.RegisterTypeScript<TableMgr>();
            Injector.RegisterTypeScript<UnityPlugins>();
            Injector.RegisterTypeScript<CanvasMgr>();
            Injector.RegisterTypeScript<UIMgr>();
            Injector.RegisterTypeScript<UITransitionMgr>();
            Injector.RegisterTypeScript<PopupMgr>();
            Injector.RegisterTypeScript<PuzzleBlockFactoryMgr>();
            Injector.RegisterTypeScript<PuzzleProcessor>();
            Injector.RegisterTypeScript<RewardMgr>();
            Injector.RegisterTypeScript<InAppMgrAndroid>();
        }

        private void InjectMono()
        {
            Injector.RegisterTypeMono<TestMgr>("Manager/TestMgr");
            Injector.RegisterTypeMono<AndroidMonobehaviour>("Manager/AndroidPlugin");
            Injector.RegisterTypeMono<InAppMgrUnity>("Manager/InAppManager");
        }
    }
}
