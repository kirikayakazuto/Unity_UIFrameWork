namespace FrameWork.Structure {
	public struct IFormConfig {
		public string prefabUrl;
		public string type;
	}
	public struct IFormData {
		public IFormConfig? loadingForm;
		public bool quickly;
		
		public IFormData(IFormConfig? loadingForm, bool quickly = false) {
			this.loadingForm = loadingForm;
			this.quickly = quickly;
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
		public ModalOpacity opacity;
		public bool clickMaskClose;

		public ModalType(ModalOpacity opacity = ModalOpacity.OpacityHalf, bool clickMaskClose = true) {
			this.opacity = opacity;
			this.clickMaskClose = clickMaskClose;
		}
	}

	public class PriorityElement<T> {
		public T data;
		public int priority;
	}
}