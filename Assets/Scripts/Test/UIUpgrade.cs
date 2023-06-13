using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIUpgrade : UIWindow {

        public Button CompletedButton;
        public Button CloseButton;
    
        void Start() {
            this.CompletedButton.onClick.AddListener(() => {
                FormMgr.Open(UIConfigs.UICompleted).Forget();
            });
        }

        public override void OnInit(Object param) {
            this.rectTransform.localScale = new Vector3(0, 0, 0);
        }

        void Update() {
        
        }
    }
}
