namespace Logic.Managers {
    public class SoundMgr {
        public bool effectState = true;
        public bool musicState = true;

        public void SwitchEffect() { 
            this.effectState = !this.effectState;
        }

        public void SwitchMusic() {
            this.musicState = !this.musicState;
        }
        
        
    }
}