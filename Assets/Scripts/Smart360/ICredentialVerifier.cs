public interface ICredentialVerifier
{
    VerificationResult Verify(Credential credential);
}