using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIMap : UIScreen {

        public Button UpgradeButton;
        public Button EncyckoButton;
        public Button LevelButton;
        
        public override void OnInit(Object param) {
            FormMgr.Open(UIConfigs.UIBackButton, null).Forget();
            FormMgr.Open(UIConfigs.UISound).Forget();
        }

        void Start() {
            this.UpgradeButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIUpgrade).Forget();
            });
            this.EncyckoButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIToastText).Forget();
            });
            this.LevelButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UILevel).Forget();
            });
        }

        void Update() {
        
        }
    }
}
