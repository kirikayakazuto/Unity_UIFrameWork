using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FrameWork {
	public class NodeManager {
		private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

		public NodeManager() {
			
		}
		
		public async UniTask<GameObject> Instantiate(string prefabUrl) {
			if(this._prefabs.TryGetValue(prefabUrl, out var gameObject)) return Object.Instantiate(gameObject);
			gameObject = await Addressables.LoadAssetAsync<GameObject>(prefabUrl);
			this._prefabs.Add(prefabUrl, gameObject);
			return Object.Instantiate(gameObject);
		}

		public void Release(string prefabUrl) {
			if(!this._prefabs.ContainsKey(prefabUrl)) return;
			var gameObject = this._prefabs[prefabUrl];
			this._prefabs.Remove(prefabUrl);
			Addressables.Release(gameObject);
		}
	}
}