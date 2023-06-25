using System.IO;
using FrameWork;
using UnityEditor;

namespace Editor {
    public static class CreateAssetBundles {
        
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles() {
            var assetBundleDirectory = SysDefine.AssetBundlePath;
            if(!Directory.Exists(assetBundleDirectory)) {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        }
    }
}