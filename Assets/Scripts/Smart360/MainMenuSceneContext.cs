using UnityEngine;

public class MainMenuSceneContext : MonoBehaviour, IMainMenuSceneContext
{
    [SerializeField] private IAccessor<int> _currentModule;
    [SerializeField] private IReader<IEdition[]> _editions;
    int IMainMenuSceneContext.currentModule { get => _currentModule.Read();}
    IEdition[] IMainMenuSceneContext.editions => _editions.Read();
}