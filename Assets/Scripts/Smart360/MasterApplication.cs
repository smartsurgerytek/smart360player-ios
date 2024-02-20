using UnityEngine;

public class MasterApplication : MonoBehaviour
{
    [SerializeField] private MasterContext _context;
    [SerializeField] private MasterView _view;
    [SerializeField] private MasterController _controller;
    public MasterContext context { get => _context; }
    public MasterView view {get=> _view;}
    public MasterController controller {get=> _controller;}

    internal void Initialize()
    {
        _context.Initialize();
        _view.Initialize();
        _controller.Initialize(_context);
    }
    internal void InternalUpdate()
    {
        _controller.InternalUpdate(_context, _view);
    }
    
}
