using Sirenix.Serialization;

public class MulticastController : IController
{
    [OdinSerialize] private IController[] _controllers;
    void IController.Execute()
    {
        for (int i = 0; i < _controllers.Length; i++)
        {
            _controllers[i].Execute();
        }
    }
}
