using System.Linq;
using Script.Inject;
using Script.Manager.CSV;
using Script.Table;
using UnityEngine;

namespace Script.Manager
{
    public class GameMgr : MonoBehaviour
    {
        [SerializeField] private GameObject goPrefabManager;
        
        private void Start()
        {
            InjectFirstTarget();
            InjectScriptable();
            InjectMono();

            TableMgr.TryGetDef<DefPerson>(out var persons);
            
            foreach (var currPerson in persons.Values.Cast<DefPerson>())
            {
                Debug.Log(currPerson.ToString());
            }
        }

        private void InjectFirstTarget()
        {
            Injector.RegisterTypeMono(goPrefabManager.GetComponent<PrefabMgr>());
            Injector.RegisterTypeMono<ObjPoolMgr>("Manager/ObjectPoolManager");
        }
        
        private void InjectScriptable()
        {
            Injector.RegisterTypeScript<TableMgr>();
            Injector.RegisterTypeScript<UIMgr>();
        }

        private void InjectMono()
        {
            Injector.RegisterTypeMono<TestMgr>("Manager/TestMgr");
        }
    }
}
