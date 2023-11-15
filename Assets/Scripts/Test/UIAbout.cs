using Cysharp.Threading.Tasks;
using FrameWork;
using FrameWork.Structure;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UIAbout : UIScreen {
        
        public override void OnInit(Object param) {
            FormMgr.Open(Form.UIBackButton, null).Forget();
            FormMgr.Open(Form.UISound).Forget();
        }

        private void Start() {
            
        }

        void Update() {
        
        }
    }
}
