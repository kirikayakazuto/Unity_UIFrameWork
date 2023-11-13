using UnityEngine;

namespace FrameWork {
	
	/**
	 * 框架内部提供的事件
	 */
	public struct FormData {
		public string fid;
	}
	
	public struct TestData {
		public string testName;
	}
	
	
	public static class BroadcastUtils {
		
		// 窗体关闭事件
		public static readonly Broadcast<FormData> FormCloseEvent = new Broadcast<FormData>();
		
		// 窗体开启事件
		public static readonly Broadcast<FormData> FormOpenEvent = new Broadcast<FormData>();
		
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