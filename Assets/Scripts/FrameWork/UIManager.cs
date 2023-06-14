using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using FrameWork.Structure;

namespace FrameWork {
	public class UIManager {
		private RectTransform _UIROOT;
		private RectTransform _NdScreen;
		private RectTransform _NdFixed;
		private RectTransform _NdWindow;
		private RectTransform _NdToast;
		private RectTransform _NdTips;

		public RectTransform NdWindow => this._NdWindow;

		private delegate UniTask<UIBase> OnLoadUIBase();
		private readonly Dictionary<string, UIBase> allForms = new Dictionary<string, UIBase>();
		public readonly Dictionary<string, UIBase> showingForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, ResourceRequest> loadingForms = new Dictionary<string, ResourceRequest>();
		private readonly Dictionary<string, UIBase> closingForms = new Dictionary<string, UIBase>();
		private readonly LRUCache _lruCache = new LRUCache();

		private static UIManager instance;
		public static UIManager GetInstance() {
			if (UIManager.instance != null) return UIManager.instance;
			
			var self = UIManager.instance = new UIManager();
			var activeScene = SceneManager.GetActiveScene();
			var objects = activeScene.GetRootGameObjects();
			var canvas = (from t in objects where t.name == "Canvas" select t.GetComponent<RectTransform>()).FirstOrDefault();
			if (canvas == null) {
				Debug.Log("UIManager: 没有找到Canvas");
				return self;
			}
			var scene = canvas.Find("Scene");
			if (scene == null) {
				Debug.Log("UIManager: 没有找到Scene");
				return self;
			}
			self._UIROOT = new GameObject(SysDefine.SYS_UIROOT_NODE).AddComponent<RectTransform>();
			self._UIROOT.SetParent(scene);
			self._UIROOT.localPosition = Vector3.zero;
			self._NdScreen = new GameObject(SysDefine.SYS_SCREEN_NODE).AddComponent<RectTransform>();
			self._NdScreen.SetParent(self._UIROOT);
			self._NdScreen.localPosition = Vector3.zero;
			self._NdFixed = new GameObject(SysDefine.SYS_FIXED_NODE).AddComponent<RectTransform>();
			self._NdFixed.SetParent(self._UIROOT);
			self._NdFixed.localPosition = Vector3.zero;
			self._NdWindow = new GameObject(SysDefine.SYS_WINDOW_NODE).AddComponent<RectTransform>();
			self._NdWindow.SetParent(self._UIROOT);
			self._NdWindow.localPosition = Vector3.zero;
			self._NdToast = new GameObject(SysDefine.SYS_TOAST_NODE).AddComponent<RectTransform>();
			self._NdToast.SetParent(self._UIROOT);
			self._NdToast.localPosition = Vector3.zero;
			self._NdTips = new GameObject(SysDefine.SYS_TIPS_NODE).AddComponent<RectTransform>();
			self._NdTips.SetParent(self._UIROOT);
			self._NdTips.localPosition = Vector3.zero;
			
			return UIManager.instance;
		}

		public async UniTask<UIBase> LoadForm(IFormConfig formConfig) {
			if (this.allForms.TryGetValue(formConfig.prefabUrl, out var com)) return com;
			var gameObject = await this._LoadForm(formConfig.prefabUrl);
			if (gameObject == null) return null;
			com = this.AddTransformTree(gameObject.GetComponent<RectTransform>());
			this.allForms[formConfig.prefabUrl] = com;
			return com;
		}

		private async UniTask<GameObject> _LoadForm(string prefabPath) {
			
			if (this.loadingForms.TryGetValue(prefabPath, out var requestLoad)) {
				return await requestLoad as GameObject;
			}
			
			var asyncLoad = Resources.LoadAsync<GameObject>(prefabPath);
			
			this.loadingForms[prefabPath] = asyncLoad;
			var gameObject = await asyncLoad as GameObject;
			if (!this.loadingForms.Remove(prefabPath)) return null;

			if (gameObject == null) return null;
			gameObject.SetActive(false);
			return Object.Instantiate(gameObject);
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

		public void LogAllDictionary() {
			Debug.Log("=====> AllForms");
			this.LogDictionary(this.allForms);
			Debug.Log("=====> ShowingForms");
			this.LogDictionary(this.showingForms);
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