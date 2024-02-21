using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct VerificationResult
{

    [SerializeField] bool _verified;
    [SerializeField] private bool _deviceInvalid;
    [SerializeField] private bool _lastTimeLoginInvalid;
    [SerializeField] private bool _applicationHashInvalid;
    [SerializeField] private bool _applicationExpired;
    [SerializeField] private bool _applicationUnpaid;
    [SerializeField] private bool[] _moduleHashInvalid;
    [SerializeField] private bool[] _moduleUnpaid;
    [SerializeField] private bool[] _moduleExpired;
    [SerializeField] private bool[] _editionHashInvalid;
    [SerializeField] private bool[] _editionUnpaid;
    [SerializeField] private bool[] _editionExpired;

    public VerificationResult(int moduleCount, int editionCount) : this()
    {
        _moduleHashInvalid = new bool[moduleCount];
        _moduleUnpaid = new bool[moduleCount];
        _moduleExpired = new bool[moduleCount];
        _editionHashInvalid = new bool[editionCount];
        _editionUnpaid = new bool[editionCount];
        _editionExpired = new bool[editionCount];
    }

    public bool applicationHashInvalid { get => _applicationHashInvalid; internal set => _applicationHashInvalid = value; }
    public bool applicationUnpaid { get => _applicationUnpaid; internal set => _applicationUnpaid = value; }
    public bool applicationExpired { get => _applicationExpired; internal set => _applicationExpired = value; }
    public bool lastTimeLoginInvalid { get => _lastTimeLoginInvalid; internal set => _lastTimeLoginInvalid = value; }
    public bool deviceInvalid { get => _deviceInvalid; internal set => _deviceInvalid = value; }
    public bool applicationInvalid
{
    get
    {
        return applicationHashInvalid || applicationUnpaid || applicationExpired || lastTimeLoginInvalid || deviceInvalid;
    }
}
    public bool[] moduleUnpaid { get => _moduleUnpaid; }
    public bool[] moduleExpired { get => _moduleExpired; }
    public bool[] editionUnpaid { get => _editionUnpaid; }
    public bool[] editionExpired { get => _editionExpired; }
    public bool[] moduleHashInvalid { get => _moduleHashInvalid; }
    public bool[] editionHashInvalid { get => _editionHashInvalid; }

    public bool verified { get => _verified; internal set => _verified = value; }
    public bool isValid
    {
        get
        {
            return !applicationHashInvalid && !applicationUnpaid & !applicationExpired & !lastTimeLoginInvalid & !deviceInvalid &
                moduleUnpaid.All(o => !o) &&
                moduleExpired.All(o => !o) &&
                editionUnpaid.All(o => !o) &&
                editionExpired.All(o => !o) &&
                moduleHashInvalid.All(o => !o) &&
                editionHashInvalid.All(o => !o) &&
                verified;
        }
    }
}
