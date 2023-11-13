using FrameWork.Structure;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Logic;
using UnityEngine;

namespace FrameWork {
	public class TipsMgr {
		public static TipsMgr instance { get; } = new TipsMgr();

		public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData formData = new IFormData()) {
			return await Game.UIMgr.OpenForm(formConfig, param, formData);
		}

		public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param, IFormData formData = new IFormData()) {
			return await Game.UIMgr.CloseForm(formConfig, param, formData);
		}
	}
}