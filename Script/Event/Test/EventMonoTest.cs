using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Event.Test
{
    public class EventMonoTest : MonoBehaviour, IDisposable
    {
        private EventDataTest _data;
        private EventMonoCtrl _eventCtrl;

        public Button BtnPlus;
        public Button BtnMinus;
        public Button BtnClear;
        public Button BtnStr1;
        public Button BtnStr2;
        public Button BtnStr3;

        public TextMeshProUGUI TxtInt1;
        public TextMeshProUGUI TxtInt2;
        public TextMeshProUGUI TxtStr1;
        public TextMeshProUGUI TxtStr2;
        public TextMeshProUGUI TxtPropertyStr;

        private readonly CompositeDisposable _disposable = new CompositeDisposable(); 
        
        private void Start()
        {
            _eventCtrl = new EventMonoCtrl();
            _data = new EventDataTest(new EventData("TestStr1", "TestStr2", 0, 0), _eventCtrl);

            BindEvent();
            _eventCtrl.UpdateData.Execute();
        }

        private void BindEvent()
        {
            BtnPlus.OnClickAsObservable().Subscribe(_=>
            {
                _eventCtrl.DoPlusEvent.Execute();
            }).AddTo(_disposable);

            BtnMinus.OnClickAsObservable().Subscribe(_ =>
            {
                _eventCtrl.DoMinusEvent.Execute();
            }).AddTo(_disposable);

            BtnClear.OnClickAsObservable().Subscribe(_ =>
            {
                _eventCtrl.ClearEvent.Execute();
            }).AddTo(_disposable);

            BtnStr1.OnClickAsObservable().Subscribe(_ =>
            {
                _eventCtrl.PlusStrEvent.Execute(UnityEngine.Random.Range(0, 10).ToString());
            }).AddTo(_disposable);
            
            BtnStr2.OnClickAsObservable().Subscribe(_ =>
            {
                _eventCtrl.MinusStrEvent.Execute();
            }).AddTo(_disposable);

            BtnStr3.OnClickAsObservable().Subscribe(_ =>
            {
                _eventCtrl.SetRandomString.Execute();
            }).AddTo(_disposable);
            
            _eventCtrl.UpdateData
                .Subscribe(UpdateData)
                .AddTo(_disposable);
            
            _eventCtrl.TestString
                .Subscribe(UpdatePropertyData)
                .AddTo(_disposable);
        }
        
        private void UpdateData()
        {
            var data = _data.GetData();
            TxtInt1.text = data.Int1.ToString();
            TxtInt2.text = data.Int2.ToString();
            TxtStr1.text = data.Str1;
            TxtStr2.text = data.Str2;
        }

        private void UpdatePropertyData(string value)
        {
            TxtPropertyStr.text = value;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
    
    public class EventMonoCtrl : IDisposable
    {
        public IEventCommand DoPlusEvent => _doPlusEvent;
        public IEventCommand DoMinusEvent => _doMinusEvent;
        public IEventCommand ClearEvent => _clearEvent;
        public IEventCommand UpdateData => _updateData;
        public IEventCommand<string> PlusStrEvent => _plusStrEvent;
        public IEventCommand MinusStrEvent => _minusStrEvent;
        public IEventCommand SetRandomString => _setRandomString;

        public IEventProperty<string> TestString => _testString;

        private readonly EventCommand _doPlusEvent = new EventCommand();
        private readonly EventCommand _doMinusEvent = new EventCommand();
        private readonly EventCommand _clearEvent = new EventCommand();
        private readonly EventCommand _updateData = new EventCommand();
        private readonly EventCommand<string> _plusStrEvent = new EventCommand<string>();
        private readonly EventCommand _minusStrEvent = new EventCommand();
        private readonly EventCommand _setRandomString = new EventCommand();
        
        private readonly EventProperty<string> _testString = new EventProperty<string>();
        public void Dispose()
        {
            _doPlusEvent.Dispose();
            _doMinusEvent.Dispose();
            _clearEvent.Dispose();
            _updateData.Dispose();
            _plusStrEvent.Dispose();
            _minusStrEvent.Dispose();
            
            _testString.Dispose();
        }
    }
}