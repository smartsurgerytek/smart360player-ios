using Sirenix.Serialization;

public class IfController : IController
{
    [OdinSerialize] private IReader<bool> _condition;
    [OdinSerialize] private IController _trueController;
    [OdinSerialize] private IController _falseController;

    void IController.Execute()
    {
        var result = _condition.Read();
        if (result)
        {
            _trueController?.Execute();
        }
        else
        {
            _falseController?.Execute();
        }
    }
}