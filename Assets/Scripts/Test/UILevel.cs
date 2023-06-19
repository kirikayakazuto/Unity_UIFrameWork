using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;

namespace Test {
    public class UILevel : UIScreen {
        public override void OnInit(Object param) {
            FormMgr.Open(UIConfigs.UIBackButton).Forget();
            FormMgr.Open(UIConfigs.UIRole).Forget();
            FormMgr.Open(UIConfigs.UISkills).Forget();
        }

        public override void OnShow(Object param) {
            UniTask.Delay(3000).GetAwaiter().OnCompleted(() => {
                if(null == this) return;
                FormMgr.Open(UIConfigs.UIPass).Forget();    
            });
        }

        private void Start() {
        
        }

        private void Update() {
        
        }
        
    }
}
