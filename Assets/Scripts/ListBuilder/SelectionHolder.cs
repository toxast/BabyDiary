using System;

namespace ui6 {

    public class SelectionHolder<T> where T : class, ITogglable {
        public T current { get; private set; }

        //returns true if value changed
        public bool Set(T next) {
            if (current == next) {
                return false;
            }
            if (current != null) {
                current.SetState(false);
            }
            current = next;
            if (current != null) {
                current.SetState(true);
            }
            return true;
        }
    }
}