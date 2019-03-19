using System;
using System.Collections.Generic;
using Optional;

namespace Apos.History
{
    /// <summary>
    /// Base class for History classes. Provides undo, redo.
    /// </summary>
    public class History {
        public History(Option<HistoryHandler> historyHandler) {
            _historyHandler = historyHandler;
        }

        public bool AutoCommit {
            get;
            set;
        } = true;

        public bool HasPast => _past.Count > 0;
        public bool HasFuture => _future.Count > 0;

        public void TryCommit() {
            if (AutoCommit) {
                Commit();
            }
        }
        public void Commit() {
            if (_pastSetup.Count > 0 && _futureSetup.Count > 0) {
                _pastSetup.Reverse();
                HistorySet hs = new HistorySet(_pastSetup, _futureSetup);
                _future.Clear();

                _historyHandler.Match(hh => {
                    hh.Add(hs);
                }, () => {
                    hs.Redo();
                    _past.Push(hs);
                });

                _futureSetup = new List<Action>();
                _pastSetup = new List<Action>();
            }
        }
        public void Undo() {
            if (_past.Count > 0) {
                HistorySet hs = _past.Pop();
                hs.Undo();
                _future.Push(hs);
            }
        }
        public void Redo() {
            if (_future.Count > 0) {
                HistorySet hs = _future.Pop();
                hs.Redo();
                _past.Push(hs);
            }
        }

        protected Stack<HistorySet> _past = new Stack<HistorySet>();
        protected Stack<HistorySet> _future = new Stack<HistorySet>();
        protected List<Action> _pastSetup = new List<Action>();
        protected List<Action> _futureSetup = new List<Action>();
        protected Option<HistoryHandler> _historyHandler;
    }
}