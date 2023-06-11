using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    private void OnEnable() {
        
    }
    
    private void Start() {
        var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        Debug.Log(gameObjects.Length);
        for (var i = 0; i < gameObjects.Length; i++) {
            Debug.Log(gameObjects[i].name);
        }

        UIManager.GetInstance();
    }
    
    void Update() {
        
    }
}
