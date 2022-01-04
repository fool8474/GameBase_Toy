using System.Linq;
using Script.InGame.PuzzleBlockFactory;
using Script.Inject;
using Script.Manager.CSV;
using Script.Manager.Util.Log;
using Script.Table;
using UnityEngine;

namespace Script.Manager
{
    // 게임에서 필요한 것들의 Inject, 데이터 초기화 등 실시
    public class GameMgr : MonoBehaviour
    {
        [SerializeField] private GameObject goPrefabManager;
        
        private void Start()
        {
            InjectFirst();
            InjectScriptable();
            InjectMono();
        }

        private void InjectFirst()
        {
            Injector.RegisterTypeMono(goPrefabManager.GetComponent<ResourceMgr>());
            Injector.RegisterTypeMono<ObjPoolMgr>("Manager/ObjectPoolManager");
        }
        
        private void InjectScriptable()
        {
            Injector.RegisterTypeScript<TableMgr>();
            Injector.RegisterTypeScript<AtlasMgr>();
            Injector.RegisterTypeScript<CanvasMgr>();
            Injector.RegisterTypeScript<UIMgr>();
            Injector.RegisterTypeScript<UITransitionMgr>();
            Injector.RegisterTypeScript<PopupMgr>();
            Injector.RegisterTypeScript<PuzzleBlockFactoryMgr>();
        }

        private void InjectMono()
        {
            Injector.RegisterTypeMono<TestMgr>("Manager/TestMgr");
        }
    }
}
