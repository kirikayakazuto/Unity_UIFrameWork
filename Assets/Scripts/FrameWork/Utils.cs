using System.Collections.Generic;
using UnityEngine;

namespace FrameWork {
	public static class Utils {
		public static Transform GetTransformByName(Transform root, string name) {
			var transform = root;
			return root.Find(name);
		}
		
		public static bool RemoveAtStack<T>(Stack<T> stack, T t) {
			if(!stack.Contains(t)) return false;
			var tmpStack = new Stack<T>();
			while (stack.Count > 0) {
				var tmp = stack.Pop();
				if (tmp.Equals(t)) break;
				tmpStack.Push(tmp);
			}

			while (tmpStack.Count > 0) {
				stack.Push(tmpStack.Pop());
			}

			return true;
		}
	}
}