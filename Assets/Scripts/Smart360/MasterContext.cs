using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
public class MasterContext : SerializedMonoBehaviour
{
    [SerializeField] private ICredentialContext _credential;
    [SerializeField] private IApplicationContext _application;
    [SerializeField] private IMainMenuSceneContext _mainMenuScene;
    [SerializeField] private IModuleModel _module;
    [SerializeField] private IEditionModel _edition;
    [SerializeField] private IVerificationContext _verification;
    public ICredentialContext credential { get => _credential; }
    internal IApplicationContext application { get => _application; }
    public IModuleModel module { get => _module; }
    internal IEditionModel edition { get => _edition; }
    internal IVerificationContext verification { get => _verification; }
    internal IMainMenuSceneContext mainMenuScene { get => _mainMenuScene; }

    [OdinSerialize] private ILoader<IEditionModel> _editionLoader;
    public void Load(Manifest manifest)
    {
        //_credential.Load(manifest.credentialPath);
        //_edition.Load(manifest.editionPath);
    }
    public void Initialize()
    {
        credential.Initialize();
        //edition?.Initialize();
    }
}
