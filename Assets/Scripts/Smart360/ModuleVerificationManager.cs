using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Module Verification Manager", menuName = "Managers/Module Verification Manager")]
public class ModuleVerificationManager : ScriptableObject
{

    [SerializeField, TableList] private ModuleVerificationModel[] _data;

    public ModuleVerificationModel[] data { get => _data?.ToArray(); internal set => _data = value; }

    internal ModuleVerificationModel[] GetModuleVerifications(int applicationId)
    {
        return data.Where(o => o.applicationId == applicationId).ToArray();
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
