using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine.UI;

namespace Test {
    public class UIAbout : UIScreen {

        public Button BackButton;

        private void Start() {
            this.BackButton.onClick.AddListener((() => {
                FormMgr.BackScene(null, null).Forget();
            }));
        }

        void Update() {
        
        }
    }
}
