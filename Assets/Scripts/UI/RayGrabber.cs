using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayGrabber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private bool _isDragging = false;
    [SerializeField] private Transform _target;
    private Transform target => _target ?? this.transform;
    [SerializeField] PointerEventDataPresentor _pointerDownPresentor;
    [SerializeField] PointerEventDataPresentor _pointerUpPresentor;
    [SerializeField] PointerEventDataPresentor _pointerMovePresentor;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (_isDragging) return;
        _pointerDownPresentor.innerData = eventData;
        _isDragging = true;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!_isDragging) return;
        _pointerUpPresentor.innerData = eventData;
        _isDragging = false;
    }
    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        if (_isDragging)
        {
            _pointerMovePresentor.innerData = eventData;
        }
    }

}
[Serializable]
public struct InspectorWrapper_RaycastResult
{
    [ShowInInspector, ReadOnly] private GameObject _gameObject;
    [ShowInInspector, ReadOnly] private BaseRaycaster _module;
    [ShowInInspector, ReadOnly] private float _distance;
    [ShowInInspector, ReadOnly] private float _index;
    [ShowInInspector, ReadOnly] private int _depth;
    [ShowInInspector, ReadOnly] private int _sortingGroupID;
    [ShowInInspector, ReadOnly] private int _sortingGroupOrder;
    [ShowInInspector, ReadOnly] private int _sortingLayer;
    [ShowInInspector, ReadOnly] private Vector3 _worldPosition;
    [ShowInInspector, ReadOnly] private Vector3 _worldNormal;
    [ShowInInspector, ReadOnly] private Vector2 _screenPosition;
    [ShowInInspector, ReadOnly] private int _displayIndex;
    [ShowInInspector, ReadOnly] private bool _isValid;

    public InspectorWrapper_RaycastResult(RaycastResult innerData)
    {
        _gameObject = innerData.gameObject;
        _module = innerData.module;
        _distance = innerData.distance;
        _index = innerData.index;
        _depth = innerData.depth;
        _sortingGroupID = innerData.sortingGroupID;
        _sortingGroupOrder = innerData.sortingGroupOrder;
        _sortingLayer = innerData.sortingLayer;
        _worldPosition = innerData.worldPosition;
        _worldNormal = innerData.worldNormal;
        _screenPosition = innerData.screenPosition;
        _displayIndex = innerData.displayIndex;
        _isValid = innerData.isValid;
    }
}
[Serializable]
public struct InspectorWrapper_PointerEventData
{
    [ShowInInspector, ReadOnly] private GameObject _pointerEnter;
    [ShowInInspector, ReadOnly] private GameObject _lastPress;
    [ShowInInspector, ReadOnly] private GameObject _rawPointerPress;
    [ShowInInspector, ReadOnly] private GameObject _pointerDrag;
    [ShowInInspector, ReadOnly] private GameObject _pointerClick;
    [ShowInInspector, ReadOnly] private InspectorWrapper_RaycastResult _pointerCurrentRaycast;
    [ShowInInspector, ReadOnly] private InspectorWrapper_RaycastResult _pointerPressRaycast;
    [ShowInInspector, ReadOnly] private List<GameObject> _hovered;
    [ShowInInspector, ReadOnly] private bool _eligibleForClick;
    [ShowInInspector, ReadOnly] private int _pointerId;
    [ShowInInspector, ReadOnly] private Vector2 _position;
    [ShowInInspector, ReadOnly] private Vector2 _delta;
    [ShowInInspector, ReadOnly] private Vector2 _pressPosition;
    [ShowInInspector, ReadOnly] private float _clickTime;
    [ShowInInspector, ReadOnly] private int _clickCount;
    [ShowInInspector, ReadOnly] private Vector2 _scrollDelta;
    [ShowInInspector, ReadOnly] private bool _useDragThreshold;
    [ShowInInspector, ReadOnly] private bool _dragging;
    [ShowInInspector, ReadOnly] private PointerEventData.InputButton _button;
    [ShowInInspector, ReadOnly] private float _pressure;
    [ShowInInspector, ReadOnly] private float _tangentialPressure;
    [ShowInInspector, ReadOnly] private float _altitudeAngle;
    [ShowInInspector, ReadOnly] private float _azimuthAngle;
    [ShowInInspector, ReadOnly] private float _twist;
    [ShowInInspector, ReadOnly] private Vector2 _radius;
    [ShowInInspector, ReadOnly] private Vector2 _radiusVariance;
    [ShowInInspector, ReadOnly] private bool _fullyExited;
    [ShowInInspector, ReadOnly] private bool _reentered;

    public InspectorWrapper_PointerEventData(PointerEventData innerData)
    {
        ;
        _pointerEnter = innerData.pointerEnter;
        _lastPress = innerData.lastPress;
        _rawPointerPress = innerData.rawPointerPress;
        _pointerDrag = innerData.pointerDrag;
        _pointerClick = innerData.pointerClick;
        _pointerCurrentRaycast = new InspectorWrapper_RaycastResult(innerData.pointerCurrentRaycast);
        _pointerPressRaycast = new InspectorWrapper_RaycastResult(innerData.pointerPressRaycast);
        _hovered = innerData.hovered;
        _eligibleForClick = innerData.eligibleForClick;
        _pointerId = innerData.pointerId;
        _position = innerData.position;
        _delta = innerData.delta;
        _pressPosition = innerData.pressPosition;
        _clickTime = innerData.clickTime;
        _clickCount = innerData.clickCount;
        _scrollDelta = innerData.scrollDelta;
        _useDragThreshold = innerData.useDragThreshold;
        _dragging = innerData.dragging;
        _button = innerData.button;
        _pressure = innerData.pressure;
        _tangentialPressure = innerData.tangentialPressure;
        _altitudeAngle = innerData.altitudeAngle;
        _azimuthAngle = innerData.azimuthAngle;
        _twist = innerData.twist;
        _radius = innerData.radius;
        _radiusVariance = innerData.radiusVariance;
        _fullyExited = innerData.fullyExited;
        _reentered = innerData.reentered;
    }


}
