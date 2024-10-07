
using UnityEngine;

public class DummyLogger : MonoBehaviour
{
    [SerializeField] private string _dummy;
    public void Log(string dummy)
    {
        Debug.Log(dummy);
    }
    public void LogDummy()
    {
        Log(_dummy);
    }
}
