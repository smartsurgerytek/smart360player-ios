﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MainMenuSceneManager : SerializedMonoBehaviour
{
    [Header("Context")]
    [SerializeField] private IMainMenuSceneContext _context;

    [Header("Components")]
    [SerializeField] private Button _exitButton;

    [Header("Views")]
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private VerificationView _verificationView;

    [Header("Edition Buttons")]
    [SerializeField] private EditionButton _editionButtonPrefab;
    [SerializeField] private Transform _editionButtonParent;
    [NonSerialized, ShowInInspector, ReadOnly] private EditionButton[] _editionButtons;

    [Header("Controllers")]
    [SerializeField] private List<IEditionButtonPreinitializer> _editionButtonPreInitializers;
    [Header("Events")]
    [SerializeField] private UnityEvent<int> _clickEditionButton;
    [SerializeField] private bool _initializeOnEnable = false;

    [Header("Debug")]
    private bool _initialized;

    internal IMainMenuSceneContext context { get => _context; set => _context = value; }
    public UnityEvent<int> clickEditionButton { get => _clickEditionButton; }
    public VerificationView verificationView { get => _verificationView; }

    internal event Action<EditionButton> preinitializeEditonButton;
    //public int editionCount { get => editionIds.Length; }
    //public int[] editionIds { get => _editionIds; internal set => _editionIds = value; }

    //public VerificationSystem verificationSystem { get => _verificationSystem; internal set => _verificationSystem = value; }
    //public VerificationView verificationView { get => _verificationView; internal set => _verificationView = value; }
    public void AddEditionButtonPreinitializer(IEditionButtonPreinitializer editionButtonPreinitializer)
    {
        _editionButtonPreInitializers.Add(editionButtonPreinitializer);
    }
    private void CleanUpEditionButtons()
    {
        if (_editionButtons == null) return;
        for (int i = 0; i < _editionButtons.Length; i++)
        {
            //_editionButtonsLayout.Remove(i, _editionButtons[i].transform);
            Destroy(_editionButtons[i]?.gameObject);
        }
        _editionButtons = null;
    }
    internal void Initialize()
    {
        if (_initialized) return;

        CleanUpEditionButtons();
        CreateEditionButtons();

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

    private void CreateEditionButtons()
    {
        var count = context.currentEditionIds.Length;
        _editionButtons = new EditionButton[count];
        for (int i = 0; i < count; i++)
        {
            _editionButtons[i] = Instantiate(_editionButtonPrefab, _editionButtonParent);
            _editionButtons[i].index = i;
            _editionButtons[i].clickButton.AddListener(_editionButton_onClick);
            _editionButtons[i].editionId = context.currentEditionIds[i];
            for (int j = 0; j< (_editionButtonPreInitializers?.Count ?? 0); j++)
            {
                _editionButtonPreInitializers[j].OnPreInitialize(_editionButtons[i]);
            }
            _editionButtons[i].Initialize();
        }
    }
    private void _exitButton_onClick()
    {
        Application.Quit();
    }
    private void _editionButton_onClick(int i)
    {
        clickEditionButton?.Invoke(i);
    }
    private void OnEnable()
    {
        if (_initializeOnEnable &&!_initialized) Initialize();
    }
}