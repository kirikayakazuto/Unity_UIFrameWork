using Logic;
using UnityEngine;

namespace Renderer {
    public class Scene : MonoBehaviour {

        private async void OnEnable() {
            await Game.Init(); 
        }

        void Start() {
        
        }

        private void Update() {
            Game.Update();
        }
    }
}
