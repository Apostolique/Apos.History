using System;

namespace Apos.History {
    /// <summary>
    /// Holds a set of actions along with their inverse.
    /// Mostly used for undo and redo functionality.
    /// </summary>
    public class HistorySet {
        /// <summary>
        /// Groups undo and redo actions.
        /// </summary>
        public HistorySet(Action[] undos, Action[] redos) {
            _undos = undos;
            _redos = redos;
        }
        /// <summary>
        /// Applies the undo actions.
        /// </summary>
        public void Undo() {
            foreach (Action a in _undos) {
                a();
            }
        }
        /// <summary>
        /// Applies the redo actions.
        /// </summary>
        public void Redo() {
            foreach (Action a in _redos) {
                a();
            }
        }

        /// <summary>
        /// Group of undo actions that should be executed together.
        /// </summary>
        protected Action[] _undos;
        /// <summary>
        /// Group of redo actions that should be executed together.
        /// </summary>
        protected Action[] _redos;
    }
}
