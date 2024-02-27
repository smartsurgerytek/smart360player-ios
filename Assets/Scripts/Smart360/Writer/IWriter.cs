public interface IWriter<T> : IWriter
{
    void Write(T value);
}
public interface IWriter
{
    void Write(object value);
}