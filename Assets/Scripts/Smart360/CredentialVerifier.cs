using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class CredentialVerifier : ICredentialVerifier
{

    [OdinSerialize] private ICredentialHasher<ApplicationCredential> _applicationHasher;
    [OdinSerialize] private ICredentialHasher<EditionCredential> _editionHasher;
    [OdinSerialize] private ICredentialHasher<ModuleCredential> _moduleHasher;
    [SerializeField] private CredentialCookie _cookie;

    VerificationResult ICredentialVerifier.Verify(Credential credential)
    {
        var rt = new VerificationResult(credential.modules.Length, credential.editions.Length);
        rt.applicationHashInvalid = credential.application.hash != _applicationHasher.Hash(credential.application);
        rt.applicationExpired = DateTime.Now.Ticks > credential.application.expiredDate;
        rt.applicationUnpaid = !credential.application.purchased;
        rt.deviceInvalid = SystemInfo.deviceUniqueIdentifier != credential.application.deviceUniqueIdentifier;
        rt.lastTimeLoginInvalid = DateTime.Now.Ticks < _cookie.lastTimeLogin;
        for (int i = 0; i < credential.modules.Length; i++)
        {
            rt.moduleHashInvalid[i] = credential.modules[i].hash != _moduleHasher.Hash(credential.modules[i]);
            rt.moduleExpired[i] = DateTime.Now.Ticks >credential.editions[i].expiredDate;
            rt.moduleUnpaid[i] = !credential.modules[i].purchased;
        }
        for (int i = 0; i < credential.editions.Length; i++)
        {
            rt.editionHashInvalid[i] = credential.editions[i].hash != _editionHasher.Hash(credential.editions[i]);
            rt.editionExpired[i] = DateTime.Now.Ticks > credential.editions[i].expiredDate;
            rt.editionUnpaid[i] = !credential.editions[i].purchased;
        }
        rt.verified = true;
        return rt;
    }
}