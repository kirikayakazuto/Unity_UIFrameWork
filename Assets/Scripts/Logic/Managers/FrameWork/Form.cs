using FrameWork.Structure;
public static class Form {
	
    public static IFormConfig UIBackButton = new IFormConfig() { 	
        name = "UIBackButton", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Fixeds/UIBackButton.prefab", 	
        type = FormType.Fixed, 	
    };	
	
    public static IFormConfig UIRole = new IFormConfig() { 	
        name = "UIRole", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Fixeds/UIRole.prefab", 	
        type = FormType.Fixed, 	
    };	
	
    public static IFormConfig UISkills = new IFormConfig() { 	
        name = "UISkills", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Fixeds/UISkills.prefab", 	
        type = FormType.Fixed, 	
    };	
	
    public static IFormConfig UISound = new IFormConfig() { 	
        name = "UISound", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Fixeds/UISound.prefab", 	
        type = FormType.Fixed, 	
    };	
	
    public static IFormConfig UIAbout = new IFormConfig() { 	
        name = "UIAbout", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Screens/UIAbout.prefab", 	
        type = FormType.Screen, 	
    };	
	
    public static IFormConfig UIHome = new IFormConfig() { 	
        name = "UIHome", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Screens/UIHome.prefab", 	
        type = FormType.Screen, 	
    };	
	
    public static IFormConfig UILevel = new IFormConfig() { 	
        name = "UILevel", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Screens/UILevel.prefab", 	
        type = FormType.Screen, 	
    };	
	
    public static IFormConfig UILoading = new IFormConfig() { 	
        name = "UILoading", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Screens/UILoading.prefab", 	
        type = FormType.Tips, 	
    };	
	
    public static IFormConfig UIMap = new IFormConfig() { 	
        name = "UIMap", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Screens/UIMap.prefab", 	
        type = FormType.Screen, 	
    };	
	
    public static IFormConfig UIToastText = new IFormConfig() { 	
        name = "UIToastText", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/UITips/UIToastText.prefab", 	
        type = FormType.Toast, 	
    };	
	
    public static IFormConfig UICompleted = new IFormConfig() { 	
        name = "UICompleted", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Windows/UICompleted.prefab", 	
        type = FormType.Window, 	
    };	
	
    public static IFormConfig UIPass = new IFormConfig() { 	
        name = "UIPass", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Windows/UIPass.prefab", 	
        type = FormType.Window, 	
    };	
	
    public static IFormConfig UIUpgrade = new IFormConfig() { 	
        name = "UIUpgrade", 	
        prefabUrl = "Assets/Artwork/Prefabs/Forms/Windows/UIUpgrade.prefab", 	
        type = FormType.Window, 	
    };	
	
}