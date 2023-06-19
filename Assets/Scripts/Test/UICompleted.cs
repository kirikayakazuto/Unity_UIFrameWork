using System;
using Cysharp.Threading.Tasks;
using FrameWork;
using FrameWork.Structure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Test {
    public class UICompleted : UIWindow {

        public override CloseType closeType { get; set; } = CloseType.Destory;

        public override void OnInit(Object param) {
            this.Test().Forget();
        }

        private async UniTask<bool> Test() {
            for (var i = 0; i < 10; i++) {
                await UniTask.Delay(1000);
                if (this == null) return false;
                Debug.Log(i + " : " + this.gameObject.name);
            }
            return false;
        }
        
        void Start() {
            
        }

        void Update() {
        
        }

        private void OnDestroy() {
            Debug.Log("do destroy");
        }
    }
}
