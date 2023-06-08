using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork {
	public abstract class UIBase: MonoBehaviour {
		// 唯一id
		public string fid;
		// 窗体数据
		public IFormData formData;
		// 窗体类型
		public FormType formType;
		// 关闭类型
		public CloseType closeType;
		/** 是否已经调用过 pre init 方法 */
		private bool _inited = false;

		public void _PreInit([CanBeNull] Object param) {
			if(this._inited) return;
			this._inited = true;

			this.OnInit(param);
		}
		/** 初始化, 只会调用一次 */
		public abstract void OnInit([CanBeNull] Object param);

		public virtual void OnShow([CanBeNull] Object param) { }

		public virtual void OnAfterShow([CanBeNull] Object param) { }

		public virtual void OnHide() { }

		public virtual void OnAfterHide() { }
		
		/** 出现动画 */
		public virtual void OnShowEffect() { }
		/** 隐藏动画 */
		public virtual void OnHideEffect() { }
		
		/** 屏蔽触摸事件 */
		public void SetBlockInput(bool block) {
			
		}

		public void CloseSelf() {
			
		}

	}
}