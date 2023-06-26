namespace FrameWork {
	
	public struct TestData {
		public string testName;
	}
	
	
	public static class BroadcastUtils {
		public static readonly Broadcast<TestData> broadcast = new Broadcast<TestData>();
	}
	
}