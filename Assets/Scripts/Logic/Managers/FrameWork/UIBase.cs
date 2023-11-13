using System;
using FrameWork.Structure;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace FrameWork {
	public abstract class UIBase: MonoBehaviour {
		// 唯一id
		[NonSerialized]
		public string fid = "";
		// 窗体数据
		public IFormData formData;
		// 窗体类型
		public abstract FormType formType { get; set; }
		// 关闭类型
		public abstract CloseType closeType { get; set; }
		/** 是否已经调用过 pre init 方法 */
		private bool _inited = false;
		
		[NonSerialized]
		public RectTransform rectTransform;

		public async UniTask<bool> _PreInit([CanBeNull] Object param) {
			if(this._inited) return true;
			this._inited = true;

			this.rectTransform = this.GetComponent<RectTransform>();
			await this.Load();
		    
		    this.OnInit(param);
		    return true;
		}

		public virtual async UniTask<bool> Load() {
			await UniTask.Delay(1);
			return false;
		}

		/** 初始化, 只会调用一次 */
		public virtual void OnInit([CanBeNull] Object param) { }

		public virtual void OnShow([CanBeNull] Object param) { }

		public virtual void OnAfterShow([CanBeNull] Object param) { }

		public virtual void OnHide([CanBeNull] Object param) { }

		public virtual void OnAfterHide([CanBeNull] Object param) { }

		/** 出现动画 */
		public virtual async UniTask<bool> OnShowEffect() {
			await UniTask.DelayFrame(1);
			return false;
		}

		/** 隐藏动画 */
		public virtual async UniTask<bool> OnHideEffect() {
			await UniTask.DelayFrame(1);
			return false;
		}

		/** 屏蔽触摸事件 */
		private Image _blockInputImage; 
		protected void SetBlockInput(bool block) {
			if (this._blockInputImage == null) {
				var image = this._blockInputImage = Utils.GenSingleColorImage("BlockInput");
				image.rectTransform.SetParent(this.rectTransform);
			
				image.color = new Color(0, 0, 0, 0);
				image.rectTransform.localPosition = new Vector3(0, 0, 0);
				image.rectTransform.sizeDelta = new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);	
			}
			this._blockInputImage.gameObject.SetActive(block);
		}

		public virtual async UniTask<bool> CloseSelf([CanBeNull] Object param = null, IFormData tFormData = new IFormData()) {
			return await Game.UIMgr.CloseForm(new IFormConfig() {prefabUrl = this.fid, type = this.formType}, param, tFormData);
		}
		
		protected bool Equals(UIBase other) {
			return base.Equals(other) && this.fid == other.fid;
		}

		public override int GetHashCode() {
			return HashCode.Combine(base.GetHashCode(), this.fid);
		}
	}
}