using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using FrameWork.Structure;
using UnityEngine;

namespace FrameWork {
	public class SceneMgr {
		public static SceneMgr instance { get; } = new SceneMgr();
		
		private IFormConfig currScene;
		private readonly Stack<IFormConfig> scenes = new Stack<IFormConfig>();

		public UIBase GetCurrScene() {
			return UIManager.GetInstance().GetForm(this.currScene.prefabUrl);
		}

		public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData formData = new IFormData()) {
			var prefabUrl = formConfig.prefabUrl;
			if (this.currScene.prefabUrl == prefabUrl) {
				Debug.LogWarning("SceneMgr: curr scene == open scene, prefabUrl: " + prefabUrl);
				return null;
			}
			
			await this.OpenLoading(param, formData);
			
			// 关闭其他显示中的UI
			var showingForms = UIManager.GetInstance().showingForms;
			if (showingForms.Count > 0) {
				var uniTasks = new UniTask<bool>[showingForms.Count];
				var idx = 0;
				foreach (var keyValuePair in showingForms) {
					if(keyValuePair.Value.formType == FormType.Tips) continue;
					uniTasks[idx] = keyValuePair.Value.CloseSelf(param, formData);
					idx++;
				}
				await UniTask.WhenAll(uniTasks);	
			}
			
			this.currScene = formConfig;
			var com = await UIManager.GetInstance().OpenForm(formConfig, param, formData);
			this.scenes.Push(formConfig);
			
			await this.CloseLoading(param, formData);
			
			return com;
		}

		public async UniTask<bool> Back([CanBeNull] Object param, IFormData formData = new IFormData()) {
			if (this.scenes.Count <= 1) {
				Debug.LogError("SceneMgr: Back error, only one scene");
				return false;
			}

			await this.OpenLoading(param, formData);
		 	
		    var scene = this.scenes.Pop();
		    await UIManager.GetInstance().CloseForm(scene, param, formData);

		    this.currScene = this.scenes.Peek();
		    await UIManager.GetInstance().OpenForm(this.currScene, param, formData);
		    
		    await this.CloseLoading(param, formData);

		    return true;
		}

		public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param, IFormData formData = new IFormData()) {
			return await UIManager.GetInstance().CloseForm(formConfig, param, formData);
		}

		private async UniTask<UIBase> OpenLoading([CanBeNull] Object param, IFormData formData = new IFormData()) {
			var loadingForm = formData.loadingForm ?? SysDefine.defaultLoadingForm;
			if (loadingForm.prefabUrl.Length <= 0) return null;
			await TipsMgr.instance.Open(loadingForm, param, formData);
			return null;
		}

		private async UniTask<bool> CloseLoading([CanBeNull] Object param, IFormData formData = new IFormData()) {
			var loadingForm = formData.loadingForm ?? SysDefine.defaultLoadingForm; 
			if (loadingForm.prefabUrl.Length <= 0) return false;
			await TipsMgr.instance.Close(loadingForm, param, formData);
			return true;
		}
	}
}