using System;
using UnityEngine;

public class MasterView : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;

    [SerializeField] private VerificationView _verificationView;



    public VerificationView verificationView { get => _verificationView; internal set => _verificationView = value; }

    internal void Initialize()
    {
    }
}
