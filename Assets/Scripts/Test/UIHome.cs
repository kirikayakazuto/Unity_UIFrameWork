using Cysharp.Threading.Tasks;
using FrameWork;
using Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIHome : UIScreen {

        public Button StartButton;
        public Button AboutButton;

        public override void OnInit(Object param) {
            FormMgr.Open(UIConfigs.UISound).Forget();
        }

        private void Start() {
            this.StartButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UIMap, null).Forget();
            });
            this.AboutButton.onClick.AddListener((() => {
                FormMgr.Open(UIConfigs.UIAbout, null).Forget();
            }));

            // Game.ConfigMgr.LoadConfigs();
        }

    
        void Update() {
        
        }
    }
}
