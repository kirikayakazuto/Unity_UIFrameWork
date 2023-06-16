using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork {
	public class WindowMgr {
		public static WindowMgr instance { get; } = new WindowMgr();
		
		private readonly Stack<IFormConfig> _showingStack = new Stack<IFormConfig>();
		private readonly Queue<WindowData> _watingQueue = new Queue<WindowData>();
		
		public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			
			formData.onOpenBeforShowEffect = (UIBase com) => {
				ModalMgr.instance.CheckModalWindow(this._showingStack.ToArray());
			};

			var showWait = formData.showWait;
			if (this._showingStack.Count <= 0 || (!showWait || this._watingQueue.Count <= 0)) {
				this._showingStack.Push(formConfig);
				var com= await UIManager.GetInstance().OpenForm(formConfig, param, formData);
				com.rectTransform.SetSiblingIndex(this._showingStack.Count);
				
				return com;
			}

			var windowData = new WindowData() { formConfig = formConfig, param = param, formData = formData};
			
			this._watingQueue.Enqueue(windowData);
			return await UIManager.GetInstance().LoadForm(formConfig);
		}

		public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			if (!this._showingStack.Contains(formConfig)) return false;
			if (!Utils.RemoveAtStack(this._showingStack, formConfig)) return false;
			
			formData.onCloseBeforHideEffect = () => {
				ModalMgr.instance.CheckModalWindow(this._showingStack.ToArray());
			};
			
			await UIManager.GetInstance().CloseForm(formConfig, param, formData);
			if (this._watingQueue.TryDequeue(out var windowData)) {
				await this.Open(windowData.formConfig, windowData.param, windowData.formData);
			}
			return true; 
		}

		public async UniTask<bool> CloseTop([CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			if (this._showingStack.Count <= 0) return false;

			formData.onCloseBeforHideEffect = () => {
				ModalMgr.instance.CheckModalWindow(this._showingStack.ToArray());
			};
			
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
				await UIManager.GetInstance().CloseForm(formConfig, null);
			}
			return true;
		}


		

	
		
		
	}
}