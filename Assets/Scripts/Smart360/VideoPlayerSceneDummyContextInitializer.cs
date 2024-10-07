using Sirenix.Serialization;

public class VideoPlayerSceneDummyContextInitializer : IController
{
    [OdinSerialize] private IArrayRouter<Edition> _loadEditions;
    private void CreateDummyContext()
    {
        _loadEditions.Execute();
    }
    void IController.Execute()
    {
        CreateDummyContext();
    }
}
