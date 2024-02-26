using Sirenix.Serialization;
public class CredentialExpiredDateUpdator : IController
{
    [OdinSerialize] private IReader<long> _expiredDate;
    [OdinSerialize] private IAccessor<Credential> _credential;
    void IController.Execute()
    {
        var expiredDate = _expiredDate?.Read() ?? 0;
        var credential = _credential.Read();
        credential.FillExpiredDate(expiredDate);
        _credential.Write(credential);
    }
}
public class CredentialDuidUpdater : IController
{
    [OdinSerialize] private IReader<string> _duid;
    [OdinSerialize] private IAccessor<Credential> _credential;

    void IController.Execute()
    {
        var duid = _duid?.Read() ?? "";
        var credential =  _credential.Read();
        credential.FillDuid(duid);
        _credential.Write(credential);
    }
}

public class CredentialHashUpdater : IController
{
    [OdinSerialize] IAccessor<Credential> _credential;  
    [OdinSerialize] ICredentialHasher<ApplicationCredential> _application;
    [OdinSerialize] ICredentialHasher<EditionCredential> _edition;
    [OdinSerialize] ICredentialHasher<ModuleCredential> _module;

    void IController.Execute()
    {
        var credential = _credential.Read();
        credential.SetApplicationHash(_application.Hash(credential.application));
        for (int i = 0; i < credential.modules.Length; i++)
        {
            credential.SetModuleHash(i, _module.Hash(credential.modules[i]));
        }
        for (int i = 0; i < credential.editions.Length; i++)
        {
            credential.SetEditionHash(i, _edition.Hash(credential.editions[i]));
        }


        _credential.Write(credential);
    }
}