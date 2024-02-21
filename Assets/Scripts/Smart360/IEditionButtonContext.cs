public interface IEditionButtonContext
{
    int count { get; }

    bool GetEnabled(int i);
    void Initialize();
    void IsUnpaid(int i);
}

