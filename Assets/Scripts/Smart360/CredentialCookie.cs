using Eason.Odin;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[SerializeField]
public struct CredentialCookie
{
    [SerializeField, DateTime] private long _lastTimeLogin;
    public long lastTimeLogin { get => _lastTimeLogin; }
    [Button]
    public void UpdateCookie()
    {
        _lastTimeLogin = DateTime.Now.Ticks;
    }
}
