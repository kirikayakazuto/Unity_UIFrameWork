using System.Collections.Generic;

namespace FrameWork.Structure {
    public class PriorityQueue<T> {
        
        private readonly List<PriorityElement<T>> _element = new List<PriorityElement<T>>();
        private readonly Queue<PriorityElement<T>> _elements = new Queue<PriorityElement<T>>();

        public void Enqueue(T data, int priority = 0) {
            var e = new PriorityElement<T>(){data = data, priority = priority};
            this._element.Add(e);
        }

        public void Dequeue() {
            
        }

        private void UpAdjust() {
            
        }

        private void DownAdjust() {
            
        }

        private void Clear() {
            this._element.Clear();
        }

        public bool HasElement(T t) {
            foreach (var priorityElement in this._element) {
                if (priorityElement.data.Equals(t)) {
                    return true;
                }
            }
            return false;
        }
    }
}