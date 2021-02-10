using System;

namespace Apos.History {
    /// <summary>
    /// This class makes it easy to provide undo and redo over multiple data structures.
    /// </summary>
    public class HistoryHandler : History {
        /// <summary>
        /// History that is managed locally.
        /// </summary>
        public HistoryHandler() { }
        /// <summary>
        /// The history is local when null. Otherwise it's managed by a HistoryHandler.
        /// </summary>
        public HistoryHandler(HistoryHandler? historyHandler) : base(historyHandler) { }

        /// <summary>
        /// Adds a HistorySet to the pending list and tries to commit it.
        /// </summary>
        public void Add(HistorySet hs) {
            _pendingUndo.Add(hs.Undo);
            _pendingRedo.Add(hs.Redo);

            TryCommit();
        }
        /// <summary>
        /// Adds undo and redo actions to the pending list and tries to commit them.
        /// </summary>
        public void Add(Action undo, Action redo) {
            _pendingUndo.Add(undo);
            _pendingRedo.Add(redo);

            TryCommit();
        }
    }
}
