using UnityEngine;

public class MasterApplication : MonoBehaviour
{
    private MasterModel _model;
    private MasterView _view;
    private MasterController _controller;

    public MasterModel model { get => _model; }
    public MasterView view {get=> _view;}
    public MasterController controller {get=> _controller;}
}
