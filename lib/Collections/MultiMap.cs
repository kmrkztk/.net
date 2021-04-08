using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collections
{
    public class MultiMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, IList<TValue>> _source;
        public MultiMap() => _source = new Dictionary<TKey, IList<TValue>>();
        public MultiMap(int capacity) => _source = new Dictionary<TKey, IList<TValue>>(capacity);
        public MultiMap(IEqualityComparer<TKey> comparer) => _source = new Dictionary<TKey, IList<TValue>>(comparer);
        public MultiMap(int capacity, IEqualityComparer<TKey> comparer) => _source = new Dictionary<TKey, IList<TValue>>(capacity, comparer);
        public MultiMap(IDictionary<TKey, TValue> dictionary) : this() => Add(dictionary);
        public MultiMap(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this(comparer) => Add(dictionary);
        public MultiMap(IEnumerable<KeyValuePair<TKey, TValue>> dictionary) : this() => Add(dictionary);
        public MultiMap(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> comparer) : this(comparer) => Add(dictionary);
        public IList<TValue> this[TKey key] => _source[key];
        public ICollection<TKey> Keys => _source.Keys;
        public ICollection<TValue> Values => new Dictionary<TKey, TValue>.ValueCollection(AsEnumerable().ToDictionary(_ => _.Key, _ => _.Value));
        public int Count => Values.Count;
        public bool IsReadOnly => false;
        public IEqualityComparer<TKey> Comparer => _source.Comparer;
        public void Add(TKey key, TValue value) 
        {
            if (_source.ContainsKey(key)) _source[key].Add(value); else _source.Add(key, new List<TValue>() { value });
        }
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public void Add(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            foreach (var v in dictionary) Add(v);
        }
        public void Add(IDictionary<TKey, TValue> dictionary) => Add((IEnumerable<KeyValuePair<TKey, TValue>>)dictionary);
        public bool Remove(TKey key) => _source.Remove(key);
        public void Clear() => _source.Clear();
        public void Clear(TKey key) => _source[key].Clear();
        public bool ContainsKey(TKey key) => _source.ContainsKey(key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerable<KeyValuePair<TKey, TValue>> AsEnumerable() => _source.SelectMany(_ => _.Value.Select(v => new KeyValuePair<TKey, TValue>(_.Key, v)));
        #region not implemented
        TValue IDictionary<TKey, TValue>.this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();
        #endregion
    }
}
