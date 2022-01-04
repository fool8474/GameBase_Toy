using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.UI.Models;
using Script.Util;
using UnityEngine;

namespace Script.UI.Views
{
    public class TestHeavyView : View<TestHeavyModel>
    {
        private List<GameObject> _cylinderList;

        public override void Initialize()
        {
            base.Initialize();
            _cylinderList = new List<GameObject>();
        }

        protected override async UniTask InitializeWithVisible()
        {
            await base.InitializeWithVisible();
            
            for (var i = 0; i < 10000; i++)
            {
                var newObject = await _objPoolMgr.GetObject(AddressableID.TEST_CYLINDER);
                newObject.transform.position = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
                newObject.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                
                _cylinderList.Add(newObject);
            }
        }

        protected override void FinalizeHide()
        {
            for (var i = 0; i < _cylinderList.Count;)
            {
                _objPoolMgr.ReturnObject(AddressableID.TEST_CYLINDER, _cylinderList[0]);
                _cylinderList.RemoveAt(0);
            }
            
            base.FinalizeHide();
        }
    }
}