using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventDataPresentor : MonoBehaviour
{
    [SerializeField] private InspectorWrapper_PointerEventData _IW_data;
    private PointerEventData _innerData;

    [SerializeField] private RaycastResultPresentor _currentRaycastResultPresentor;
    [SerializeField] private RaycastResultPresentor _pressRaycastResultPresentor;

    public PointerEventData innerData
    {
        get => _innerData;
        set
        {
            _innerData = value;
            _IW_data = new InspectorWrapper_PointerEventData(value);
        }
    }


}
