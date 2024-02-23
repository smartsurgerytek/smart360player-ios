using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Credential
{

    [SerializeField] ApplicationCredential _application;
    [SerializeField, TableList] private ModuleCredential[] _modules;
    [SerializeField, TableList] private EditionCredential[] _editions;

    public ApplicationCredential application { get => _application; }
    public ModuleCredential[] modules { get => _modules.ToArray(); }
    public EditionCredential[] editions { get => _editions.ToArray(); }
    public bool purchased { get => application.purchased; }


#if UNITY_EDITOR
    internal void SetApplicationHash(string hash)
    {
        _application.hash = hash;
    }
    internal void SetEditionHash(int id, string hash)
    {
        _editions[id].hash = hash;
    }
    internal void SetModuleHash(int id, string hash)
    {
        _modules[id].hash = hash;
    }
    internal void FillDuid(string duid)
    {
        _application.SetDuid(duid);
        for (int i = 0; i < modules.Length; i++)
        {
            _modules[i].SetDuid(duid);
        }
        for (int i = 0; i < editions.Length; i++)
        {
            _editions[i].SetDuid(duid);
        }
    }
    internal void FillExpiredDate(long time)
    {
        _application.SetExpiredDate(time);
        for (int i = 0; i < modules.Length; i++)
        {
            _modules[i].SetExpiredDate(time);
        }
        for (int i = 0; i < editions.Length; i++)
        {
            _editions[i].SetExpiredDate(time);
        }
    }
#endif
}
