using FrameWork.Structure;
public static class UIConfigs {
	
    public static IFormConfig UITest = new IFormConfig() { 	
        name = "UITest", 	
        prefabUrl = "UITest", 	
        type = FormType.Screen, 	
        assetbundleUrl = "abtest3.assetbundle" 	
    };	
	
    public static IFormConfig UIBackButton = new IFormConfig() { 	
        name = "UIBackButton", 	
        prefabUrl = "Forms/Fixeds/UIBackButton", 	
        type = FormType.Fixed, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIRole = new IFormConfig() { 	
        name = "UIRole", 	
        prefabUrl = "Forms/Fixeds/UIRole", 	
        type = FormType.Fixed, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UISkills = new IFormConfig() { 	
        name = "UISkills", 	
        prefabUrl = "Forms/Fixeds/UISkills", 	
        type = FormType.Fixed, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UISound = new IFormConfig() { 	
        name = "UISound", 	
        prefabUrl = "Forms/Fixeds/UISound", 	
        type = FormType.Fixed, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIAbout = new IFormConfig() { 	
        name = "UIAbout", 	
        prefabUrl = "Forms/Screens/UIAbout", 	
        type = FormType.Screen, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIHome = new IFormConfig() { 	
        name = "UIHome", 	
        prefabUrl = "Forms/Screens/UIHome", 	
        type = FormType.Screen, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UILevel = new IFormConfig() { 	
        name = "UILevel", 	
        prefabUrl = "Forms/Screens/UILevel", 	
        type = FormType.Screen, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UILoading = new IFormConfig() { 	
        name = "UILoading", 	
        prefabUrl = "Forms/Screens/UILoading", 	
        type = FormType.Tips, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIMap = new IFormConfig() { 	
        name = "UIMap", 	
        prefabUrl = "Forms/Screens/UIMap", 	
        type = FormType.Screen, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIToastText = new IFormConfig() { 	
        name = "UIToastText", 	
        prefabUrl = "Forms/UITips/UIToastText", 	
        type = FormType.Toast, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UICompleted = new IFormConfig() { 	
        name = "UICompleted", 	
        prefabUrl = "Forms/Windows/UICompleted", 	
        type = FormType.Window, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIPass = new IFormConfig() { 	
        name = "UIPass", 	
        prefabUrl = "Forms/Windows/UIPass", 	
        type = FormType.Window, 	
        assetbundleUrl = "Resources" 	
    };	
	
    public static IFormConfig UIUpgrade = new IFormConfig() { 	
        name = "UIUpgrade", 	
        prefabUrl = "Forms/Windows/UIUpgrade", 	
        type = FormType.Window, 	
        assetbundleUrl = "Resources" 	
    };	
	
}