using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;

class FreeEditor : SerializedMonoBehaviour
{
    //[ShowInInspector] private char pathSeperator => Path.DirectorySeparatorChar;
    [OdinSerialize] private ISaverLoader<EditionContext>[] _editon;
    [OdinSerialize] private ISaverLoader<ModuleContext>[] _module;
}