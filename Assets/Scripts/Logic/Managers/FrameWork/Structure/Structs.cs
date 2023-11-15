using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Logic;
using UnityEngine;

namespace FrameWork.Structure {
	public struct IFormConfig {
		// 窗体名称
		public string name;
		// ab包下的路径
		public string prefabUrl;
		// 窗体类型
		public FormType type;

		public async UniTask<UIBase> Open([CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			return await FormMgr.Open(this, param, formData);
		}
		
		public async UniTask<bool> Close([CanBeNull] Object param = null, IFormData formData = new IFormData()) {
			return await FormMgr.Close(this, param, formData);
		}
	}

	public struct INodeConfig {
		// 窗体名称
		public string name;
		// ab包下的路径
		private string prefabUrl;

		public async UniTask<GameObject> Instance() {
			return await Game.NodeManager.Instantiate(this.prefabUrl);
		}

		public void Release() {
			Game.NodeManager.Release(this.prefabUrl);
		}
	}

	public delegate void OnOpen(UIBase com);
	public delegate void OnClose();
	
	public struct IFormData {
		public IFormConfig? loadingForm;
		[CanBeNull] public OnOpen onOpenBeforShowEffect;
		[CanBeNull] public OnClose onCloseBeforHideEffect;
		[CanBeNull] public OnClose onClose;
		public bool quickly;
		// window 才有
		public bool showWait; 
		
		public IFormData(IFormConfig? loadingForm) {
			this.loadingForm = loadingForm;
			this.quickly = false;
			this.showWait = false;
		   	this.onClose = null;
		    this.onOpenBeforShowEffect = null;
		    this.onCloseBeforHideEffect = null;
		}
	}

	public enum FormType {
		Screen,
		Fixed,
		Window,
		Toast,
		Tips,
	}
	
	public enum CloseType {
		Hide,
		Destory,
		LRU
	}

	public enum ModalOpacity {
		/** 没有mask, 可以穿透 */
		None,
		/** 完全透明，不能穿透 */
		OpacityZero,
		/** 高透明度，不能穿透 */
		OpacityLow,
		/** 半透明，不能穿透 */
		OpacityHalf,
		/** 低透明度, 不能穿透 */
		OpacityHigh,
		/** 完全不透明 */
		OpacityFull
	}

	public class ModalType {
		public bool useEase;
		public ModalOpacity opacity;
		public bool clickMaskClose;

		public ModalType(ModalOpacity opacity = ModalOpacity.OpacityHalf, bool clickMaskClose = true, bool useEase = true) {
			this.opacity = opacity;
			this.clickMaskClose = clickMaskClose;
			this.useEase = useEase;
		}
	}

	public class PriorityElement<T> {
		public T data;
		public int priority;
	}

	public struct WindowData {
		public IFormConfig formConfig;
		[CanBeNull] public Object param;
		public IFormData formData;
	}
}