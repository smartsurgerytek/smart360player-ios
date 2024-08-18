using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOSPlayerController : MonoBehaviour
{
    public float speed { get => _speed; set => _speed = value; }
    public float lerp { get => _lerp; set => _lerp = value; }

    [Header("Settings")]
    [SerializeField] private float _speed = .33f;
    [SerializeField] private float _lerp = .5f;

    [Header("Components")]
    [SerializeField] private Transform _yawTransform;
    [SerializeField] private Transform _pitchTransform;
    private Vector3 _previousPosition;


    private void Update()
    {
        UpdateMouse();
        var delta = Vector2.zero;
        if (UnityEngine.Application.isEditor)
        {
            delta = GetMouseInputDeltaPositoin();
        }
        else
        {
#if UNITY_IOS
            delta = GetTouchInputDeltaPoistion();
#else
#endif

        }
        if (delta == Vector2.zero) return;
        delta *= speed;
        //_yawTransform.RotateAround(_yawTransform.position,Vector3.up, delta.x);
        var y = _yawTransform.localEulerAngles.y;
        var originalY = y;
        y += delta.x;
        y = Mathf.Lerp(originalY, y, lerp);
        _yawTransform.localEulerAngles = new Vector3(0, y, 0);
        //_pitchTransform.RotateAround(_pitchTransform.position, _yawTransform.right, -delta.y);
        var x = _pitchTransform.localEulerAngles.x;
        while (x < -180)
        {
            x += 360f;
        }
        while (x > 180)
        {
            x -= 360f;
        }
        var originalX = x;
        x += -delta.y;
        x = Mathf.Clamp(x, -90, 90);
        x = Mathf.Lerp(originalX, x, lerp);
        _pitchTransform.localEulerAngles = new Vector3(x, 0, 0);
    }

    private void UpdateMouse()
    {

    }

    private Vector2 GetMouseInputDeltaPositoin()
    {
        var currentPosition = Input.mousePosition;
        var delta = Vector2.zero;
        if (Input.GetMouseButton(0))
        {
            delta = currentPosition - _previousPosition;
        }
        _previousPosition = currentPosition;
        return delta;
    }

    private Vector2 GetTouchInputDeltaPoistion()
    {
        if (Input.touchCount != 2) return Vector2.zero;
        return (Input.GetTouch(0).deltaPosition + Input.GetTouch(1).deltaPosition) * .5f;
    }
}
