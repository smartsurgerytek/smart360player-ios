internal interface IMainMenuSceneContext
{
    int currentModule { get; }
    IReader<IEdition[]> editions { get; }
}