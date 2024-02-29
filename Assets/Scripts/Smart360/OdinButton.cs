using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class OdinButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _execute;
    [Button] private void Execute()
    {
        _execute?.Invoke();
    }
}
