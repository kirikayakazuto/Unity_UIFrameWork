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

		private delegate UniTask<UIBase> OnLoadUIBase();
		private readonly Dictionary<string, UIBase> _allForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> _showingForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, ResourceRequest> _loadingForms = new Dictionary<string, ResourceRequest>();
		private readonly Dictionary<string, UIBase> _closingForms = new Dictionary<string, UIBase>();

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

		public async UniTask<UIBase> LoadForm(string prefabPath) {
			var gameObject = await this._LoadForm(prefabPath);
			return gameObject == null ? null : this.AddTransformTree(gameObject.GetComponent<RectTransform>());
		}

		private async UniTask<GameObject> _LoadForm(string prefabPath) {
			var requestLoad = this._loadingForms[prefabPath];
			if (requestLoad != null) {
				return await requestLoad as GameObject;
			}
			
			var asyncLoad = Resources.LoadAsync<GameObject>(prefabPath);
			
			this._loadingForms[prefabPath] = asyncLoad;
			var gameObject = await asyncLoad as GameObject;
			this._loadingForms.Remove(prefabPath);

			return gameObject;
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

			var com = await this.LoadForm(prefabUrl);
			if (!com) {
				Debug.LogError("UIManager: 资源加载失败 prefabUrl: " + prefabUrl);
			}
			
			com.fid = prefabUrl;
			com.formData = formData ?? new IFormData();

			switch (com.formType) {
				case FormType.Screen:
					await this.EnterToScreen(com.fid, param);
					break;
				case FormType.Fixed:
					await this.EnterToFixed(com.fid, param);
					break;
				case FormType.Window:
					await this.EnterToWindos(com.fid, param);
					break;
				case FormType.Tips:
					await this.EnterToTips(com.fid, param);
					break;
				case FormType.Toast:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			return com;
		}

		public async UniTask<bool> CloseForm(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			var prefabUrl = formConfig.prefabUrl;
			if (prefabUrl.Length <= 0) {
				Debug.LogError("UIManager: open form error, prefabUrl: " + prefabUrl);
				return false;
			}
			var com = this._allForms[prefabUrl];
			if (!com) return false;

			if (this._closingForms.ContainsKey(prefabUrl)) {
				Debug.LogWarning("UIManager: form closing, please wait, prefabUrl: " + prefabUrl);
				return false;
			}
			this._closingForms[prefabUrl] = com;

			switch (com.formType) {
				case FormType.Screen:
					await this.ExitToScreen(prefabUrl, param);
					break;
				case FormType.Fixed:
					await this.ExitToFixed(prefabUrl, param);
					break;
				case FormType.Window:
					await this.ExitToWindow(prefabUrl, param);
					break;
				case FormType.Tips:
					await this.ExitToTips(prefabUrl, param);
					break;
				case FormType.Toast:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			this.DestroyForm(com);
			
			this._closingForms.Remove(prefabUrl);
			
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

			this._allForms[com.fid] = com;

			return com;
		}

		private async UniTask<bool> EnterToScreen(string fid, [CanBeNull] Object param) {
			if (!this._allForms.ContainsKey(fid)) return false;
			
			var uniTasks = new UniTask<bool>[this._showingForms.Count];
			var idx = 0;
			foreach (var keyValuePair in this._showingForms) {
				if(keyValuePair.Value.formType == FormType.Tips) continue;
				uniTasks[idx] = keyValuePair.Value.CloseSelf();
				idx++;
			}

			await UniTask.WhenAll(uniTasks);

			var com = this._allForms[fid];
			this._showingForms[fid] = com;
			
		 	await com._PreInit(param);
			com.OnShow(param);
			await com.OnShowEffect();
			com.OnAfterShow(param);
			return true;
		}

		private async UniTask<bool> EnterToFixed(string fid, Object param) {
			var com = this._allForms[fid];
			if(!com) return false;
			await com._PreInit(param);
			
			com.OnShow(param);
			this._showingForms[fid] = com;
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		private async UniTask<bool> EnterToWindos(string fid, Object param) {
			var com = this._allForms[fid];
			if (!com) return false;
			await com._PreInit(param);
			
			com.OnShow(param);
			this._showingForms[fid] = com;
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		private async UniTask<bool> EnterToTips(string fid, Object param) {
			var com = this._allForms[fid];
			if (!com) return false;
			await com._PreInit(param);
			
			com.OnShow(param);
			this._showingForms[fid] = com;
			await com.OnShowEffect();
			com.OnAfterShow(param);

			return true;
		}

		private async UniTask<bool> EnterToToast(UIBase com, Object param) {
			await com._PreInit(param);
			
			com.OnShow(param);
			await com.OnShowEffect();
			com.OnAfterShow(param);
			
			return true;
		}

		private async UniTask<bool> ExitToScreen(string fid, Object param) {
			var com = this._showingForms[fid];
			if (!com) return false;
			
			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			this._showingForms.Remove(fid);
			return true;
		}

		private async UniTask<bool> ExitToFixed(string fid, Object param) {
			var com = this._showingForms[fid];
			if (!com) return false;
			
			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			this._showingForms.Remove(fid);
			return true;
		}

		private async UniTask<bool> ExitToWindow(string fid, Object param) {
			var com = this._showingForms[fid];
			if (!com) return false;
			
			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			this._showingForms.Remove(fid);
			return true;
		}

		private async UniTask<bool> ExitToTips(string fid, Object param) {
			var com = this._showingForms[fid];
			if (!com) return false;
			
			com.OnHide(param);
			await com.OnHideEffect();
			com.OnAfterHide(param);

			this._showingForms.Remove(fid);
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
			this._allForms.Remove(com.fid);
		}

		public bool CheckFormShowing(string prefabPath) {
			return this._showingForms.ContainsKey(prefabPath);
		}

		public bool CheckFormLoading(string prefabPath) {
			return this._loadingForms.ContainsKey(prefabPath);
		}

		public UIBase GetForm(string prefabPath) {
			return this._allForms[prefabPath];
		}
	}
}