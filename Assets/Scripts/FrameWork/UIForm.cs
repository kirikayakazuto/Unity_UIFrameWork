using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FrameWork {
	public class UIScreen: UIBase {
		public override FormType formType { get; set; } = FormType.Screen;
		public override CloseType closeType { get; set; } = CloseType.Destory;
		
	}

	public class UIFixed: UIBase {
		public override FormType formType { get; set; } = FormType.Fixed;
		public override CloseType closeType { get; set; } = CloseType.Hide;
	}

	public class UIWindow: UIBase {
		public override FormType formType { get; set; } = FormType.Window;
		public override CloseType closeType { get; set; } = CloseType.Hide;
		public ModalType modalType = new ModalType();

		public override async UniTask<bool> OnShowEffect() {
			var rectTransform = this.GetComponent<RectTransform>();
		 	await rectTransform.DOScale(Vector3.one, 0.3f);
		    return true;
		}
	}

	public class UITips: UIBase {
		public override FormType formType { get; set; } = FormType.Tips;
		public override CloseType closeType { get; set; } = CloseType.Hide;
	}

	public class UIToast: UIBase {
		public override FormType formType { get; set; } = FormType.Toast;
		public override CloseType closeType { get; set; } = CloseType.Hide;
	}
	
	
	
	
}