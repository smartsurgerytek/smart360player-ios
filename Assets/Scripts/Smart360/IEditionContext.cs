internal interface IEditionContext
{
    void Initialize();
    int[] GetCurrentEditions();
    int GetCount(int module);
    string GetName(int edition);
}
internal interface IVerificationContext
{
    VerificationResult result { get; internal set; }
}
