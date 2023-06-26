using UnityEngine;

namespace FrameWork {
	
	public struct TestData {
		public string testName;
	}
	
	
	public static class BroadcastUtils {
		public static readonly Broadcast<TestData> broadcast = new Broadcast<TestData>();
	}


	public class TestBroadcast {
		public void Test() {
			
			BroadcastUtils.broadcast.On(this.OnTestMessage);
        
			BroadcastUtils.broadcast.On(this.OnTestMessage2);
        
			Debug.Log(BroadcastUtils.broadcast.Has(this.OnTestMessage));
        
			BroadcastUtils.broadcast.Emit(new TestData() {testName = "this is test name"});
        
			BroadcastUtils.broadcast.Off(this.OnTestMessage);
        
			BroadcastUtils.broadcast.Emit(new TestData() {testName = "this is test name"});
			
		}
		
		
		private void OnTestMessage(TestData t) {
			Debug.Log("OnTestMessage: " + t.testName);
		}

		private void OnTestMessage2(TestData t) {
			Debug.Log("OnTestMessage2: " + t.testName);   
		}
	}
}