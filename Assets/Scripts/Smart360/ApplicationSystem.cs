using Sirenix.OdinInspector;
using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSystem : MonoBehaviour
{
    private static ApplicationSystem _instance;
    private MasterApplication _masterApplication;

    [SerializeField] private ApplicationManager _applicationManager;
    [SerializeField] private VerificationSystem _verificationSystem;
    [SerializeField] private string _videoPlayerScene;
    [SerializeField] private string _mainMenuScene;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _destroying;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;


    [NonSerialized, ShowInInspector, ReadOnly] private bool _needLoadVideoPlayerScene;
    [NonSerialized, ShowInInspector, ReadOnly] private int _editionToLoad;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _waitForVideoPlayerSceneLoaded;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _videoPlayerSceneNeedInitialize;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _needLoadMainMenuScene = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _waitForMainMenuLoaded;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _mainMenuSceneNeedInitialize;

    [NonSerialized, ShowInInspector, ReadOnly] private GameManager _gameManager;
    [NonSerialized, ShowInInspector, ReadOnly] private MainMenuManager _mainMenuManager;

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
        //_masterApplication.controller.credentialLoader = new 
        var credentialContext = new EasonLoadingCredentialContext();
        credentialContext = (EasonLoadingCredentialContext)_masterApplication.controller.credentialLoader.Load(credentialContext);

        _verificationSystem.Initialize(_masterApplication.controller.credentialLoader);
        _initialized = true;
    }


    private void Update()
    {
        _verificationSystem.InternalUpdate();
        CheckMainMenu();
        CheckVideoPlayerScene();
        //_menuManager_onClickEditionButton
    }


    private void CheckMainMenu()
    {
        if (_needLoadMainMenuScene)
        {
            SceneManager.LoadScene(_mainMenuScene);
            _mainMenuManager = null;
            _needLoadMainMenuScene = false;
            _waitForMainMenuLoaded = true;
        }
        if (_waitForMainMenuLoaded)
        {
            _mainMenuManager = GameObject.FindObjectOfType<MainMenuManager>();
            if (_mainMenuManager)
            {
                _waitForMainMenuLoaded = false;
                _mainMenuSceneNeedInitialize = true;
            }
        }
        if (_mainMenuSceneNeedInitialize && _mainMenuManager)
        {

            //_verificationSystem.OnMainMenuSceneInitializing(_mainMenuManager);
            _mainMenuManager.verificationSystem = _verificationSystem;
            _mainMenuManager.clickEditionButton.AddListener(_mainMenuManager_onClickEditionButton);
            _mainMenuManager.Initialize();
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

    private void _mainMenuManager_onClickEditionButton(int i)
    {

        var isUnpaid = _verificationSystem.IsEditionUnpaid(i);
        var isExpired = _verificationSystem.IsEditionExpired(i);
        if (isUnpaid)
        {
            //_verificationSystem.needShowVerificationView = true;
            //_verificationSystem.verificationViewToShow = VerificationView.Views.Purchase;
        } 
        else if (isExpired)
        {
            //_verificationSystem.needShowVerificationView = true;
            //_verificationSystem.verificationViewToShow = VerificationView.Views.Expired;
        }
        else
        {
            _editionToLoad = i;
            _needLoadVideoPlayerScene = true;
        }
    }

    private void _gameManager_onQuit()
    {
        _needLoadMainMenuScene = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
