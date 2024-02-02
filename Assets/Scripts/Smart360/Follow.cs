using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _follower;
    [SerializeField] private string _followeeName;
    [ShowInInspector] private Transform _followee => GameObject.Find(_followeeName)?.transform;


    [SerializeField] private Transform _anchor;
    [SerializeField, Range(0,100)] private float _speed = 20;
    [SerializeField, Range(0,100)] private float _rotationSpeed = 20;
    [SerializeField] private bool _following = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _followingChanged = false;
    [NonSerialized, ShowInInspector, ReadOnly] private Matrix4x4 _followingMatrix;
    [SerializeField] private float _disableLatancy = .2f;
    [SerializeField] private Coroutine _followingCorotine;
    public bool following
    {
        get => _following;
        set
        {
            if (_following == value) return;
            _following = value;
            _followingChanged = true;
        }
    }
    private void OnEnable()
    {
        _followingMatrix = _follower.localToWorldMatrix;
        _followingChanged = true;
    }
    private void Update()
    {
        if (_followee)
        {
            _followingMatrix = _followee.localToWorldMatrix * _anchor.worldToLocalMatrix * _follower.localToWorldMatrix;
        }

        if (_followingChanged && !_following)
        {
            if (_followingCorotine != null) StopCoroutine(_followingCorotine);
            _followingCorotine = StartCoroutine( FollowForSecond(_disableLatancy));
            _followingChanged = false;
        }
        if (_following)
        {
            FollowStep(_followingMatrix);
        }
    }
    private IEnumerator FollowForSecond(float second)
    {
        var startTime = Time.time;
        while(Time.time - startTime < second)
        {
            FollowStep(_followingMatrix);
            yield return null;
        }
    }
    private void FollowStep(Matrix4x4 target)
    {
        _follower.position = Vector3.Lerp(_follower.position, target.GetPosition(), _speed * Time.deltaTime);
        _follower.rotation = Quaternion.Slerp(_follower.rotation, target.rotation, _rotationSpeed * Time.deltaTime);
    }
}