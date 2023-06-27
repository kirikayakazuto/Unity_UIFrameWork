using Logic;
using UnityEngine;

namespace FrameWork {
    public class Scene : MonoBehaviour {
    
        void Start() {
        
        }

        public async void OnGameInit() {
            await Game.Init();
        }

        void Update() {
            Game.Update();
        }
    }
}
