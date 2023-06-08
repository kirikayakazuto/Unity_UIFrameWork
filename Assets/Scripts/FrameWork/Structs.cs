namespace FrameWork {

	public struct IFormConfig {
		public string prefabUrl;
    	public string type;
	}
	public struct IFormData {
		
	}

	public enum FormType {
		Screen,
		Fixed,
		Window,
		Toast,
		Tips,
	}
}