#if UNITY_EDITOR
using UnityEditor;
#endif
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

class FreeEditor : SerializedMonoBehaviour
{
    //[ShowInInspector] private char pathSeperator => Path.DirectorySeparatorChar;
    [OdinSerialize] private IAccessor<Edition[]> _editon;
    [OdinSerialize] private IAccessor<Module[]> _module;
#if UNITY_EDITOR
    [MenuItem("Free/Check 1")]
    private static void OdinFreeButton_1()
    {
        Debug.Log(JsonUtility.ToJson(new Edition[1]
        {
        new Edition{
            id = 0,
            displayName = "Hello,",
            englishName = "World!",
            name = "HelloWorld"
        },
        }));
    }
#endif
}
[Serializable]
public class Wrapper<T>
{
    [SerializeField] private T _value;

    public Wrapper(T value)
    {
        _value = value;
    }

    public T value { get => _value; }
}