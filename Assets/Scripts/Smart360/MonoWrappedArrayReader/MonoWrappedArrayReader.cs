public class MonoWrappedArrayReader<T> : MonoWrappedReader<T[]>, ICountProvider
{
    int ICountProvider.count => innerData.Read().Length;
}
