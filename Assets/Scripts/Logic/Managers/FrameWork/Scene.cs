using System;
using Logic;
using UnityEngine;

namespace FrameWork {
    public class Scene : MonoBehaviour {

        private async void OnEnable() {
            await Game.Init(); 
        }

        void Start() {
        
        }
        
        void Update() {
            Game.Update();
        }
    }
}
