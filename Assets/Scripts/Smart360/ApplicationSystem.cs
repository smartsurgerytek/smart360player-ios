using Sirenix.OdinInspector;
using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
public interface IEditionButtonPreinitializer
{
    internal void OnPreInitialize(EditionButton editionButton);
}
public class ApplicationSystem : MonoBehaviour
{
    private static ApplicationSystem _instance;
    [SerializeField] private MasterApplication _masterApplication;

    [SerializeField] private ApplicationManager _applicationManager;
    [SerializeField] private VerificationSystem _verificationSystem;
    [SerializeField] private string _initialSceneToLoad;
    [SerializeField] private string _videoPlayerScene;
    [SerializeField] private string _mainMenuScene;


    [NonSerialized, ShowInInspector, ReadOnly] private bool _destroying;

    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;

    // general scene
    [NonSerialized, ShowInInspector, ReadOnly] private bool _needToLoadScene; // trigger SceneManager.LoadScene
    [NonSerialized, ShowInInspector, ReadOnly] private bool _needToLoadSceneContext; // trigger
    [NonSerialized, ShowInInspector, ReadOnly] private bool _needToInitializeScene;
    [NonSerialized, ShowInInspector, ReadOnly] private string _sceneToLoad = "MainMenu";

    [NonSerialized, ShowInInspector, ReadOnly] private int _editionToLoad;


    [NonSerialized, ShowInInspector, ReadOnly] private VideoPlayerSceneManager _videoPlayerSceneManager;
    [NonSerialized, ShowInInspector, ReadOnly] private MainMenuSceneManager _mainMenuManager;

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
        _masterApplication.Initialize();
        _sceneToLoad = _initialSceneToLoad;
        _needToLoadScene = true;
        //_verificationSystem.Initialize();
        _initialized = true;
    }


    private void Update()
    {

        //_verificationSystem.InternalUpdate();
        CheckNeedToLoadScene(); // Check if there is a scene need to load
        CheckNeedToLoadSceneContext(); // Check if there is a scene just loaded.
        //CheckViewContext(); // After Scene just loaded, we need to ensure the component context is filled. 
        CheckNeedToInitializeScene(); // After the component found, we start to initialize each component.
        //_masterApplication.Update();
    }

    private void CheckNeedToLoadScene()
    {
        if (!_needToLoadScene) return;
        SceneManager.LoadScene(_sceneToLoad);

        if (_sceneToLoad ==_mainMenuScene)
        {
            _mainMenuManager = null;
        }
        else if (_sceneToLoad == _videoPlayerScene)
        {
        }
        _needToLoadScene = false;

        if (!_needToLoadScene) _needToLoadSceneContext = true;
    }

    private void CheckNeedToLoadSceneContext()
    {
        if (!_needToLoadSceneContext) return;

        if (_sceneToLoad == _mainMenuScene)
        {
            _mainMenuManager = GameObject.FindObjectOfType<MainMenuSceneManager>();
            if (_mainMenuManager)
            {
                _mainMenuManager.context = _masterApplication.context.mainMenuScene;
                _masterApplication.view.verificationView = _mainMenuManager.verificationView;

                _needToLoadSceneContext = false;
            }
        }
        else if (_sceneToLoad == _videoPlayerScene)
        {
            _videoPlayerSceneManager = GameObject.FindObjectOfType<VideoPlayerSceneManager>();
            if (_videoPlayerSceneManager)
            {
                _needToLoadSceneContext = false;
            }
        }
        if (!_needToLoadSceneContext) _needToInitializeScene = true;
    }
    private void CheckNeedToInitializeScene()
    {
        if (!_needToInitializeScene) return;
        if (_sceneToLoad == _mainMenuScene)
        {
            _mainMenuManager.AddEditionButtonPreinitializer(_masterApplication.controller.editionButtonPreinitializer);
            _mainMenuManager.clickEditionButton.AddListener(_mainMenuManager_onClickEditionButton);
            _mainMenuManager.Initialize();
        }
        else if (_sceneToLoad == _videoPlayerScene)
        {
            _videoPlayerSceneManager.quit.AddListener(_videoPlayerSceneManager_onQuit);
            _videoPlayerSceneManager.Initialize(_editionToLoad);
        }
        _needToInitializeScene = false;
    }


    private void _mainMenuManager_onClickEditionButton(int i)
    {
        var verification = _masterApplication.context.verification.result;
        var view = _masterApplication.view.verificationView;
        var isUnpaid = verification.editionUnpaid[i];
        var isExpired = verification.editionExpired[i];
        var isHashInvalid= verification.editionHashInvalid[i];
        if (isUnpaid)
        {
            view.needToShowView = true;
            view.viewToShow = VerificationView.Views.Purchase;
        } 
        else if (isExpired)
        {
            view.needToShowView = true;
            view.viewToShow = VerificationView.Views.Expired;
        }
        else if (isHashInvalid)
        {
            view.needToShowView = true;
            view.viewToShow = VerificationView.Views.Warning;
        }
        else
        {
            _editionToLoad = i;
            _needToLoadScene= true;
            _sceneToLoad = _videoPlayerScene;
        }
    }
    private void _videoPlayerSceneManager_onQuit()
    {
        _needToLoadScene = true;
        _sceneToLoad = _videoPlayerScene;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
