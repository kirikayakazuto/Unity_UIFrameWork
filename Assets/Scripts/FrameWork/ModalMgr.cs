using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FrameWork.Structure;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork {
    public class ModalMgr {
        public static ModalMgr instance { get; } = new ModalMgr();
        
        private readonly Image _image;

        public ModalMgr() {
            this._image = this.GenSingleColorImage();
            var ndWindow = UIManager.GetInstance().NdWindow;
            this._image.rectTransform.SetParent(ndWindow);
            this._image.color = new Color(0, 0, 0, 0);
            this._image.rectTransform.localPosition = new Vector3(0, 0, 0);
            this._image.rectTransform.sizeDelta = new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);
            this._image.gameObject.SetActive(false);
        }
        
        private Image GenSingleColorImage() {
            var goImage = new GameObject("ModalImage");
            var button = goImage.AddComponent<Button>();
            button.onClick.AddListener(() => {
                WindowMgr.instance.CloseTop(null).Forget();
            });
            goImage.AddComponent<RectTransform>();
            return goImage.AddComponent<Image>();
        }

        private async UniTask<bool> ShowModal(ModalType modalType) {
            if (modalType.opacity == ModalOpacity.None) return false;
            var o = modalType.opacity switch {
                ModalOpacity.OpacityZero => 0,
                ModalOpacity.OpacityLow => 63 / 255f,
                ModalOpacity.OpacityHalf => 0.5f,
                ModalOpacity.OpacityHigh => 189 / 255f,
                ModalOpacity.OpacityFull => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };
            this._image.gameObject.SetActive(true);
            var color = new Color(0, 0, 0, o);
            if (modalType.useEase) {
                await this._image.DOColor(color, 0.2f);
            }
            else this._image.color = color;
            return true;
        }

        public void CheckModalWindow(IFormConfig[] formConfigs) {

            if (formConfigs.Length <= 0) {
                this._image.gameObject.SetActive(false);
                return;;
            }
            
            var button = this._image.GetComponent<Button>();
            
            for (var i = formConfigs.Length - 1; i >= 0; i--) {
                var com = UIManager.GetInstance().GetForm(formConfigs[i].prefabUrl) as UIWindow;
                if(com == null) continue;
                if (com.modalType.opacity <= 0) continue;
                this._image.rectTransform.SetSiblingIndex(i);
                this.ShowModal(com.modalType).Forget();
                break;
            }
        }


    }
}