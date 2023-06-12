using System;
using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Test {
    public class UIBackButton : UIFixed {

        public Button BackButton;

        public override void OnInit(Object param) {
            this.BackButton.onClick.AddListener(() => {
                FormMgr.BackScene(null).Forget();
            });

            var width = UnityEngine.Screen.width;
            var height = UnityEngine.Screen.height;
            var rect = this.rectTransform.rect;
            var x = width / 2f - rect.width / 2f;
            var y = height / 2f - rect.height / 2f;
            this.rectTransform.localPosition = new Vector3(-x, y, 0);    
        }

        private void Start() {
            
        }


        void Update() {
        
        }
    }
}
