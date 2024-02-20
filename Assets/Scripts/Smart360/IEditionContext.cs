internal interface IEditionContext
{
    void Initialize();
    int GetCount(int module);
    string GetName(int edition);
}
internal interface IVerificationContext
{
    VerificationResult result { get; internal set; }
}
