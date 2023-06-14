using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace FrameWork.Structure {
	public class LRUCache {
		public const int MAX_SIZE = 5;
		private ObjectPool<LRUNode> pool = new ObjectPool<LRUNode>(() => new LRUNode("", new LRUNode("")));

		private readonly LRUNode head;
		private LRUNode last;
		private int size;
		
		public LRUCache() {
			this.head = new LRUNode("head");
			this.size = 0;
		}
		
		// 把node放到最前面
		public bool Put(string v) {
			if (this.size <= 0) {
				this.last = this.pool.Get();
				this.last.value = v;
				this.last.prev = head;
				this.head.next = this.last;
				this.size = 1;
				return true;
			}
			var node = this.GetByValue(v);
			if (node == null) {
				node = this.pool.Get();
				node.value = v;
				this.AddHead(node);
				return true;
			}
			// 如果是最后一个, 那么需要替换last
			if (this.last == node) {
				this.last = node.prev;
			}
			this.RemoveNode(node);
			this.AddHead(node);
			return true;
		}

		public void AddHead(LRUNode node) {
			node.next = this.head.next;
			if (node.next != null) node.next.prev = node;
			this.head.next = node;
			node.prev = this.head;
			this.size++;
		}
		

		private LRUNode GetByValue(string v) {
			var next = this.head.next;
			while (next != null) {
				if (next.value == v) return next;
				next = next.next;
			}
			return null;
		}

		private void RemoveNode(LRUNode node) {
			node.prev.next = node.next;
			if (node.next != null) {
				node.next.prev = node.prev;
			}
			this.size--;
		}

		public bool NeedToDeleted() {
			return this.size > LRUCache.MAX_SIZE;
		}
	
		public string DeleteLastNode() {
			var value = this.last.value;
			this.RemoveNode(this.last);
			this.pool.Release(this.last);
			this.last = this.last.prev;
			return value;
		}

		public void Remove(string v) {
			var node = this.GetByValue(v);
			if(node == null) return;
			this.RemoveNode(node);
		}
	}

	public class LRUNode {
		public LRUNode prev;
		[CanBeNull] public LRUNode next;
		public string value;
		public LRUNode(string value, [CanBeNull] LRUNode next = null) {
			this.next = next;
			this.value = value;
		}

		public void Use(string _value, [CanBeNull] LRUNode _next = null) {
			this.value = _value;
			this.next = _next;
		}

		public void Free() {
			this.value = "";
			this.next = null;
		}
	}
}