using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace FrameWork {
    public class ToastMgr {
        public static ToastMgr instance { get; } = new ToastMgr();
        
        private readonly Dictionary<string, ObjectPool<UIToast>> pools = new Dictionary<string, ObjectPool<UIToast>>();
        private readonly Dictionary<string, List<UIToast>> showingList = new Dictionary<string, List<UIToast>>();

        public async UniTask<UIToast> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData formData = new IFormData()) {
            if (!this.pools.TryGetValue(formConfig.prefabUrl, out var pool)) {
                pool = await this.GenPool(formConfig.prefabUrl);
                this.pools.Add(formConfig.prefabUrl, pool);
            }

            var com = pool.Get();
            com.fid = formConfig.prefabUrl;
            await UIManager.GetInstance().EnterToToast(com, param);

            if (!this.showingList.TryGetValue(formConfig.prefabUrl, out var arr)) {
                arr = new List<UIToast>();
                this.showingList.Add(formConfig.prefabUrl, arr);
            }

            arr.Add(com);
            
            return com;
        }

        public async UniTask<bool> Close(UIToast com, [CanBeNull] Object param = null) {
            if (!this.pools.TryGetValue(com.fid, out var pool)) return false;
            pool.Release(com);
            await UIManager.GetInstance().ExitToToast(com, param);
            return this.showingList.TryGetValue(com.fid, out var arr) && arr.Remove(com);
        }

        public async UniTask<bool> ClearToast(string prefabUrl, [CanBeNull] Object param) {
            if (!this.pools.TryGetValue(prefabUrl, out var pool)) return false;
            if (this.showingList.TryGetValue(prefabUrl, out var arr)) {
                for (var i = arr.Count - 1; i >= 0; i--) {
                    await UIManager.GetInstance().ExitToToast(arr[i], param);
                    pool.Release(arr[i]);
                }
                arr.Clear();
                if(!this.showingList.Remove(prefabUrl)) {
                    
                }
            }
            pool.Clear();
            return this.pools.Remove(prefabUrl);
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