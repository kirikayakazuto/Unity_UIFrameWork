using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork {
	public class SceneMgr {
		public SceneMgr instance { get; } = new SceneMgr();
		

		private IFormConfig currScene;
		private Stack<IFormConfig> scenes = new Stack<IFormConfig>();

		public UIBase GetCurrScene() {
			return UIManager.GetInstance().GetForm(this.currScene.prefabUrl);
		}

		public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			var prefabUrl = formConfig.prefabUrl;
			if (this.currScene.prefabUrl == prefabUrl) {
				Debug.LogWarning("SceneMgr: curr scene == open scene, prefabUrl: " + prefabUrl);
				return null;
			}
			await this.OpenLoading();
			if (this.scenes.Count > 0) {
				var scene = this.scenes.Peek();
				await UIManager.GetInstance().CloseForm(this.currScene, param, formData);
			}
			this.currScene = formConfig;

			var com = await UIManager.GetInstance().OpenForm(formConfig, param, formData);
			await this.CloseLoading();
			
			return com;
		}

		public async UniTask<bool> Back([CanBeNull] Object param, IFormData? formData) {
			if (this.scenes.Count <= 1) {
				Debug.LogError("SceneMgr: Back error, only one scene");
				return false;
			}
		 	await this.OpenLoading();
		    var scene = this.scenes.Pop();
		    await UIManager.GetInstance().CloseForm(scene, param, formData);

		    this.currScene = this.scenes.Peek();
		    await UIManager.GetInstance().OpenForm(this.currScene, param, formData);
		    await this.CloseLoading();

		    return true;
		}

		public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			return await UIManager.GetInstance().CloseForm(formConfig, param, formData);
		}

		private async UniTask<UIBase> OpenLoading() {
			return null;
		}

		private async UniTask<bool> CloseLoading() {
			return false;
		}
	}
}