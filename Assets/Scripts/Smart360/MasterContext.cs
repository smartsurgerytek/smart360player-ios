using Sirenix.OdinInspector;
using UnityEngine;

public class MasterContext : SerializedMonoBehaviour
{
    //[SerializeField] private IEditionButtonContext _editionButton;
    [SerializeField] private ICredentialContext _credential;
    [SerializeField] private IApplicationContext _application;
    [SerializeField] private IMainMenuSceneContext _mainMenuScene;
    [SerializeField] private IModuleContext _module;
    [SerializeField] private IEditionContext _edition;
    [SerializeField] private IVerificationContext _verification;
    public ICredentialContext credential { get => _credential; }
    //public IEditionButtonContext editionButton { get => _editionButton; }
    internal IApplicationContext application { get => _application; }
    public IModuleContext module { get => _module; }
    internal IEditionContext edition { get => _edition; }
    internal IVerificationContext verification { get => _verification; }
    internal IMainMenuSceneContext mainMenuScene { get => _mainMenuScene; }

    public void Load(Manifest manifest)
    {
        //_credential.Load(manifest.credentialPath);
        //_edition.Load(manifest.editionPath);
    }
    public void Initialize()
    {
        credential.Initialize();
        //module?.Initialize();
        edition?.Initialize();
        //_editionButton.Initialize();
        //_edition.Initialize(this);
    }

}
