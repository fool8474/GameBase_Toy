using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager.CSV;
using Script.Table;
using Script.UI.Models;
using Script.UI.ScrollItems;

namespace Script.UI.Controllers
{
    public class TestController : Controller<TestModel>
    {
        public TestController() : base("UITest", new TestModel()) { }

        public override void InitializeWithVisible()
        {
            base.InitializeWithVisible();
            SetTestData();
        }

        private void SetTestData()
        {
            if(TableMgr.TryGetDefDic<DefPerson>(out var defPerson) == false)
            {
                return;
            }

            var dataList = new List<TestScrollData>();
            var count = UnityEngine.Random.Range(5, 30);
            
            for (var i = 0; i < count; i++)
            {
                var idx = UnityEngine.Random.Range(0, defPerson.Count);
                var defBase = defPerson.Values.ElementAt(idx);

                if (defBase is DefPerson person)
                {
                    dataList.Add(new TestScrollData(idx, person.Name, person.LastName));
                }

                else
                {
                    continue;
                }
            }

            _model.ScrollDataProperty.Value = dataList;
        }
    }
}