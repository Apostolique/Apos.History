using System;
using System.Collections.Generic;
using System.Linq;
using Optional;

namespace Apos.History {
    /// <summary>
    /// A list that can preserve it's history.
    /// </summary>
    public class HistoryList<T> : History {
        public HistoryList(List<T> list, Option<HistoryHandler> historyHandler) : base(historyHandler) {
            _hList = list;
        }

        public void Add(T item) {
            int index = _hList.Count;
            _futureSetup.Add(() => {
                _hList.Add(item);
            });
            _pastSetup.Add(() => {
                _hList.RemoveAt(index);
            });

            TryCommit();
        }
        public void Insert(int index, T item) {
            _futureSetup.Add(() => {
                _hList.Insert(index, item);
            });
            _pastSetup.Add(() => {
                _hList.RemoveAt(index);
            });

            TryCommit();
        }
        public void RemoveAt(int index) {
            T removedItem = _hList[index];
            _futureSetup.Add(() => {
                _hList.RemoveAt(index);
            });
            _pastSetup.Add(() => {
                _hList.Insert(index, removedItem);
            });

            TryCommit();
        }
        public void ReplaceAt(int index, T item) {
            T removedItem = _hList[index];
            _futureSetup.Add(() => {
                _hList[index] = item;
            });
            _pastSetup.Add(() => {
                _hList[index] = removedItem;
            });

            TryCommit();
        }
        public void Clear() {
            List<T> savedList = _hList.ToList();
            _futureSetup.Add(() => {
                _hList.Clear();
            });
            _pastSetup.Add(() => {
                _hList = savedList.ToList();
            });

            TryCommit();
        }
        public int Count() {
            return _hList.Count;
        }
        public T this [int index] {
            get {
                return _hList[index];
            }
            set {
                ReplaceAt(index, value);

                TryCommit();
            }
        }
        public IEnumerator<T> GetEnumerator() {
            return _hList.GetEnumerator();
        }

        List<T> _hList;
    }
}