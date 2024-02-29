using Sirenix.OdinInspector;
using UnityEngine;
public class MasterContext : SerializedMonoBehaviour
{
    [SerializeField] private ICredentialContext _credential;
    [SerializeField] private IApplicationContext _application;
    [SerializeField] private IMainMenuSceneContext _mainMenuScene;
    [SerializeField] private IModuleModel _module;
    [SerializeField] private IAccessor<Edition[]> _editions;
    //[SerializeField] private IVerificationContext _verification;
    public ICredentialContext credential { get => _credential; }
    internal IApplicationContext application { get => _application; }
    public IModuleModel module { get => _module; }
    internal IAccessor<Edition[]> edition { get => _editions; }
    //internal IVerificationContext verification { get => _verification; }
    internal IMainMenuSceneContext mainMenuScene { get => _mainMenuScene; }

    //internal void SetVerificationResult(VerificationResult result)
    //{
    //    _verification.result = result;
    //}
    public void Load(Manifest manifest)
    {
        //_credential.Load(manifest.credentialPath);
        //_edition.Load(manifest.editionPath);
    }
    public void Initialize()
    {
        //credential.Initialize();
        //edition?.Initialize();
    }
}
