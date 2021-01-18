using System;
using System.Collections.Generic;

namespace Apos.History {
    /// <summary>
    /// Base class for History classes. Provides undo, redo.
    /// </summary>
    public class History {
        /// <summary>
        /// The history is local when null. Otherwise it's managed by a HistoryHandler.
        /// </summary>
        public History(HistoryHandler? historyHandler) {
            _historyHandler = historyHandler;
        }

        /// <summary>
        /// When false, the history is delayed until Commit() is called manually.
        /// </summary>
        public bool AutoCommit {
            get;
            set;
        } = true;

        /// <summary>
        /// True when it's possible to undo.
        /// </summary>
        public bool CanUndo => _undo.Count > 0;
        /// <summary>
        /// True when it's possible to redo.
        /// </summary>
        public bool CanRedo => _redo.Count > 0;

        /// <summary>
        /// Commits when AutoCommit is set to true.
        /// </summary>
        public void TryCommit() {
            if (AutoCommit) {
                Commit();
            }
        }
        /// <summary>
        /// Commits pending undo and redo to the undo and redo stacks.
        /// </summary>
        public void Commit() {
            if (_pendingUndo.Count > 0 && _pendingRedo.Count > 0) {
                _pendingUndo.Reverse();
                HistorySet hs = new HistorySet(_pendingUndo.ToArray(), _pendingRedo.ToArray());
                _redo.Clear();

                if (_historyHandler != null) {
                    _historyHandler.Add(hs);
                } else {
                    hs.Redo();
                    _undo.Push(hs);
                }

                _pendingRedo.Clear();
                _pendingUndo.Clear();
            }
        }
        /// <summary>
        /// Restores the previous state.
        /// </summary>
        public void Undo() {
            if (_undo.Count > 0) {
                HistorySet hs = _undo.Pop();
                hs.Undo();
                _redo.Push(hs);
            }
        }
        /// <summary>
        /// Restores the next state.
        /// </summary>
        public void Redo() {
            if (_redo.Count > 0) {
                HistorySet hs = _redo.Pop();
                hs.Redo();
                _undo.Push(hs);
            }
        }

        /// <summary>
        /// Undo stack.
        /// </summary>
        protected Stack<HistorySet> _undo = new Stack<HistorySet>();
        /// <summary>
        /// Redo stack.
        /// </summary>
        protected Stack<HistorySet> _redo = new Stack<HistorySet>();
        /// <summary>
        /// Undo states that haven't been commited yet.
        /// </summary>
        protected List<Action> _pendingUndo = new List<Action>();
        /// <summary>
        /// Redo states that haven't been commited yet.
        /// </summary>
        protected List<Action> _pendingRedo = new List<Action>();
        /// <summary>
        /// When not null, the history is managed elsewhere.
        /// </summary>
        protected HistoryHandler? _historyHandler;
    }
}
