using Cysharp.Threading.Tasks;
using FrameWork;
using Logic.Managers;

namespace Logic {
    public static class Game {
        private static bool _inited = false;
        
        public static ConfigMgr ConfigMgr;
        public static SoundMgr SoundMgr;
        public static UIManager UIMgr;
        public static async UniTask<bool> Init() {
            if(Game._inited) return false;
            
            // new mgr 
            Game.UIMgr = new UIManager();
            Game.SoundMgr = new SoundMgr();
            Game.ConfigMgr = new ConfigMgr();
            // Game.ConfigMgr
            
            // load configs
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