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

        private void Start() {
        
        }

        private void Update() {
        
        }
    }
}
