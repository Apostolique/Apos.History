using System;
using System.Collections.Generic;

namespace Apos.History {
    /// <summary>
    /// Holds a set of "actions" along with their inverse.
    /// Mostly used for undo and redo functionality.
    /// </summary>
    public class HistorySet {
        public HistorySet(List<Action> undos, List<Action> redos) {
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

        List<Action> _redos;
        List<Action> _undos;
    }
}