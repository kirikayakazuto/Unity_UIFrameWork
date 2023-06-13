using System;
using Cysharp.Threading.Tasks;
using FrameWork.Structure;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FrameWork {
    public class FormMgr {
        public static async UniTask<UIBase> Open(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
            return formConfig.type switch {
                FormType.Screen => await SceneMgr.instance.Open(formConfig, param, formData),
                FormType.Fixed => await FixedMgr.instance.Open(formConfig, param, formData),
                FormType.Window => await WindowMgr.instance.Open(formConfig, param, formData),
                FormType.Toast => await ToastMgr.instance.Open(formConfig, param, formData),
                FormType.Tips => await TipsMgr.instance.Open(formConfig, param, formData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static async UniTask<bool> Close(IFormConfig formConfig, [CanBeNull] Object param = null, IFormData formData = new IFormData()) {
            return formConfig.type switch {
                FormType.Screen => await SceneMgr.instance.Close(formConfig, param, formData),
                FormType.Fixed => await FixedMgr.instance.Close(formConfig, param, formData),
                FormType.Window => await WindowMgr.instance.Close(formConfig, param, formData),
                FormType.Tips => await TipsMgr.instance.Close(formConfig, param, formData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static async UniTask<bool> BackScene([CanBeNull] Object param = null, IFormData formData = new IFormData()) {
            return await SceneMgr.instance.Back(param, formData);
        }

        public static async UniTask<bool> CloseAllWindows() {
            return await WindowMgr.instance.CloseAll();
        }
    }
}