using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class MonoWrapper<T> : SerializedMonoBehaviour
{
    [OdinSerialize] private T _innerData;

    public T innerData { get => _innerData; set => _innerData = value; }
}


public class MainMenuLoadSceneController : ILoadSceneController
{
    [OdinSerialize] private IReader<int> _moduleToLoad;
    [OdinSerialize] private IReader<MainMenuSceneManager> _mainMenuSceneManager;
    [OdinSerialize] private ICachedWriter<VerificationView> _verificationView;
    [OdinSerialize] private IWriter<int> _currentModule;
    void ILoadSceneController.LoadScene(ILoadSceneModel rawModel)
    {
        if (!(rawModel is MainMenuLoadSceneModel)) return;
        var moduleId = _moduleToLoad.Read();
        var manager = _mainMenuSceneManager.Read();
        if (manager)
        {
            _verificationView.Write(manager.verificationView);
        }
    }
}

