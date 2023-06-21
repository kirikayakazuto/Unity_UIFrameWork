// -----------------------------------------------------------------------------
//
// Use this editor example C# file to develop editor (non-runtime) code.
//
// -----------------------------------------------------------------------------

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
        
        // 生成auto config文件
        [MenuItem("Tools/AutoConfig")]
        public static void GenAutoConfig() {
            // 1. 创建UIConfigs文件
            Debug.Log(Application.dataPath);
            // 
            
            // 1 获取所有ab包
            var abNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var abName in abNames) {
                Debug.Log(abName);
            }

            var formConfig = new Dictionary<string, IFormConfig>();

            var paths = AutoConfig.GetAllPrefabRelativePath();
            
            var screenBlock = new Regex(": UIScreen");
            var fixedBlock = new Regex(": UIFixed");
            var windowBlock = new Regex(": UIWindow");
            var tipsBlock = new Regex(": UITips");
            var toastBlock = new Regex(": UIToast");
            
            foreach (var path in paths) {
                Debug.Log(path);
                var prefabText = File.ReadAllText(path);
                
                var guidMatch = Regex.Matches(prefabText, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type: [0-9]}");
                foreach (Match o in guidMatch) {
                    var guid = o.Groups["GuidValue"].Value;
                    var scriptPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(!File.Exists(scriptPath)) continue;

                    var scriptText = File.ReadAllText(scriptPath);
                    var formName = Path.GetFileNameWithoutExtension(scriptPath);
                    // path.Split("/")
                    if (screenBlock.Match(scriptText).Success) {
                        formConfig.Add(formName, new IFormConfig(){name = formName, prefabUrl = path, type = "Screen", assetbundle = "resource"});
                    }else if (fixedBlock.Match(scriptText).Success) {
                        formConfig.Add(formName, new IFormConfig(){name = formName, prefabUrl = path, type = "Fixed", assetbundle = "resource"});
                    }else if (windowBlock.Match(scriptText).Success) {
                        formConfig.Add(formName, new IFormConfig(){name = formName, prefabUrl = path, type = "Window", assetbundle = "resource"});
                    }else if (tipsBlock.Match(scriptText).Success) {
                        formConfig.Add(formName, new IFormConfig(){name = formName, prefabUrl = path, type = "Tips", assetbundle = "resource"});
                    }else if (toastBlock.Match(scriptText).Success) {
                        formConfig.Add(formName, new IFormConfig(){name = formName, prefabUrl = path, type = "Toast", assetbundle = "resource"});
                    }
                }
            }

            var configScript = @"using FrameWork.Structure;
public static class UIConfigs {";
            foreach (var keyValuePair in formConfig) {
                configScript += $"\t\n\t\npublic static IFormConfig {keyValuePair.Key} = new IFormConfig() {{prefabUrl = \"{keyValuePair.Value.prefabUrl}\", type = FormType.{keyValuePair.Value.type}}};";
            }
            configScript += "\t\n}";
            
            File.WriteAllText(Application.dataPath + "/Scripts/UIConfigs.cs", configScript);
            AssetDatabase.Refresh();
        }
 
        
        private static List<string> GetAllPrefabRelativePath() {
            var list = new List<string>();
            var guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            return list;
        }
        
    }
}