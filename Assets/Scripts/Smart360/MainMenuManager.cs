using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuManager : SerializedMonoBehaviour
{
    [Header("Components")]
    //[SerializeField] private VerificationSystem _verificationSystem;
    [SerializeField] private Button _exitButton;
    //[SerializeField] private VerificationView _verificationView;

    [Header("Edition Buttons")]
    //[SerializeField, HideInInspector] private bool[] _enables;
    //[SerializeField, SceneObjectsOnly] private ExactPositionLayout _editionButtonsLayout;
    [SerializeField] private EditionButton _editionButtonPrefab;
    [SerializeField] private Transform _editionButtonParent;
    [SerializeField] private int[] _editionButtonIds;
    [NonSerialized, ShowInInspector, ReadOnly] private EditionButton[] _editionButtons;

    [Header("Controllers")]
    [SerializeField] private IEditionButtonPreinitializer[] _editionButtonPreInitializers;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> _clickEditionButton;
    [SerializeField] private bool _initializeOnEnable = false;

    [Header("Debug")]
    private bool _initialized;

    public UnityEvent<int> clickEditionButton { get => _clickEditionButton; }

    internal event Action<EditionButton> preinitializeEditonButton;
    public int editionCount { get => _editionButtonIds.Length; }

    //public VerificationSystem verificationSystem { get => _verificationSystem; internal set => _verificationSystem = value; }
    //public VerificationView verificationView { get => _verificationView; internal set => _verificationView = value; }

    private void CleanUpEditionButtons()
    {
        if (_editionButtons == null) return;
        for (int i = 0; i < _editionButtons.Length; i++)
        {
            //_editionButtonsLayout.Remove(i, _editionButtons[i].transform);
            Destroy(_editionButtons[i].gameObject);
        }
    }
    internal void Initialize()
    {
        if (_initialized) return;
        CleanUpEditionButtons();
        CreateEditionButtons();
        //_verificationSystem.verificationView = verificationView;
        _exitButton.onClick.AddListener(_exitButton_onClick);

        _initialized = true;
    }
    private void CreateEditionButtons()
    {
        //var editions = _applicationManager?.editionManager?.data;
        //if (editions == null)
        //{
        //    return;
        //}
        //var view = rawView.mainMenuView.editionButtonsView;
        //var credentialContext = context.credential;
        //var editions = context.edition.GetCurrentEditions();
        //view.CleanUp();
        _editionButtons = new EditionButton[editionCount];
        for (int i = 0; i < editionCount; i++)
        {
            _editionButtons[i] = Instantiate(_editionButtonPrefab, _editionButtonParent);
            _editionButtons[i].index = i;
            for (int j = 0; j< (_editionButtonPreInitializers?.Length ?? 0); j++)
            {
                _editionButtonPreInitializers[j].OnPreInitialize(_editionButtons[i]);
            }
            _editionButtons[i].Initialize();
        }
        //_editionButtons = new EditionButton[editions.Length];
        //for (int i = 0; i < editions.Length; i++)
        //{
        //    var state = EditionButton.State.Normal;
        //    if (!_enables[i]) state = EditionButton.State.Unenabled;
        //    else if (verificationSystem.IsEditionUnpaid(i)) state = EditionButton.State.Unpaid;
        //    else if (verificationSystem.IsEditionExpired(i)) state = EditionButton.State.Expired;


        //    _editionButtonsLayout.Layout(i, _editionButtons[i].transform);
        //}
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
