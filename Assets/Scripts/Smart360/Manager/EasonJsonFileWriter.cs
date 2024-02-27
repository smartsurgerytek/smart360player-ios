
public interface IArrayAccessor<T> : IAccessor<T[]>, IArrayReader<T>, IArrayWriter<T>
{
}
public interface IArrayWriter<T> : IWriter<T[]>, IArrayWriter
{

}
public interface IArrayReader<T> : IReader<T[]>, ICountProvider
{
    
}
public interface IArrayWriter : IWriter
{

}