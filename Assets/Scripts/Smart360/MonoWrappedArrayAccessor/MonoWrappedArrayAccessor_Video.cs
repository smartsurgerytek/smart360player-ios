using UnityEngine;

[AddComponentMenu("Array Accessor - Video")]
public class MonoWrappedArrayAccessor_Video : MonoWrappedArrayAccessor<Video>
{
    private void OnValidate()
    {
        if (!(innerData is DefaultArrayAccessor<Video>)) return;
        var data = innerData.Read();
        for (int i = 0; i< innerData.count; i++)
        {
            data[i].index = i;
        }
        innerData.Write(data);
    }
}