using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using FrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    private void OnEnable() {
        
    }
    
    private void Start() {
        FormMgr.Open(UIConfigs.UIHome).Forget();
        
        // var bundle = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundles/abtest3");
        // var assets = bundle.LoadAllAssets();
        // foreach (var asset in assets) {
        //     Debug.Log(asset.name);
        // }

    }
    
    private void Update() {
        
    }
}
