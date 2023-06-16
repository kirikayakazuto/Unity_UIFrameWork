using FrameWork;
using Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class UISound : UIFixed {

        public Button EffectButton;
        public Button MusicButton;

        public Texture EffectOn;
        public Texture EffectOff;

        public Texture MusicOn;
        public Texture MusicOff;
        
        private void Start() {
            
            this.EffectButton.onClick.AddListener(() => {
                Game.SoundMgr.SwitchEffect();
                this.UpdateUIs();
            });
            
            this.MusicButton.onClick.AddListener(() => {
                Game.SoundMgr.SwitchMusic();
                this.UpdateUIs();
            });
            
            this.UpdateUIs();
            
            var width = UnityEngine.Screen.width;
            var height = UnityEngine.Screen.height;
            var rect = this.rectTransform.rect;
            var x = width / 2f - rect.width / 2f;
            var y = height / 2f - rect.height / 2f;
            this.rectTransform.localPosition = new Vector3(x, y, 0);  
        }

        public void UpdateUIs() {
            var image = this.EffectButton.GetComponent<RawImage>();
            image.texture = Game.SoundMgr.effectState ? this.EffectOn : this.EffectOff;

            image = this.MusicButton.GetComponent<RawImage>();
            image.texture = Game.SoundMgr.musicState ? this.MusicOn : this.MusicOff;
        }
    
        private void Update() {
        
        }
    }
}
