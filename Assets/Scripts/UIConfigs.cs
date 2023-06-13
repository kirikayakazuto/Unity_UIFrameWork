using FrameWork.Structure;

public static class UIConfigs {

    public static IFormConfig UIBackButton = new IFormConfig() {prefabUrl = "Forms/Fixeds/UIBackButton", type = FormType.Fixed};
    
    public static IFormConfig UIHome = new IFormConfig() {prefabUrl = "Forms/Screens/UIHome", type = FormType.Screen};
    
    public static IFormConfig UIAbout = new IFormConfig() {prefabUrl = "Forms/Screens/UIAbout", type = FormType.Screen};
    
    public static IFormConfig UILoading = new IFormConfig() {prefabUrl = "Forms/Screens/UILoading", type = FormType.Tips};
    
    public static IFormConfig UIMap = new IFormConfig() {prefabUrl = "Forms/Screens/UIMap", type = FormType.Screen};
    
    public static IFormConfig UIUpgrade = new IFormConfig() {prefabUrl = "Forms/Windows/UIUpgrade", type = FormType.Window};
    
    public static IFormConfig UICompleted = new IFormConfig() {prefabUrl = "Forms/Windows/UICompleted", type = FormType.Window};
}