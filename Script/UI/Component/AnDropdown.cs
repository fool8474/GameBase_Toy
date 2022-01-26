using System;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.ModernUIPack;
using TMPro;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Component
{
    [RequireComponent(typeof(Animator))]
    public class AnDropdown : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IDisposable
    {
        [SerializeField] private GameObject _itemContainerGo;
        [SerializeField] private GameObject _itemObject;

        [SerializeField] private List<ItemData> _itemList = new List<ItemData>();
        [SerializeField] private AnimationType _animationType;

        [SerializeField] private Image _selectedImage;
        [SerializeField] private TextMeshProUGUI _selectedText;

        [SerializeField] private bool _useIcon = true;
        [SerializeField] private bool _useTrigger = true;
        [SerializeField] private GameObject _triggerGO;
        [SerializeField] private UnityEvent<int> _dropdownEvent;
        [SerializeField] private string _dropdownTag = "Select Item...";
        
        private int _currIdx;
        private TextMeshProUGUI _setItemText;
        private Image _setItemImage;
        private bool _isOn;
        private GameObject _listContainerGo;

        private EventTrigger _triggerEvent;
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private enum AnimationType
        {
            FADING = 0,
            SLIDING = 1,
            STYLISH = 2,
        }
        
        
        [Serializable]
        private class ItemData
        {
            public string itemName = "Dropdown Item";
            public Sprite itemIcon;
            public Action OnItemSelection;
        }

        private void Start()
        {
            if (_itemList.Count == 0)
            {
                return;
            }
            
            SetupDropdown();
            _listContainerGo = transform.parent.gameObject;

            if (_useTrigger && _triggerGO != null)
            {
                _triggerEvent = _triggerGO.AddComponent<EventTrigger>();
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => {Animate();});
                _triggerEvent.triggers.Add(entry);
            }

            // todo : Animator

            transform.SetAsLastSibling();
            
            // todo : save
        }

        private void SetupDropdown()
        {
            var parentTr = _itemContainerGo.transform;
            foreach (GameObject child in parentTr)
            {
               Destroy(child); 
            }

            _currIdx = 0;

            foreach (var item in _itemList)
            {
                var go = InstantiateItem();
                SetItemData(go, item);
                BindEvent(go, item);
            }
            
            SetSelectedDropDownData();
        }

        private void SetSelectedDropDownData()
        {
            if (_selectedImage != null && _useIcon == false)
            {
                _selectedImage.gameObject.SetActive(false);
            }

            if (_itemList.Count >= _currIdx)
            {
                var currItem = _itemList[_currIdx];
        
                _selectedText.text = currItem.itemName;
                if (_selectedImage != null)
                {
                    _selectedImage.sprite = currItem.itemIcon;
                }    
            }

            else
            {
                _selectedText.text = _dropdownTag;
            }
            
            _listContainerGo = transform.parent.gameObject;
        }

        private GameObject InstantiateItem()
        {
            var go = Instantiate(_itemObject, Vector3.zero, quaternion.identity);
            go.transform.SetParent(_itemContainerGo.transform, false);

            return go;
        }

        private void SetItemData(GameObject go, ItemData itemData)
        {
            var itemImage = go.gameObject.transform.Find("Icon")?.GetComponent<Image>();
            var itemText = go.GetComponentInChildren<TextMeshProUGUI>();

            if (itemImage)
            {
                itemImage.sprite = itemData.itemIcon;
            }

            if (itemText)
            {
                itemText.text = itemData.itemName;
            }
        }
        
        private void BindEvent(GameObject go, ItemData itemData)
        {
            // event bind
            var itemButton = go?.GetComponent<AnButton>();
            itemButton?.OnClickAsObservable().Subscribe(_ =>
            {
                var idx = go.transform.GetSiblingIndex();
                    
                Animate();
                ChangeSelectedItem(idx);
                _dropdownEvent.Invoke(idx);
                    
                // todo: save Selected
            }).AddTo(_disposable);

            if (itemData.OnItemSelection == null)
            {
                return;
            }
            
            itemButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    itemData.OnItemSelection.Invoke();
                }).AddTo(_disposable);
        }

        private void ChangeSelectedItem(int idx)
        {
            _currIdx = idx;

            if (_selectedImage != null && _useIcon)
            {
                _selectedImage.sprite = _itemList[idx].itemIcon;
            }

            if (_selectedText != null)
            {
                _selectedText.text = _itemList[idx].itemName;
            }
        
            // TODO : Sound
        }

        private void Animate()
        {
            switch (_animationType)
            {
                case AnimationType.FADING:
                {
                    
                }
                    break;
                case AnimationType.SLIDING:
                {
                    
                }
                    break;
                case AnimationType.STYLISH:
                {
                    
                }
                    break;
            }
            
            // todo : enable trigger
        }

        private void CreateNewItem(string title, Sprite icon)
        {
            var item = new ItemData();
            item.itemName = title;
            item.itemIcon = icon;
            _itemList.Add(item);
        }

        private void RemoveItem(string title)
        {
            var item = _itemList.Find(x => x.itemName == title);
            if (item != null)
            {
                _itemList.Remove(item);
            }
            SetupDropdown();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // todo : sound
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // todo : sound
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}