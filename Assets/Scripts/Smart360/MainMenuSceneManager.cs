using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MainMenuSceneManager : SerializedMonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _exitButton;

    [Header("Views")]
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private VerificationView _verificationView;

    [Header("Controllers")]
    [SerializeField] private IController _initializer;
    [Header("Events")]
    [SerializeField] private UnityEvent<int> _clickEditionButton;
    [SerializeField] private bool _initializeOnEnable = false;

    [Header("Debug")]
    private bool _initialized;

    //internal IMainMenuSceneContext context { get => _context; set => _context = value; }
    public UnityEvent<int> clickEditionButton { get => _clickEditionButton; }
    public VerificationView verificationView { get => _verificationView; }

    internal event Action<EditionButton> preinitializeEditonButton;
    internal void Initialize()
    {
        if (_initialized) return;
        _initializer.Execute();

        _verificationView.show.AddListener(_verificationView_back);
        _verificationView.back.AddListener(_verificationView_back);
        _exitButton.onClick.AddListener(_exitButton_onClick);

        _initialized = true;
    }

    private void _verificationView_show(VerificationView.Views view)
    {
        _mainMenuView.Hide();
    }
    private void _verificationView_back(VerificationView.Views view)
    {
        switch (view)
        {
            case VerificationView.Views.Purchase:
            case VerificationView.Views.Expired:
                _mainMenuView.Show();
                return;
            case VerificationView.Views.Warning:
                Application.Quit();
                return;
            default:
                return;
        }
    }
    
    private void _exitButton_onClick()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        if (_initializeOnEnable && !_initialized)
        {
            Initialize();
        }
    }
}
