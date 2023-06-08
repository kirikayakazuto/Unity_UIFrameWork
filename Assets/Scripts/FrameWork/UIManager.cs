using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
	public class UIManager {
		private Transform _UIROOT;
		private Transform _NdScreen;
		private Transform _NdFixed;
		private Transform _NdPop;
		private Transform _NdToast;
		private Transform _NdTips;

		private readonly Dictionary<string, UIBase> _allForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> _showingForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> _tipsForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> _loadingForms = new Dictionary<string, UIBase>();
		private readonly Dictionary<string, UIBase> _closingForms = new Dictionary<string, UIBase>();

		private static UIManager instance;
		public static UIManager GetInstance() {
			if (UIManager.instance != null) return UIManager.instance;
			
			var self = UIManager.instance = new UIManager();
			var activeScene = SceneManager.GetActiveScene();
			var objects = activeScene.GetRootGameObjects();
			var canvas = (from t in objects where t.name == "Canvas" select t.GetComponent<Transform>()).FirstOrDefault();
			if (canvas == null) {
				Debug.Log("UIManager: 没有找到Canvas");
				return self;
			}
			var scene = canvas.Find("Scene");
			if (scene == null) {
				Debug.Log("UIManager: 没有找到Scene");
				return self;
			}
			self._UIROOT = new GameObject(SysDefine.SYS_UIROOT_NODE).AddComponent<Transform>();
			self._UIROOT.parent = scene;
			self._NdScreen = new GameObject(SysDefine.SYS_SCREEN_NODE).AddComponent<Transform>();
			self._NdScreen.parent = self._UIROOT;
			self._NdFixed = new GameObject(SysDefine.SYS_FIXED_NODE).AddComponent<Transform>();
			self._NdFixed.parent = self._UIROOT;
			self._NdPop = new GameObject(SysDefine.SYS_POPUP_NODE).AddComponent<Transform>();
			self._NdPop.parent = self._UIROOT;
			self._NdToast = new GameObject(SysDefine.SYS_TOAST_NODE).AddComponent<Transform>();
			self._NdToast.parent = self._UIROOT;
			self._NdTips = new GameObject(SysDefine.SYS_TIPS_NODE).AddComponent<Transform>();
			self._NdTips.parent = self._UIROOT;


			return UIManager.instance;
		}

		public UIBase LoadForm(string prefabPath) {
			var gameObject = (GameObject)Resources.Load(prefabPath);
			return this.AddTransformTree(gameObject.GetComponent<Transform>());
		}
		public UIBase OpenForm(IFormConfig formConfig) {
			return null;
		}

		public UIBase AddTransformTree(Transform transform) {
			var com = transform.GetComponent<UIBase>();
			transform.gameObject.SetActive(false);

			transform.parent = com.formType switch {
				FormType.Screen => this._NdScreen,
				FormType.Fixed => this._NdFixed,
				FormType.Window => this._NdPop,
				FormType.Toast => this._NdToast,
				FormType.Tips => this._NdTips,
				_ => transform.parent
			};

			this._allForms[com.fid] = com;

			return com;
		}

		public bool checkFormShowing(string prefabPath) {
			return this._showingForms[prefabPath] != null;
		}
	}
}