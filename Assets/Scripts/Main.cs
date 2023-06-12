using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    private void OnEnable() {
        
    }
    
    private void Start() {
        FormMgr.Open(UIConfigs.UIHome, null, null).Forget();
    }
    
    void Update() {
        
    }
}
