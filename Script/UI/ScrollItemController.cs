using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Script.Inject;
using Script.Manager;
using Script.Manager.Util.Log;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ScrollItemController : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _trParent;
        [SerializeField] private string _itemAddressId;
        [SerializeField] private Scrollbar _scrollbar;

        private TweenerCore<float, float, FloatOptions> _tweenMove;
        
        private List<ScrollItem> _itemList;
        private ObjPoolMgr _objPoolMgr;
        
        public void Init()
        {
            Inject();
            _itemList = new List<ScrollItem>();
        }
        
        private void Inject()
        {
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }

        public async void Build<TData, TItem>(List<TData> scrollData) 
            where TData : ScrollData 
            where TItem : ScrollItem
        {
            if (_itemList.Count > scrollData.Count)
            {
                ReturnScrollItem(scrollData.Count);
            }
        
            var prevItemCount = _itemList.Count;

            for (var i = 0; i < scrollData.Count; i++)
            {
                TItem item = null;
                if (prevItemCount > i)
                {
                    item = _itemList.ElementAt(i) as TItem;

                    if (item == null)
                    {
                        Log.EF(LogCategory.UI, "ItemData is null, {0} at count {1}", gameObject.name, i);
                        continue;
                    }
                    
                    item.UpdateData(scrollData[i]);
                    continue;
                }
                
                var go = await _objPoolMgr.GetObject(_itemAddressId, parent: _trParent);
                item = go.GetComponent<TItem>();
                item.Init(scrollData[i]);
                item.UpdateData(scrollData[i]);
                _itemList.Add(item); 
            }
        }

        private void ReturnScrollItem(int start = 0)
        {
            var count = _itemList.Count - start;
            for (var i = 0; i < count; i++)
            {
                var item = _itemList[start + i];
                item.Dispose();
                _objPoolMgr.ReturnObject(_itemAddressId, item.gameObject);
            }
            
            _itemList.RemoveRange(start, count);
        }
     
        public void SetScrollIdx(int index, bool immediately = false)
        {
            if (_itemList.Count < 1)
            {
                return;
            }

            var scrollValue = math.clamp(1 - index / (float)(_itemList.Count - 1), 0.0f, 1.0f);
            
            if (immediately)
            {
                _scrollbar.value = scrollValue;
            }

            else
            {
                _tweenMove?.Kill();
                _tweenMove = DOTween.To(() => _scrollbar.value, value => _scrollbar.value = value, scrollValue,
                    1.0f).SetEase(Ease.OutQuad).SetAutoKill(true);
                _tweenMove.Play();
            }
        }
        
        public List<ScrollItem> GetScrollItemList()
        {
            return _itemList;
        }

        private void Update()
        {
            UpdatedVisibleOnOff();
        }

        private void UpdatedVisibleOnOff()
        {
            // myRectTransform.IsFullyVisibleFrom(myCamera);
        }

        public void Dispose()
        {
            _tweenMove?.Kill();
            
            _tweenMove = null;
            ReturnScrollItem();
        }
    }
}