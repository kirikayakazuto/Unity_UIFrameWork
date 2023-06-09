using System.Collections.Generic;

namespace FrameWork.Structure {
	/**
	 * 带优先的栈
	 */
	public class PriorityStatck<T> {
		public delegate bool Compare(T a, T b);

		private Compare _compare;

		private readonly Stack<PriorityElement<T>> _stack = new Stack<PriorityElement<T>>();

		public PriorityStatck(Compare compare) {
			this._compare = compare;
		}

		public void Add(T e, int priority) {
			var element = new PriorityElement<T>() {data = e, priority = priority};
			this._stack.Push(element);
		}

		private void Adjust() {
			
		}
		
		
		
		
		
		
	}
}