using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("Router/Router - Credential")]
public class MonoWrappedRouter_Credential : MonoWrappedRouter<Credential> { }

public class TransformPositionAccessor : IAccessor<Vector3>
{
    [OdinSerialize]IReader<Transform> _transform;
    Vector3 IReader<Vector3>.Read()
    {
        return _transform.Read().position;
    }

    void IWriter<Vector3>.Write(Vector3 value)
    {
        _transform.Read().position = value;
    }

    void IWriter.Write(object value)
    {
        ((IWriter<Vector3>)this).Write(value);
    }
}
public class TransformRotationAccessor : IAccessor<Quaternion>
{
    [OdinSerialize] IReader<Transform> _transform;
    Quaternion IReader<Quaternion>.Read()
    {
        return _transform.Read().rotation;
    }

    void IWriter<Quaternion>.Write(Quaternion value)
    {
        _transform.Read().rotation = value;
    }
    void IWriter.Write(object value)
    {
        ((IWriter<Quaternion>)this).Write((Quaternion)value);
    }
}