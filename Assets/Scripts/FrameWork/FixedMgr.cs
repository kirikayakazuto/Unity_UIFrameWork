using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork {
    public class FixedMgr {
        public FixedMgr instance { get; } = new FixedMgr();
        
        public async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
            return await UIManager.GetInstance().OpenForm(formConfig, param, formData);
        }

        public async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param, IFormData? formData) {
            return await UIManager.GetInstance().CloseForm(formConfig, param, formData);
        }
    }
}