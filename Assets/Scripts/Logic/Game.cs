using Cysharp.Threading.Tasks;
using Logic.Managers;

namespace Logic {
    public static class Game {
        private static bool _inited = false;
        
        public static readonly ConfigMgr ConfigMgr = new ConfigMgr();
        public static async UniTask<bool> Init() {
            if(Game._inited) return false;
            
            await Game.ConfigMgr.LoadConfigs();
            
            Game._inited = true;
            return true;
        }

        public static void Update() {
            if(!Game._inited) return;
            // UnityEngine.Time.deltaTime
        }
    }
}