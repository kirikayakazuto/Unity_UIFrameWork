using FrameWork.Structure;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork {
	public abstract class UIBase: MonoBehaviour {
		// 唯一id
		public string fid;
		// 窗体数据
		public IFormData formData;
		// 窗体类型
		public abstract FormType formType { get; set; }
		// 关闭类型
		public abstract CloseType closeType { get; set; }
		/** 是否已经调用过 pre init 方法 */
		private bool _inited = false;

		public async UniTask<bool> _PreInit([CanBeNull] Object param) {
			if(this._inited) return true;
			this._inited = true;
			
		 	await this.Load();
		    
		    this.OnInit(param);
		    return true;
		}
		public virtual async UniTask<bool> Load() { return false; }
		/** 初始化, 只会调用一次 */
		public virtual void OnInit([CanBeNull] Object param) { }

		public virtual void OnShow([CanBeNull] Object param) { }

		public virtual void OnAfterShow([CanBeNull] Object param) { }

		public virtual void OnHide([CanBeNull] Object param) { }

		public virtual void OnAfterHide([CanBeNull] Object param) { }

		/** 出现动画 */
		public virtual async UniTask<bool> OnShowEffect() {
			
			// var task = Task.Run()
			var rectTransform = this.GetComponent<RectTransform>();
			rectTransform.DOScale(new Vector3(1, 1, 1), 1).OnComplete(() => {

			});
			
			return false;
		}
		/** 隐藏动画 */
		public virtual async UniTask<bool> OnHideEffect() { return false; }
		
		/** 屏蔽触摸事件 */
		public void SetBlockInput(bool block) {
			
		}

		public async UniTask<bool> CloseSelf() {
			return false;
		}

	}
}