using System.Collections.Generic;

namespace FrameWork.Structure {
	/**
	 * 带优先的栈
	 */
	public class PriorityStatck<T> where T : new() {
		
		private readonly List<PriorityElement<T>> _elements = new List<PriorityElement<T>>();
		
		public void Push(T e, int priority = 0) {
			var element = new PriorityElement<T>() {data = e, priority = priority};
			this._elements.Add(element);
			this.Adjust();
		}

		public T Pop() {
			if (this._elements.Count <= 0) return new T();
			var data = this._elements[^1].data;
			this._elements.RemoveAt(this._elements.Count - 1);
			return data;
		}

		private void Adjust() {
			for (var i = this._elements.Count - 1; i > 0; i--) {
				if (this._elements[i].priority < this._elements[i - 1].priority) {
					this._swap(i, i-1);
				}
			}
		}

		public bool HasElement(T e) {
			return this.GetElementIdx(e) != -1;
		}

		private int GetElementIdx(T e) {
			var idx = -1;
			for (var i = 0; i < this._elements.Count; i++) {
				var d = this._elements[i].data;
				if (!d.Equals(e)) continue;
				idx = i;
				break;;
			}

			return idx;
		}

		public bool Remove(T e) {
			var idx = this.GetElementIdx(e);
			if (idx < 0) return false;
			this._elements.RemoveAt(idx);
			return true;
		}

		public T GetTopElement() {
			if (this._elements.Count <= 0) return new T();
			return this._elements[^1].data;
		}

		public int GetTopElementPriority() {
			if (this._elements.Count <= 0) return 0;
			return this._elements[^1].priority;
		}
		
		private void _swap(int a, int b) {
			(this._elements[a], this._elements[b]) = (this._elements[b], this._elements[a]);
		}

		public void Clear() {
			this._elements.Clear();
		}

		public T[] GetElements() {
			var arr = new T[this._elements.Count];
			for (var i = 0; i < this._elements.Count; i++) {
				arr[i] = this._elements[i].data;
			}

			return arr;
		}

		public int Count => this._elements.Count;



	}
}