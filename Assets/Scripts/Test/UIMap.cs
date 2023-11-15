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
            FormMgr.Open(Form.UIBackButton, null).Forget();
            FormMgr.Open(Form.UISound).Forget();
        }

        void Start() {
            this.UpgradeButton.onClick.AddListener(() => {
                FormMgr.Open(Form.UIUpgrade).Forget();
            });
            this.EncyckoButton.onClick.AddListener(() => {
                FormMgr.Open(Form.UIToastText).Forget();
            });
            this.LevelButton.onClick.AddListener(() => {
                FormMgr.Open(Form.UILevel).Forget();
            });
        }

        void Update() {
        
        }
    }
}
