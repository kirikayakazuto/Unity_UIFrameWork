using Cysharp.Threading.Tasks;
using DG.Tweening;
using FrameWork;
using UnityEngine;

namespace Test {
    public class UILoading : UITips {

        public RectTransform RightPanel;
        public RectTransform LeftPanel;
        
        void Start() {
        
        }

        public override async UniTask<bool> OnShowEffect() {
            
            this.RightPanel.localPosition = new Vector3(500, 0, 0);
            this.LeftPanel.localPosition = new Vector3(-500, 0, 0);
            
            this.RightPanel.DOLocalMoveX(225, 0.3f);
            this.LeftPanel.DOLocalMoveX(-225, 0.3f);
            await UniTask.Delay(300);
            return true;
        }

        public override async UniTask<bool> OnHideEffect() {
            
            this.RightPanel.localPosition = new Vector3(225, 0, 0);
            this.LeftPanel.localPosition = new Vector3(-225, 0, 0);
            
            await UniTask.Delay(300);
            this.RightPanel.DOLocalMoveX(500, 0.3f);
            this.LeftPanel.DOLocalMoveX(-500, 0.3f);
            await UniTask.Delay(300);
            return true;
        }
    }
}
