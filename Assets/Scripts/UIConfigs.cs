using FrameWork.Structure;

public static class UIConfigs {
    
    public static IFormConfig UIHome = new IFormConfig() {prefabUrl = "Forms/Screens/UIHome", type = FormType.Screen};
    
    public static IFormConfig UIAbout = new IFormConfig() {prefabUrl = "Forms/Screens/UIAbout", type = FormType.Screen};
    
    public static IFormConfig UILoading = new IFormConfig() {prefabUrl = "Forms/Screens/UILoading", type = FormType.Tips};
}