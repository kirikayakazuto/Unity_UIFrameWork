using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;

public class Scene : MonoBehaviour {
    
    void Start() {
        
    }

    public async void OnGameInit() {
        await Game.Init();
    }

    void Update() {
        Game.Update();
    }
}
