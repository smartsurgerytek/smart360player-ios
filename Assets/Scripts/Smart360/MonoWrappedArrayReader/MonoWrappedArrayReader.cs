public class MonoWrappedArrayReader<T> : MonoWrappedReader<T[]>, IArrayReader<T>
{
    int ICountProvider.count => innerData.Read().Length;
}
