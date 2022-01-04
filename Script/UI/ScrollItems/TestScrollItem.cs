using Script.Manager.Util.Log;
using Script.UI.Component;
using TMPro;
using UniRx;
using UnityEngine;

namespace Script.UI.ScrollItems
{
    public class TestScrollData : ScrollData
    {
        public int Index { get; }
        public string Name { get; }
        public string LastName { get; }

        public TestScrollData(int index, string name, string lastName)
        {
            Name = name;
            Index = index;
            LastName = lastName;
        }
    }
    
    public class TestScrollItem : ScrollItem
    {
        [SerializeField] private TextMeshProUGUI _txtName;
        [SerializeField] private TextMeshProUGUI _txtLastName;
        [SerializeField] private TextMeshProUGUI _txtIdx;
        [SerializeField] private AnButton _btnShow;

        public override void Init(ScrollData data)
        {
            base.Init(data);
            
            _btnShow
                .OnClickAsObservable()
                .Subscribe(_ => ShowDataLog())
                .AddTo(_disposable);
        }

        public override void UpdateData(ScrollData data)
        {
            base.UpdateData(data);

            if (!(data is TestScrollData testData))
            {
                Log.EF(LogCategory.UI, "Not TestScrollData, {0}", gameObject.name);
                return;
            }
            
            _txtName.text = testData.Name;
            _txtLastName.text = testData.LastName;
            _txtIdx.text = testData.Index.ToString();
        }

        private void ShowDataLog()
        {
            Log.DF(LogCategory.TEST, "TestScrollItem // {0}, {1}", _txtName.text, _txtIdx.text);
        }
    }
}