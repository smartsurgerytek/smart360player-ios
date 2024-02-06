using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName ="Application Verification Manager", menuName = "Managers/Application Verification Manager")]
public class ApplicationVerificationManager : ScriptableObject
{

    [SerializeField, TableList] private ApplicationVerificationModel[] _data;
    public ApplicationVerificationModel[] data { get => _data?.ToArray(); internal set => _data = value; }

    internal object GetApplicationVerification(int applicationId)
    {
        return data[applicationId];
    }

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateId();
    }
    private void ValidateId()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }
}
