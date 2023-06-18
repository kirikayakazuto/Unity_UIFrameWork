using FrameWork;
using UnityEngine;

namespace Test {
    public class UIRole : UIFixed {
        
        public override void OnInit(Object param) {
            var width = UnityEngine.Screen.width;
            var height = UnityEngine.Screen.height;
            var rect = this.rectTransform.rect;
            var x = width / 2f - rect.width / 2f;
            var y = height / 2f - rect.height / 2f;
            this.rectTransform.localPosition = new Vector3(x, y, 0);   
        }

        private void Update() {
        
        }
    }
}