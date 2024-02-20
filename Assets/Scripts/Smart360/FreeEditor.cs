using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Runtime.CompilerServices;
using UnityEngine;

class FreeEditor : SerializedMonoBehaviour
{
    [OdinSerialize] private ISaverLoader<EditionContext> _editon;
    [OdinSerialize] private ISaverLoader<ModuleContext> _module;
}


