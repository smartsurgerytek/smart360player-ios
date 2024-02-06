using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Edition Verification Manager", menuName = "Managers/Edition Verification Manager")]
public class EditionVerificationManager : ScriptableObject
{

    [SerializeField, TableList] private EditionVerificationModel[] _data;
    public EditionVerificationModel[] data { get => _data?.ToArray(); internal set => _data = value; }


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
    internal EditionVerificationModel[] GetEditionVerifications(int applicationId)
    {
        return data.Where(o => o.applicationId == applicationId).ToArray();
    }
}
