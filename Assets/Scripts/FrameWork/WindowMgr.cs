using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork {
	public class WindowMgr {
		public static WindowMgr instance { get; } = new WindowMgr();
		
		private readonly Stack<IFormConfig> _showingStack = new Stack<IFormConfig>();
		private readonly Queue<WindowData> _watingQueue = new Queue<WindowData>();
		
		public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			var prefabPath = formConfig.prefabUrl;

			var showWait = formData?.showWait ?? false;
			if (this._showingStack.Count <= 0 || (!showWait || this._watingQueue.Count <= 0)) {
				ModalMgr.instance.CheckModalWindow(this._showingStack.ToArray());
				var com= await UIManager.GetInstance().OpenForm(formConfig, param, formData);
				this._showingStack.Push(formConfig);
				com.gameObject.layer = this._showingStack.Count;
				return com;
			}

			var windowData = new WindowData() { formConfig = formConfig, param = param, formData = formData};
			
			this._watingQueue.Enqueue(windowData);
			return await UIManager.GetInstance().LoadForm(formConfig);
		}

		public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
			if (!this._showingStack.Contains(formConfig)) return false;
			if (!Utils.RemoveAtStack(this._showingStack, formConfig)) return false;
			await UIManager.GetInstance().CloseForm(formConfig, param, formData);
			if (this._watingQueue.TryDequeue(out var windowData)) {
				await this.Open(windowData.formConfig, windowData.param, windowData.formData);
			}
			return true; 
		}

		public async UniTask<bool> CloseTop([CanBeNull] Object param, IFormData? formData) {
			if (this._showingStack.Count <= 0) return false;
			var formConfig = this._showingStack.Pop();
			await UIManager.GetInstance().CloseForm(formConfig, param, formData);
			if (this._watingQueue.TryDequeue(out var windowData)) {
				await this.Open(windowData.formConfig, windowData.param, windowData.formData);
			}
			return true;
		}


		public async UniTask<bool> CloseAll() {
			while (this._showingStack.Count > 0) {
				var formConfig = this._showingStack.Pop();
				await UIManager.GetInstance().CloseForm(formConfig, null, null);
			}
			return true;
		}


		

	
		
		
	}
}