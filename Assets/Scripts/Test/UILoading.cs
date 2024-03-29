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
            
            this.RightPanel.localPosition = new Vector3(700, 0, 0);
            this.LeftPanel.localPosition = new Vector3(-700, 0, 0);
            
            var task1 = this.RightPanel.DOLocalMoveX(225, 0.3f).ToUniTask();
            var task2 = this.LeftPanel.DOLocalMoveX(-225, 0.3f).ToUniTask();
            
            await UniTask.WhenAll(task1, task2);
            
            return true;
        }

        public override async UniTask<bool> OnHideEffect() {
            
            this.RightPanel.localPosition = new Vector3(225, 0, 0);
            this.LeftPanel.localPosition = new Vector3(-225, 0, 0);
            
            await UniTask.Delay(300);
            var task1 = this.RightPanel.DOLocalMoveX(700, 0.3f).ToUniTask();
            var task2 = this.LeftPanel.DOLocalMoveX(-700, 0.3f).ToUniTask();
            
            await UniTask.WhenAll(task1, task2);
            
            return true;
        }
    }
}
