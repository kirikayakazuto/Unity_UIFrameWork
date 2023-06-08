using UnityEngine;

namespace FrameWork {
	public class Utils {
		public static Transform GetTransformByName(Transform root, string name) {
			var transform = root;
			return root.Find(name);
		}
	}
}