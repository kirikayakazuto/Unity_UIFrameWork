using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIMap : UIScreen {

        public Button UpgradeButton;
        
        public override void OnInit(Object param) {
            FormMgr.Open(UIConfigs.UIBackButton, null).Forget();
        }

        void Start() {
            this.UpgradeButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIUpgrade, null).Forget();
            });
        }

        void Update() {
        
        }
    }
}
