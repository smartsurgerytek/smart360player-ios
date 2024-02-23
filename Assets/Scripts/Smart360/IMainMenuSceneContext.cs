internal interface IMainMenuSceneContext
{
    int currentModule { get; }
    IEdition[] editions { get; }
}