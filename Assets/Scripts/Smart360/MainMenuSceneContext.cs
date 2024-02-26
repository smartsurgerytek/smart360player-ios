using UnityEngine;

public class MainMenuSceneContext : MonoBehaviour, IMainMenuSceneContext
{
    [SerializeField] private int _currentModule;
    [SerializeField] private IReader<IEdition[]> _editions;
    int IMainMenuSceneContext.currentModule { get => _currentModule; }
    IReader<IEdition[]> IMainMenuSceneContext.editions => _editions;
}