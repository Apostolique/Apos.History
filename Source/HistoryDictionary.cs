using System;
using System.Collections.Generic;
using Optional;

namespace Apos.History {
    /// <summary>
    /// A dictionary that can preserve it's history.
    /// </summary>
    public class HistoryDictionary<T, K> : History {
        public HistoryDictionary(Dictionary<T, K> dict, Option<HistoryHandler> historyHandler) : base(historyHandler) {
            _hDict = dict;
        }

        public void Add(T key, K item) {
            _futureSetup.Add(() => {
                _hDict.Add(key, item);
            });
            _pastSetup.Add(() => {
                _hDict.Remove(key);
            });

            TryCommit();
        }
        public void Remove(T key) {
            K removedItem = _hDict[key];
            _futureSetup.Add(() => {
                _hDict.Remove(key);
            });
            _pastSetup.Add(() => {
                _hDict.Add(key, removedItem);
            });

            TryCommit();
        }
        public void Replace(T key, K item) {
            K removedItem = _hDict[key];
            _futureSetup.Add(() => {
                _hDict[key] = item;
            });
            _pastSetup.Add(() => {
                _hDict[key] = removedItem;
            });

            TryCommit();
        }
        public void Clear() {
            Dictionary<T, K> savedDict = new Dictionary<T, K>(_hDict);
            _futureSetup.Add(() => {
                _hDict.Clear();
            });
            _pastSetup.Add(() => {
                _hDict = new Dictionary<T, K>(savedDict);
            });

            TryCommit();
        }
        public bool ContainsKey(T key) {
            return _hDict.ContainsKey(key);
        }
        public bool TryGetValue(T key, out K value) {
            return _hDict.TryGetValue(key, out value);
        }
        public int Count() {
            return _hDict.Count;
        }
        public K this [T key] {
            get {
                return _hDict[key];
            }
            set {
                Replace(key, value);

                TryCommit();
            }
        }
        public IEnumerator<KeyValuePair<T, K>> GetEnumerator() {
            return _hDict.GetEnumerator();
        }

        Dictionary<T, K> _hDict;
    }
}