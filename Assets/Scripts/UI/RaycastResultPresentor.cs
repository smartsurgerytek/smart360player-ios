using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastResultPresentor : MonoBehaviour
{
    [SerializeField] private InspectorWrapper_RaycastResult _IW_data;
    private RaycastResult _innerData;
    public RaycastResult innerData
    {
        get => _innerData;
        set
        {
            _innerData = value;
            _IW_data = new InspectorWrapper_RaycastResult(value);
        }
    }
}