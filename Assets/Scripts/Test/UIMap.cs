using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIMap : UIScreen {

        public Button UpgradeButton;
        public Button EncyckoButton;
        
        public override void OnInit(Object param) {
            FormMgr.Open(UIConfigs.UIBackButton, null).Forget();
        }

        void Start() {
            this.UpgradeButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIUpgrade, null).Forget();
            });
            this.EncyckoButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIToastText).Forget();
            });
        }

        void Update() {
        
        }
    }
}
