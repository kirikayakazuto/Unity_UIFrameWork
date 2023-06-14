using Cysharp.Threading.Tasks;

namespace Logic.Managers {
    public class ConfigMgr {
        public async UniTask<bool> LoadConfigs() {
            await UniTask.Delay(1);
            return false;
        }
        public void OnConfigChange() {
            
        }
    }
}