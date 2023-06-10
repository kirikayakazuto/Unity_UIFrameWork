using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace FrameWork {
    public class ToastMgr {
        private readonly Dictionary<string, ObjectPool<UIToast>> pools = new Dictionary<string, ObjectPool<UIToast>>();
        private readonly Dictionary<string, List<UIToast>> showingList = new Dictionary<string, List<UIToast>>();

        public async UniTask<UIToast> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
            if (!this.pools.TryGetValue(formConfig.prefabUrl, out var pool)) {
                pool = await this.GenPool(formConfig.prefabUrl);
                this.pools[formConfig.prefabUrl] = pool;
            }

            var com = pool.Get();
            await UIManager.GetInstance().EnterToToast(com, param);

            var arr = this.showingList[formConfig.prefabUrl];
            if (arr == null) {
                arr = this.showingList[formConfig.prefabUrl] = new List<UIToast>();
            }

            arr.Add(com);
            
            return com;
        }

        public async UniTask<bool> Close(UIToast com, [CanBeNull] Object param) {
            await UIManager.GetInstance().ExitToToast(com, param);
            this.pools[com.fid].Release(com);
            var arr = this.showingList[com.fid];
            arr.Remove(com);
            return true;
        }

        public async UniTask<bool> ClearToast(string prefabUrl, [CanBeNull] Object param) {
            var pool = this.pools[prefabUrl];
            if (pool == null) return false;
            var arr = this.showingList[prefabUrl];
            if (arr != null) {
                for (var i = arr.Count - 1; i >= 0; i--) {
                    await UIManager.GetInstance().ExitToToast(arr[i], param);
                    pool.Release(arr[i]);
                }
                arr.Clear();
                this.showingList.Remove(prefabUrl);
            }
            
            pool.Clear();
            this.pools.Remove(prefabUrl);
            return true;
        }

        private async UniTask<ObjectPool<UIToast>> GenPool(string prefabUrl) {
            var prefab = await Resources.LoadAsync(prefabUrl) as GameObject;
            if (prefab == null) return null;
            return new ObjectPool<UIToast>(() => {
                var gameObject = Object.Instantiate(prefab);
                UIManager.GetInstance().AddTransformTree(gameObject.GetComponent<RectTransform>());
                return gameObject.GetComponent<UIToast>();
            });
        }
        
        
    }
}