using Script.Manager.Util.Log;
using UniRx;
using UnityEngine;

namespace Script.Event.Test
{
    public class EventData
    {
        public string Str1;
        public string Str2;

        public int Int1;
        public int Int2;
        
        public EventData(string str1, string str2, int int1, int int2)
        {
            Str1 = str1;
            Str2 = str2;
            Int1 = int1;
            Int2 = int2;
        }
    }
    
    public class EventDataTest
    {
        private EventMonoCtrl _ctrl;
        private EventData _data;

        private CompositeDisposable _disposable = new CompositeDisposable();
        public EventDataTest(EventData data, EventMonoCtrl ctrl)
        {
            _data = data;
            _ctrl = ctrl;
            
            BindEvent();
        }

        private void BindEvent()
        {
            _ctrl.DoPlusEvent
                .Subscribe(PlusData)
                .AddTo(_disposable);
            
            _ctrl.DoMinusEvent
                .Subscribe(MinusData)
                .AddTo(_disposable);
            
            _ctrl.ClearEvent
                .Subscribe(ClearData)
                .AddTo(_disposable);
            
            _ctrl.PlusStrEvent
                .Subscribe(PlusRandomString)
                .AddTo(_disposable);
            
            _ctrl.MinusStrEvent
                .Subscribe(MinusRandomString)
                .AddTo(_disposable);

            _ctrl.SetRandomString
                .Subscribe(SetNewStringData)
                .AddTo(_disposable);
        }

        private void PlusData()
        {
            _data.Int1++;
            _data.Int2++;
            _ctrl.UpdateData.Execute();
        }

        private void MinusData()
        {
            _data.Int1--;
            _data.Int2--;
            _ctrl.UpdateData.Execute();
        }

        private void ClearData()
        {
            _data.Int1 = 0;
            _data.Int2 = 0;
            _data.Str1 = "";
            _data.Str2 = "";
            _ctrl.UpdateData.Execute();
        }

        private void PlusRandomString(string targetString)
        {
            var target = Random.Range(0, 2);

            if(target == 0)
            {
                _data.Str1 += targetString;
            }

            else
            {
                _data.Str2 += targetString;
            }

            _ctrl.UpdateData.Execute();
        }

        private void MinusRandomString()
        {
            var target = Random.Range(0, 2);

            if (target == 0 && _data.Str1.Length > 0)
            {
                _data.Str1 = _data.Str1.Substring(0, _data.Str1.Length - 1);
            }

            else if(target != 0 && _data.Str2.Length > 0)
            {
                _data.Str2 = _data.Str2.Substring(0, _data.Str2.Length - 1);
            }

            _ctrl.UpdateData.Execute();
        }

        private void SetNewStringData()
        {
            _ctrl.TestString.Value = "TestData : " + Random.Range(0, 100);
        }

        public EventData GetData()
        {
            if (_data == null)
            {
                Log.EF("Test", "data is null");
                return null;
            }
            
            return _data;
        }
    }
}