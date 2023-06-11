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
			self._UIROOT.parent = scene;
			self._NdScreen = new GameObject(SysDefine.SYS_SCREEN_NODE).AddComponent<RectTransform>();
			self._NdScreen.parent = self._UIROOT;
			self._NdFixed = new GameObject(SysDefine.SYS_FIXED_NODE).AddComponent<RectTransform>();
			self._NdFixed.parent = self._UIROOT;
			self._NdWindow = new GameObject(SysDefine.SYS_WINDOW_NODE).AddComponent<RectTransform>();
			self._NdWindow.parent = self._UIROOT;
			self._NdToast = new GameObject(SysDefine.SYS_TOAST_NODE).AddComponent<RectTransform>();
			self._NdToast.parent = self._UIROOT;
			self._NdTips = new GameObject(SysDefine.SYS_TIPS_NODE).AddComponent<RectTransform>();
			self._NdTips.parent = self._UIROOT;
			
			return UIManager.instance;
		}

		public async UniTask<UIBase> LoadForm(IFormConfig formConfig) {
			var gameObject = await this._LoadForm(formConfig.prefabUrl);
			return gameObject == null ? null : this.AddTransformTree(gameObject.GetComponent<RectTransform>());
		}

		private async UniTask<GameObject> _LoadForm(string prefabPath) {
			var requestLoad = this.loadingForms[prefabPath];
			if (requestLoad != null) {
				return await requestLoad as GameObject;
			}
			
			var asyncLoad = Resources.LoadAsync<GameObject>(prefabPath);
			
			this.loadingForms[prefabPath] = asyncLoad;
			var gameObject = await asyncLoad as GameObject;
			this.loadingForms.Remove(prefabPath);

			return Object.Instantiate(gameObject);
		}

		public async UniTask<UIBase> OpenForm(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
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
			com.formData = formData ?? new IFormData();

			await this.EnterToTree(com.fid, param);
			
			return com;
		}

		public async UniTask<bool> CloseForm(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			var prefabUrl = formConfig.prefabUrl;
			if (prefabUrl.Length <= 0) {
				Debug.LogError("UIManager: open form error, prefabUrl: " + prefabUrl);
				return false;
			}
			var com = this.allForms[prefabUrl];
			if (!com) return false;

			if (this.closingForms.ContainsKey(prefabUrl)) {
				Debug.LogWarning("UIManager: form closing, please wait, prefabUrl: " + prefabUrl);
				return false;
			}
			this.closingForms[prefabUrl] = com;

			await this.ExitToTree(prefabUrl, param);
			
			this.DestroyForm(com);
			
			this.closingForms.Remove(prefabUrl);
			
			return true;
		}

		public UIBase AddTransformTree(RectTransform transform) {
			var com = transform.GetComponent<UIBase>();
			transform.gameObject.SetActive(false);

			transform.parent = com.formType switch {
				FormType.Screen => this._NdScreen,
				FormType.Fixed => this._NdFixed,
				FormType.Window => this._NdWindow,
				FormType.Toast => this._NdToast,
				FormType.Tips => this._NdTips,
				_ => transform.parent
			};

			this.allForms[com.fid] = com;

			return com;
		}

		private async UniTask<bool> EnterToTree(string fid, [CanBeNull] Object param) {
			var com = this.allForms[fid];
			if(!com) return false;
			await com._PreInit(param);
			
			com.OnShow(param);
			this.showingForms[fid] = com;
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		public async UniTask<bool> EnterToToast(UIBase com, Object param) {
			await com._PreInit(param);
			
			com.OnShow(param);
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		private async UniTask<bool> ExitToTree(string fid, Object param) {
			var com = this.showingForms[fid];
			if (!com) return false;

			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			this.showingForms.Remove(fid);
			return true;
		}

		public async UniTask<bool> ExitToToast(UIBase com, Object param) {
			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			return true;
		}

		/**
		 * 销毁窗体
		 */
		private void DestroyForm(UIBase com) {
			this.allForms.Remove(com.fid);
		}

		public bool CheckFormShowing(string prefabPath) {
			return this.showingForms.ContainsKey(prefabPath);
		}

		public bool CheckFormLoading(string prefabPath) {
			return this.loadingForms.ContainsKey(prefabPath);
		}

		public UIBase GetForm(string prefabPath) {
			return this.allForms[prefabPath];
		}
	}
}