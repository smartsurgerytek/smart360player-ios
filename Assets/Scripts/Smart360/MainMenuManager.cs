using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private ApplicationManager _applicationManager;
    [Header("Components")]
    [SerializeField] private VerificationSystem _verificationSystem;
    [SerializeField] private Button _exitButton;

    [Header("Edition Buttons")]
    [SerializeField, HideInInspector] private bool[] _enables;
    [SerializeField] private EditionButton _editionButtonPrefab;
    [SerializeField, SceneObjectsOnly] private ExactPositionLayout _editionButtonsLayout;
    [NonSerialized, ShowInInspector, ReadOnly] private EditionButton[] _editionButtons;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> _clickEditionButton;
    [SerializeField] private bool _initializeOnEnable = false;

    [Header("Debug")]
    private bool _initialized;


    public ApplicationManager applicationManager { get => _applicationManager; internal set => _applicationManager = value; }
    public UnityEvent<int> clickEditionButton { get => _clickEditionButton; }

    private void CleanUp()
    {
        if (_editionButtons == null) return;
        for (int i = 0; i < _editionButtons.Length; i++)
        {
            _editionButtonsLayout.Remove(i, _editionButtons[i].transform);
            Destroy(_editionButtons[i].gameObject);
        }
    }
    internal void Initialize()
    {
        if (_initialized) return;
        CleanUp();

        //_muteButton = GetComponent<MuteButton>();
        //_muteButton.SetAudioSource(_audioSource);
        _exitButton.onClick.AddListener(_exitButton_onClick);
        var editions = _applicationManager?.editionManager?.data;
        if (editions == null)
        {
            return;
        }
        _editionButtons = new EditionButton[editions.Length];
        for (int i = 0; i < editions.Length; i++)
        {
            _editionButtons[i] = Instantiate(_editionButtonPrefab);
            _editionButtons[i].clickButton.AddListener(_editionButton_onClick);
            _editionButtons[i].Initialize(i, editions[i].englishName, _enables[i]);
            
            _editionButtonsLayout.Layout(i, _editionButtons[i].transform);
        }
        _initialized = true;
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
