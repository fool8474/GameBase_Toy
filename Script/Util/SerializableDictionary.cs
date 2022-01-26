using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Script.Client.Util
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair> _list = new List<KeyValuePair>();
        [SerializeField] private Dictionary<TKey, int> _idxByKey = new Dictionary<TKey, int>();
        [SerializeField, HideInInspector] Dictionary<TKey, TValue> _keyValueDic = new Dictionary<TKey, TValue>();
        [SerializeField, HideInInspector] bool _keyCollision;

        [Serializable]
        private class KeyValuePair
        {
            public TKey Key;
            public TValue Value;

            public KeyValuePair(TKey Key, TValue Value)
            {
                this.Key = Key;
                this.Value = Value;
            }
        }

        #region Editor
        // List는 unity에 의해 알아서 직렬화되므로 구현 필요 없음
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            _keyValueDic.Clear();
            _idxByKey.Clear();

            for (int i = 0; i < _list.Count; i++)
            {
                var key = _list[i].Key;
                if (key != null && !ContainsKey(key))
                {
                    _keyValueDic.Add(key, _list[i].Value);
                    _idxByKey.Add(key, i);
                }
            }
        }
        #endregion

        public TValue this[TKey key]
        {
            get => _keyValueDic[key];
            set
            {
                _keyValueDic[key] = value;

                if (_idxByKey.ContainsKey(key))
                {
                    var index = _idxByKey[key];
                    _list[index] = new KeyValuePair(key, value); // Key 겹칠 경우 해당 데이터에 새롭게 정의
                }
                else
                {
                    _list.Add(new KeyValuePair(key, value));
                    _idxByKey.Add(key, _list.Count - 1);
                }
            }
        }

        public bool ContainsKey(TKey key) => _keyValueDic.ContainsKey(key);
        public ICollection<TKey> Keys => _keyValueDic.Keys;
        public ICollection<TValue> Values => _keyValueDic.Values;
        public int Count => _keyValueDic.Count;
        public bool IsReadOnly { get; set; }

        public void Add(TKey key, TValue value)
        {
            _keyValueDic.Add(key, value);
            _list.Add(new KeyValuePair(key, value));
            _idxByKey.Add(key, _list.Count - 1);
        }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public bool Remove(TKey key)
        {
            if (_keyValueDic.Remove(key))
            {
                var index = _idxByKey[key];
                _list.RemoveAt(index);
                UpdateIndexes(index);
                _idxByKey.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        void UpdateIndexes(int removedIndex)
        {
            for (int i = removedIndex; i < _list.Count; i++)
            {
                var key = _list[i].Key;
                _idxByKey[key]--;
            }
        }

        public bool TryGetValue(TKey key, out TValue value) => _keyValueDic.TryGetValue(key, out value);

        public void Clear()
        {
            _keyValueDic.Clear();
            _list.Clear();
            _idxByKey.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            if (_keyValueDic.TryGetValue(pair.Key, out TValue value))
            {
                return EqualityComparer<TValue>.Default.Equals(value, pair.Value);
            }
            else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (array.Length - arrayIndex < _keyValueDic.Count)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            foreach (var pair in _keyValueDic)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            if (_keyValueDic.TryGetValue(pair.Key, out TValue value))
            {
                bool valueMatch = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
                if (valueMatch)
                {
                    return Remove(pair.Key);
                }
            }
            return false;
        }

        // IEnumerable
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValueDic.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _keyValueDic.GetEnumerator();
    }
}