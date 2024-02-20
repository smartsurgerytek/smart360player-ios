using System;
using UnityEngine;

[Serializable]
internal class VerificationContext : MonoBehaviour, IVerificationContext,IProvider<VerificationResult>
{
    [SerializeField] private VerificationResult _result;

    VerificationResult IVerificationContext.result { get => _result; set => _result = value; }

    //VerificationResult IVerificationContext.result 
    //{
    //    get => _result;
    //    set => _result = value;
    //}

    VerificationResult IProvider<VerificationResult>.Get()
    {
        return _result;
    }
}