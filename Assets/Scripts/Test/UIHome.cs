using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine.UI;

namespace Test {
    public class UIHome : UIScreen {
        public Button AboutButton;
        void Start() {
            this.AboutButton.onClick.AddListener((() => {
                FormMgr.Open(UIConfigs.UIAbout, null, null).Forget();
            }));
        }

    
        void Update() {
        
        }
    }
}
