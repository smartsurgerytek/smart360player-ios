using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class MasterController : SerializedMonoBehaviour
{
    //[SerializeField] ICredentialVerifier _credentialVerifier;
    //[SerializeField] IVerificationResultReceiver _verificationResultReceiver;
    //[OdinSerialize] ISpawnInitializer<EditionButton> _editionButtonPreinitializer;
    [OdinSerialize] private IController _initializer;
    //public ISpawnInitializer<EditionButton> editionButtonPreinitializer { get => _editionButtonPreinitializer; }
    //public ICredentialVerifier credentialVerifier { get => _credentialVerifier; }
    internal void Initialize(MasterContext context)
    {
        _initializer.Execute();
    }
    internal void InternalUpdate(MasterContext context, MasterView view)
    {
    }
}
