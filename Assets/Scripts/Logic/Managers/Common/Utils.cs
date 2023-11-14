using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Logic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

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
		
		public static Image GenSingleColorImage(string name) {
			var goImage = new GameObject(name);
			var button = goImage.AddComponent<Button>();
			button.onClick.AddListener(() => { });
			goImage.AddComponent<RectTransform>();
			return goImage.AddComponent<Image>();
		}
	}
}