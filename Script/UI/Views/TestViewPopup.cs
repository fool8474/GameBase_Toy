using System.Collections.Generic;
using Script.UI.Models;
using TMPro;
using UniRx;
using UnityEngine;

namespace Script.UI.Views
{
    public class TestViewPopup : View<TestModelPopup>
    {
        [SerializeField] private List<TextMeshProUGUI> _initTextList;
            
        public override void Initialize()
        {
            base.Initialize();

            _model.InitList
                .Subscribe(UpdateInitData)
                .AddTo(_disposable);
        }

        private void UpdateInitData(List<string> stringList)
        {
            for (var i = 0; i < stringList.Count && i < _initTextList.Count; i++)
            {
                _initTextList[i].text = stringList[i];
            }
        }
    }
}