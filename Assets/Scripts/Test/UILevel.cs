using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;

namespace Test {
    public class UILevel : UIScreen {
        public override void OnInit(Object param) {
            FormMgr.Open(Form.UIBackButton).Forget();
            FormMgr.Open(Form.UIRole).Forget();
            FormMgr.Open(Form.UISkills).Forget();
        }

        public override void OnShow(Object param) {
            UniTask.Delay(3000).GetAwaiter().OnCompleted(() => {
                if(null == this) return;
                FormMgr.Open(Form.UIPass).Forget();    
            });
        }

        private void Start() {
        
        }

        private void Update() {
        
        }
        
    }
}
