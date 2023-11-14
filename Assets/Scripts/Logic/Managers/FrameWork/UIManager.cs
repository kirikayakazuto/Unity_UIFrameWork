using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using FrameWork.Structure;
using UnityEngine.AddressableAssets;
using Scene = Renderer.Scene;

namespace FrameWork {
	public class UIManager {
		private readonly RectTransform _UIROOT;
		private readonly RectTransform _NdScreen;
		private readonly RectTransform _NdFixed;
		private readonly RectTransform _NdWindow;
		private readonly RectTransform _NdToast;
		private readonly RectTransform _NdTips;

		public RectTransform NdWindow => this._NdWindow;
		
		private readonly Dictionary<string, UIBase> allForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> showingForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UniTask<UIBase>> loadingForms = new Dictionary<string, UniTask<UIBase>>();
		private readonly Dictionary<string, UIBase> closingForms = new Dictionary<string, UIBase>();
		private readonly LRUCache _lruCache = new LRUCache();
		
		public UIManager() {
			var activeScene = SceneManager.GetActiveScene();
			var objects = activeScene.GetRootGameObjects();
			var canvas = (from t in objects where t.name == "Canvas" select t.GetComponent<RectTransform>()).FirstOrDefault();
			if (canvas == null) {
				Debug.Log("UIManager: 没有找到Canvas, 请确保场景内存在Canvas Game Object");
				return;
			}
			var scene = canvas.Find("Scene");
			if (scene == null) {
				Debug.LogWarning("UIManager: 没有找到Scene, 建议在Canvas节点下创建Scene结点");
				scene = new GameObject(SysDefine.SYS_SCENE_NODE).AddComponent<RectTransform>();
				scene.gameObject.AddComponent<Scene>();
				scene.SetParent(canvas);
				scene.localPosition = Vector3.zero;
			}
			this._UIROOT = new GameObject(SysDefine.SYS_UIROOT_NODE).AddComponent<RectTransform>();
			this._UIROOT.SetParent(scene);
			this._UIROOT.localPosition = Vector3.zero;
			this._NdScreen = new GameObject(SysDefine.SYS_SCREEN_NODE).AddComponent<RectTransform>();
			this._NdScreen.SetParent(this._UIROOT);
			this._NdScreen.localPosition = Vector3.zero;
			this._NdFixed = new GameObject(SysDefine.SYS_FIXED_NODE).AddComponent<RectTransform>();
			this._NdFixed.SetParent(this._UIROOT);
			this._NdFixed.localPosition = Vector3.zero;
			this._NdWindow = new GameObject(SysDefine.SYS_WINDOW_NODE).AddComponent<RectTransform>();
			this._NdWindow.SetParent(this._UIROOT);
			this._NdWindow.localPosition = Vector3.zero;
			this._NdToast = new GameObject(SysDefine.SYS_TOAST_NODE).AddComponent<RectTransform>();
			this._NdToast.SetParent(this._UIROOT);
			this._NdToast.localPosition = Vector3.zero;
			this._NdTips = new GameObject(SysDefine.SYS_TIPS_NODE).AddComponent<RectTransform>();
			this._NdTips.SetParent(this._UIROOT);
			this._NdTips.localPosition = Vector3.zero;
		}

		public Dictionary<string, UIBase> GetShowingForms() {
			return this.showingForms;
		}

		public async UniTask<UIBase> LoadForm(IFormConfig formConfig) {
			if (this.allForms.TryGetValue(formConfig.prefabUrl, out var com)) return com;
			if (this.loadingForms.TryGetValue(formConfig.prefabUrl, out var asyncLoad)) return await asyncLoad;
			
			asyncLoad = this._LoadForm(formConfig);

			this.loadingForms[formConfig.prefabUrl] = asyncLoad;

			com = await asyncLoad;
			this.loadingForms.Remove(formConfig.prefabUrl);
			
			return com;
		}

		private async UniTask<UIBase> _LoadForm(IFormConfig formConfig) {
			var prefabPath = formConfig.prefabUrl;

			var prefab = await Addressables.LoadAssetAsync<GameObject>(prefabPath);

			if (prefab == null) return null;
			
			var gameObject = Object.Instantiate(prefab);
			gameObject.SetActive(false);

			var com = this.AddTransformTree(gameObject.GetComponent<RectTransform>());
			this.allForms[formConfig.prefabUrl] = com;

			return com;
		}

		public async UniTask<UIBase> OpenForm(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			
			var prefabUrl = formConfig.prefabUrl;
			if (prefabUrl.Length <= 0) {
				Debug.LogError("UIManager: open form error, prefabUrl: " + prefabUrl);
				return null;
			}
			
			if (this.CheckFormShowing(prefabUrl)) {
				Debug.LogWarning("UIManager: open form error form is showing, prefabUrl: " + prefabUrl);
				return null;
			}

			var com = await this.LoadForm(formConfig);
			if (!com) {
				Debug.LogError("UIManager: 资源加载失败 prefabUrl: " + prefabUrl);
			}
			
			com.fid = prefabUrl;
			com.formData = formData;

			formData.onOpenBeforShowEffect?.Invoke(com);
			await this.EnterToTree(com.fid, param);

			if (com.closeType == CloseType.LRU) {
				this._lruCache.Remove(com.fid);
			}
			
			BroadcastUtils.FormOpenEvent.Emit(new FormData() {fid = com.fid});

			return com;
		}

		public async UniTask<bool> CloseForm(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			var prefabUrl = formConfig.prefabUrl;
			if (prefabUrl.Length <= 0) {
				Debug.LogError("UIManager: close form error, prefabUrl: " + prefabUrl);
				return false;
			}
			
			if (!this.allForms.TryGetValue(prefabUrl, out var com)) {
				Debug.LogWarning("UIManager: all forms do not have prefabUrl, prefabUrl: " + prefabUrl);
				return false;
			}

			if (this.closingForms.ContainsKey(prefabUrl)) {
				Debug.LogWarning("UIManager: form closing, please wait, prefabUrl: " + prefabUrl);
				return false;
			}
			
			this.closingForms[prefabUrl] = com;

			formData.onCloseBeforHideEffect?.Invoke();
			await this.ExitToTree(prefabUrl, param);
			
			formData.onClose?.Invoke();

			switch (com.closeType) {
				case CloseType.Destory:
					this.DestroyForm(com);
					break;
				case CloseType.Hide:
					break;
				case CloseType.LRU:
					this.PutLRUCache(com);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			BroadcastUtils.FormCloseEvent.Emit(new FormData() {fid = com.fid});
			
			return this.closingForms.Remove(prefabUrl);
		}

		public UIBase AddTransformTree(RectTransform transform) {
			var com = transform.GetComponent<UIBase>();
			
			transform.SetParent(com.formType switch {
				FormType.Screen => this._NdScreen,
				FormType.Fixed => this._NdFixed,
				FormType.Window => this._NdWindow,
				FormType.Toast => this._NdToast,
				FormType.Tips => this._NdTips,
				_ => transform.parent
			});
			
			transform.localPosition = Vector3.zero;
			
			return com;
		}

		private async UniTask<bool> EnterToTree(string fid, [CanBeNull] Object param) {
			if (!this.allForms.TryGetValue(fid, out var com)) return false;
			await com._PreInit(param);
			com.OnShow(param);
			com.gameObject.SetActive(true);
			this.showingForms[fid] = com;
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		public async UniTask<bool> EnterToToast(UIBase com, Object param) {
			await com._PreInit(param);
			com.OnShow(param);
			com.gameObject.SetActive(true);
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		private async UniTask<bool> ExitToTree(string fid, Object param) {
			if (!this.showingForms.TryGetValue(fid, out var com)) return false;
			
			com.OnHide(param);
			await com.OnHideEffect();
			com.gameObject.SetActive(false);
			com.OnAfterHide(param);

			return this.showingForms.Remove(fid);
		}

		public async UniTask<bool> ExitToToast(UIBase com, Object param) {
			com.OnHide(param);
			await com.OnHideEffect();
			com.gameObject.SetActive(false); 
			com.OnAfterHide(param);

			return true;
		}

		/**
		 * 销毁窗体
		 */
		private void DestroyForm(UIBase com) {
			Object.Destroy(com.gameObject);
			this.allForms.Remove(com.fid);
		}

		public bool CheckFormShowing(string prefabPath) {
			return this.showingForms.ContainsKey(prefabPath);
		}

		public bool CheckFormLoading(string prefabPath) {
			return this.loadingForms.ContainsKey(prefabPath);
		}

		public UIBase GetForm(string prefabPath) {
			if (this.allForms.ContainsKey(prefabPath)) {
				return this.allForms[prefabPath];
			}
			return null;
		}
		
		public void LogDictionary(Dictionary<string , UIBase> dic) {
			foreach (var keyValuePair in dic) {
				Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value.fid);
			}
		}

		private void PutLRUCache(UIBase com) {
			this._lruCache.Put(com.fid);
			if(!this._lruCache.NeedToDeleted()) return;
			var deleteFid = this._lruCache.DeleteLastNode();
			if (deleteFid.Length <= 0) return;
			com = this.GetForm(deleteFid);
			if(com == null || com.gameObject == null) return;
			this.DestroyForm(com);
		}
	}
}