// -----------------------------------------------------------------------------
//
// Use this editor example C# file to develop editor (non-runtime) code.
//
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Honmono.Autoconfig.Editor {

    public struct IFormConfig {
        public string name;
        public string prefabUrl;
        public string type;
        public string assetbundle;
    }
    public static class AutoConfig {

        private const string UIConfigPath = "/Scripts/Logic/Managers/FrameWork/UIConfigs.cs";
        
        // 生成auto config文件
        [MenuItem("Tools/AutoConfig")]
        public static void GenAutoConfig() {
            var formConfigs = new Dictionary<string, IFormConfig>();

            var paths = AutoConfig.GetAllPrefabRelativePath();
            
            var screenBlock = new Regex(": UIScreen");
            var fixedBlock = new Regex(": UIFixed");
            var windowBlock = new Regex(": UIWindow");
            var tipsBlock = new Regex(": UITips");
            var toastBlock = new Regex(": UIToast");
            
            foreach (var path in paths) {
                // asset name
                var assetName = Path.GetFileNameWithoutExtension(path);
                
                // asset bundle name
                var prefabText = File.ReadAllText(path);
                var assetBundleName = AssetDatabase.GetImplicitAssetBundleName(path) + "." + AssetDatabase.GetImplicitAssetBundleVariantName(path);
                if (assetBundleName.Equals(".")) assetBundleName = "Resources";

                var assetPath = path;
                // if (assetBundleName.Equals("Resources")) {
                //     var match = Regex.Match(path, "Assets/Resources/(?<PathVale>.*?).prefab");
                //     if (match.Success) assetPath = match.Groups["PathVale"].Value;    
                // } else {
                //     var assets = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                //     foreach (var asset in assets) {
                //         if (asset.Contains(assetName)) {
                //             assetPath = assetName;
                //         }
                //     }
                // }
                
                var guidMatch = Regex.Matches(prefabText, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type: [0-9]}");
                foreach (Match o in guidMatch) {
                    var guid = o.Groups["GuidValue"].Value;
                    var scriptPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(!File.Exists(scriptPath)) continue;

                    var scriptText = File.ReadAllText(scriptPath);
                    var formName = Path.GetFileNameWithoutExtension(scriptPath);
                    
                    if (screenBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Screen"});
                    }else if (fixedBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Fixed"});
                    }else if (windowBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Window"});
                    }else if (tipsBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Tips"});
                    }else if (toastBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Toast"});
                    }
                }
            }

            var configScript = @"using FrameWork.Structure;
public static class UIConfigs {
";
            foreach (var keyValuePair in formConfigs) {
                configScript += $"\t\n    public static IFormConfig {keyValuePair.Key} = new IFormConfig() {{ " +
                                $"\t\n        name = \"{keyValuePair.Value.name}\", " +
                                $"\t\n        prefabUrl = \"{keyValuePair.Value.prefabUrl}\", " +
                                $"\t\n        type = FormType.{keyValuePair.Value.type}, " +
                                $"\t\n    }};\t\n";
            }
            configScript += "\t\n}";
            
            File.WriteAllText(Application.dataPath + UIConfigPath, configScript);
            AssetDatabase.Refresh();
            
            Debug.Log("AutoConfig run success");
        }
 
        
        private static List<string> GetAllPrefabRelativePath() {
            var list = new List<string>();
            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            return list;
        }
        
    }
}