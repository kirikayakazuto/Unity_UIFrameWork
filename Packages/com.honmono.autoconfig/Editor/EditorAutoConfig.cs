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
    }
    
    public struct INodeConfig {
        public string name;
        public string prefabUrl;
    }
    public static class AutoConfig {

        private const string FormConfigPath = "/Scripts/Logic/Managers/FrameWork/Form.cs";
        private const string NodeConfigPath = "/Scripts/Logic/Managers/FrameWork/Node.cs";
        
        // 生成auto config文件
        [MenuItem("Tools/AutoConfig")]
        public static void GenAutoConfig() {
            var nodeConfigs = new Dictionary<string, INodeConfig>();
            var formConfigs = new Dictionary<string, IFormConfig>();

            var paths = AutoConfig.GetAllPrefabRelativePath();
            
            var screenBlock = new Regex(": UIScreen");
            var fixedBlock = new Regex(": UIFixed");
            var windowBlock = new Regex(": UIWindow");
            var tipsBlock = new Regex(": UITips");
            var toastBlock = new Regex(": UIToast");

            var normalBlock = new Regex(": MonoBehaviour");
            
            foreach (var path in paths) {
                // asset name
                var assetName = Path.GetFileNameWithoutExtension(path);
                
                // asset bundle name
                var prefabText = File.ReadAllText(path);
                
                var assetPath = path;

                var guidMatch = Regex.Matches(prefabText, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type: [0-9]}");
                var extendsForm = false;
                
                var formName = Path.GetFileNameWithoutExtension(path);
                
                foreach (Match o in guidMatch) {
                    var guid = o.Groups["GuidValue"].Value;
                    var scriptPath = AssetDatabase.GUIDToAssetPath(guid);
                    if(!File.Exists(scriptPath)) continue;

                    var scriptText = File.ReadAllText(scriptPath);
                    
                    if (screenBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Screen"});
                        extendsForm = true;
                    }else if (fixedBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Fixed"});
                        extendsForm = true;
                    }else if (windowBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Window"});
                        extendsForm = true;
                    }else if (tipsBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Tips"});
                        extendsForm = true;
                    }else if (toastBlock.Match(scriptText).Success) {
                        formConfigs.Add(formName, new IFormConfig() {name = formName, prefabUrl = assetPath, type = "Toast"});
                        extendsForm = true;
                    }
                }
                if (!extendsForm) {
                    nodeConfigs.Add(formName, new INodeConfig() {name = formName, prefabUrl = assetPath});
                }
            }

            var formConfigScript = @"using FrameWork.Structure;
public static class Form {
";
            foreach (var keyValuePair in formConfigs) {
                formConfigScript += $"\t\n    public static IFormConfig {keyValuePair.Key} = new IFormConfig() {{ " +
                                $"\t\n        name = \"{keyValuePair.Value.name}\", " +
                                $"\t\n        prefabUrl = \"{keyValuePair.Value.prefabUrl}\", " +
                                $"\t\n        type = FormType.{keyValuePair.Value.type}, " +
                                $"\t\n    }};\t\n";
            }
            formConfigScript += "\t\n}";

            var nodeConfigScript = @"using FrameWork.Structure;
public static class Node {
";
            
            foreach (var keyValuePair in nodeConfigs) {
                nodeConfigScript += $"\t\n    public static INodeConfig {keyValuePair.Key} = new INodeConfig() {{ " +
                                    $"\t\n        name = \"{keyValuePair.Value.name}\", " +
                                    $"\t\n        prefabUrl = \"{keyValuePair.Value.prefabUrl}\", " +
                                    $"\t\n    }};\t\n";
            }
            nodeConfigScript += "\t\n}";
            
            File.WriteAllText(Application.dataPath + FormConfigPath, formConfigScript);
            File.WriteAllText(Application.dataPath + NodeConfigPath, nodeConfigScript);
            
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