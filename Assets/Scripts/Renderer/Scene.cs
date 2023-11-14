using Logic;
using UnityEngine;

namespace Renderer {
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
