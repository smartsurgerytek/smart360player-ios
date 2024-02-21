public interface ICredentialContext
{
    void Initialize();
    bool IsUnpaid(int edition);
    bool isExpired(int edition);

    Credential credential { get; }
    CredentialCookie cookie { get; }
}
