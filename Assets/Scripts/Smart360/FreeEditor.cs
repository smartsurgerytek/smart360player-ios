using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;

class FreeEditor : SerializedMonoBehaviour
{
    //[ShowInInspector] private char pathSeperator => Path.DirectorySeparatorChar;
    [OdinSerialize] private IAccessor<Edition[]> _editon;
    [OdinSerialize] private IAccessor<Module[]> _module;
}