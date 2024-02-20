using UnityEngine;

public class MainMenuSceneContext : MonoBehaviour, IMainMenuSceneContext
{
    [SerializeField] private int _currentModule;
    [SerializeField] private EditionManager _editionManager;
    int IMainMenuSceneContext.currentModule { get => _currentModule; }
    int[] IMainMenuSceneContext.currentEditionIds { get => _editionManager.GetEditionsOfModule(_currentModule); }
}
