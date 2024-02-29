using UnityEngine;

[AddComponentMenu("Reader/Reader - Bool")]
public class MonoWrappedReader_Bool : MonoWrappedReader<bool>, IReader<bool>
{
    [SerializeField] private bool _reverse;
    bool IReader<bool>.Read()
    {
        if (_reverse)
        {
            return !innerData.Read();
        }
        else
        {
            return innerData.Read();
        }
    }
}
