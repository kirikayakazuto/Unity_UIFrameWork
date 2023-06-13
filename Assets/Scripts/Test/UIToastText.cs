using Cysharp.Threading.Tasks;
using DG.Tweening;
using FrameWork;
using TMPro;
using UnityEngine;

namespace Test {
    public class UIToastText : UIToast {

        public TextMeshProUGUI text;
        void Start() {
            
        }

        void Update() {
        
        }

        public override async UniTask<bool> OnShowEffect() {
            this.rectTransform.localPosition = new Vector3(0, 0, 0);
            await UniTask.Delay(300); 
            await this.rectTransform.DOLocalMoveY(-50, 0.3f);
            await this.CloseSelf();
            return true;
        }
    }
}
