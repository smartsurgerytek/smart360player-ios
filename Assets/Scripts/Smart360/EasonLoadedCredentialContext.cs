using Sirenix.Serialization;

public class SceneLoader : IController
{
    [OdinSerialize] private IReader<string> _sceneToLoad;
    void IController.Execute()
    {
        var sceneToLoad = _sceneToLoad.Read();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}