using Sirenix.OdinInspector;
using UnityEngine;

public class MasterContext : SerializedMonoBehaviour
{
    [SerializeField] private IEditionButtonContext _editionButton;
    [SerializeField] private ICredentialContext _credential;
    [SerializeField] private IModuleContext _module;
    [SerializeField] private IEditionContext _edition;

    public ICredentialContext credential { get => _credential; }
    public IEditionButtonContext editionButton { get => _editionButton; }
    public IModuleContext module { get => _module; }
    internal IEditionContext edition { get => _edition; }

    public void Initialize()
    {
        _credential.Initialize();
        _editionButton.Initialize();
    }

}
