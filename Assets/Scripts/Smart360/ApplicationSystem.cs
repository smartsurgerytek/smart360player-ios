using Sirenix.OdinInspector;
using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ApplicationSystem : MonoBehaviour
{
    private static ApplicationSystem _instance;

    [SerializeField] private ApplicationManager _applicationManager;
    [SerializeField] private VerificationSystem _verificationSystem;
    [SerializeField] private VerificationView _verificationView;
    [SerializeField] private string _videoPlayerScene;
    [SerializeField] private string _mainMenuScene;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _destroying;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _needShowVerificationView;
    [NonSerialized, ShowInInspector, ReadOnly] private VerificationView.Views _verificationViewToShow;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _needLoadVideoPlayerScene;
    [NonSerialized, ShowInInspector, ReadOnly] private int _editionToLoad;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _waitForVideoPlayerSceneLoaded;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _videoPlayerSceneNeedInitialize;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _needLoadMainMenuScene = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _waitForMainMenuLoaded;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _mainMenuSceneNeedInitialize;

    [NonSerialized, ShowInInspector, ReadOnly] private GameManager _gameManager;
    [NonSerialized, ShowInInspector, ReadOnly] private MainMenuManager _mainManuManager;




    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
            _destroying = true;
        }
        _instance = this;
    }
    private void OnEnable()
    {
        if (_destroying) return;
        if (!_initialized) Initialize();
    }

    private void Initialize()
    {

        _initialized = true;
    }


    private void Update()
    {
        CheckVerificationSystem();
        CheckMainMenu();
        CheckVideoPlayerScene();
        //_menuManager_onClickEditionButton
    }

    private void CheckVerificationSystem()
    {
        if (_needShowVerificationView)
        {
            _verificationView.ShowView(_verificationViewToShow);
            _needShowVerificationView = false;
        }
    }

    private void CheckMainMenu()
    {
        if (_needLoadMainMenuScene)
        {
            SceneManager.LoadScene(_mainMenuScene);
            _mainManuManager = null;
            _needLoadMainMenuScene = false;
            _waitForMainMenuLoaded = true;
        }
        if (_waitForMainMenuLoaded)
        {
            _mainManuManager = GameObject.FindObjectOfType<MainMenuManager>();
            if (_mainManuManager)
            {
                _waitForMainMenuLoaded = false;
                _mainMenuSceneNeedInitialize = true;
            }
        }
        if (_mainMenuSceneNeedInitialize && _mainManuManager)
        {
            _mainManuManager.clickEditionButton.AddListener(_mainMenuManager_onClickEditionButton);
            _mainManuManager.Initialize();
            _mainMenuSceneNeedInitialize = false;
        }
    }

    private void CheckVideoPlayerScene()
    {
        if (_needLoadVideoPlayerScene)
        {
            SceneManager.LoadScene(_videoPlayerScene);
            _needLoadVideoPlayerScene = false;
            _waitForVideoPlayerSceneLoaded = true;
        }
        if (_waitForVideoPlayerSceneLoaded)
        {
            _gameManager = GameObject.FindObjectOfType<GameManager>();
            if (_gameManager)
            {
                _waitForVideoPlayerSceneLoaded = false;
                _videoPlayerSceneNeedInitialize = true;
            }
        }
        if (_videoPlayerSceneNeedInitialize && _gameManager && _gameManager.quit != null)
        {
            _gameManager.quit.AddListener(_gameManager_onQuit);
            _gameManager.Initialize(_editionToLoad);
            _videoPlayerSceneNeedInitialize = false;
        }
    }

    private void _gameManager_onQuit()
    {
        _needLoadMainMenuScene = true;
    }
    private void _mainMenuManager_onClickEditionButton(int i)
    {
        var isValid = IsPurchased(i);
        if (isValid)
        {
            _editionToLoad = i;
            _needLoadVideoPlayerScene = true;
        }
        else
        {
            _needShowVerificationView = true;
            //_verificationViewToShow = VerificationView.Views.
        }
    }

    private bool IsPurchased(int i)
    {
        return true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
