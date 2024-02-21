internal interface IProvider<T>
{
    T Get();
}
internal interface IReceiver<T>
{
    void Set(T value);
}