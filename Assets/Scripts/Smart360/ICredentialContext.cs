using UnityEngine;

public interface ICredentialContext
{
    Credential credential { get; }
    CredentialCookie cookie { get; }
}
public struct EasonLoadingCredentialContext : ICredentialContext
{
    internal string folderPath;
    internal string fileName;
    internal string cookieName;

    internal Credential credential;
    internal CredentialCookie cookie;

    Credential ICredentialContext.credential => this.credential;
    CredentialCookie ICredentialContext.cookie => this.cookie;
}