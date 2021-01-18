using System;

namespace Apos.History {
    /// <summary>
    /// Holds a set of "actions" along with their inverse.
    /// Mostly used for undo and redo functionality.
    /// </summary>
    public class HistorySet {
        public HistorySet(Action[] undos, Action[] redos) {
            _undos = undos;
            _redos = redos;
        }
        public void Undo() {
            foreach (Action a in _undos) {
                a();
            }
        }
        public void Redo() {
            foreach (Action a in _redos) {
                a();
            }
        }

        Action[] _redos;
        Action[] _undos;
    }
}
