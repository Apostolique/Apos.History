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
        /// Gets the number of elements in the undo stack.
        /// </summary>
        public int UndoCount => _undo.Count;
        /// <summary>
        /// Gets the number of elements in the redo stack.
        /// </summary>
        public int RedoCount => _redo.Count;
        /// <summary>
        /// Gets the number of elements in total. (UndoCount + RedoCount)
        /// </summary>
        public int Count => UndoCount + RedoCount;

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
                    _undo.AddLast(hs);
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
                HistorySet hs = _undo.Last.Value;
                _undo.RemoveLast();
                hs.Undo();
                _redo.AddLast(hs);
            }
        }
        /// <summary>
        /// Restores the next state.
        /// </summary>
        public void Redo() {
            if (_redo.Count > 0) {
                HistorySet hs = _redo.Last.Value;
                _redo.RemoveLast();
                hs.Redo();
                _undo.AddLast(hs);
            }
        }

        /// <summary>
        /// Removes a range of elements from the undo stack keeping only the newest elements.
        /// </summary>
        /// <param name="count">The number of elements to keep.</param>
        public void Keep(int count) {
            int amount = Math.Max(count, 0);
            while (_undo.Count > amount) {
                _undo.RemoveFirst();
            }
        }
        /// <summary>
        /// Removes a range of elements from the undo stack keeping only the newest elements.
        /// </summary>
        /// <param name="count">The number of elements to remove.</param>
        public void Remove(int count) {
            int amount = Math.Min(count, _undo.Count);
            for (int i = 0; i < amount; i++) {
                _undo.RemoveFirst();
            }
        }

        /// <summary>
        /// Undo stack.
        /// </summary>
        protected LinkedList<HistorySet> _undo = new LinkedList<HistorySet>();
        /// <summary>
        /// Redo stack.
        /// </summary>
        protected LinkedList<HistorySet> _redo = new LinkedList<HistorySet>();
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
