using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class CredentialVerifier : IRouter<Credential, VerificationResult>
{

    [OdinSerialize] private ICredentialHasher<ApplicationCredential> _applicationHasher;
    [OdinSerialize] private ICredentialHasher<EditionCredential> _editionHasher;
    [OdinSerialize] private ICredentialHasher<ModuleCredential> _moduleHasher;
    [OdinSerialize] private IReader<Credential> _credential;
    [OdinSerialize] private IWriter<VerificationResult> _result;
    [SerializeField] private CredentialCookie _cookie;

    void IController.Execute()
    {
        _result.Write(Verify(_credential.Read()));
    }

    Credential IReader<Credential>.Read()
    {
        return _credential.Read();
    }

    void IWriter<VerificationResult>.Write(VerificationResult value)
    {
        _result.Write(value);
    }
    VerificationResult Verify(Credential credential)
    {
        var rt = new VerificationResult(credential.modules.Length, credential.editions.Length);
        rt.applicationHashInvalid = credential.application.hash != _applicationHasher.Hash(credential.application);
        rt.applicationExpired = DateTime.Now.Ticks > credential.application.expiredDate;
        rt.applicationUnpaid = !credential.application.purchased;
        rt.deviceInvalid = SystemInfo.deviceUniqueIdentifier != credential.application.deviceUniqueIdentifier;
        rt.lastTimeLoginInvalid = DateTime.Now.Ticks < _cookie.lastTimeLogin;
        for (int i = 0; i < credential.modules.Length; i++)
        {
            rt.moduleIds[i] = credential.modules[i].id;
            rt.moduleHashInvalid[i] = credential.modules[i].hash != _moduleHasher.Hash(credential.modules[i]);
            rt.moduleExpired[i] = DateTime.Now.Ticks >credential.modules[i].expiredDate;
            rt.moduleUnpaid[i] = !credential.modules[i].purchased;
        }
        for (int i = 0; i < credential.editions.Length; i++)
        {
            rt.editionIds[i] = credential.editions[i].id;
            rt.editionHashInvalid[i] = credential.editions[i].hash != _editionHasher.Hash(credential.editions[i]);
            rt.editionExpired[i] = DateTime.Now.Ticks > credential.editions[i].expiredDate;
            rt.editionUnpaid[i] = !credential.editions[i].purchased;
        }
        rt.verified = true;
        return rt;
    }

    void IWriter.Write(object value)
    {
        ((IWriter<VerificationResult>)this).Write((VerificationResult)value);
    }
}