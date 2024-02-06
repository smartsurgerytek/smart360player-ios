using System.IO;
using UnityEngine;

public interface ICredentialLoader
{
    ICredentialContext Load(ICredentialContext context);
}
public class EasonCredentialLoader : ICredentialLoader
{
    ICredentialContext ICredentialLoader.Load(ICredentialContext rawContext)
    {
        var context = (EasonLoadingCredentialContext)rawContext;
        string json;
        json = File.ReadAllText(Path.Combine(Application.persistentDataPath, context.folderPath, context.fileName));
        context.credential = JsonUtility.FromJson<Credential>(json);

        json = File.ReadAllText(Path.Combine(Application.persistentDataPath, context.folderPath, context.fileName));
        context.cookie = JsonUtility.FromJson<CredentialCookie>(json);
        return context;

    }
}