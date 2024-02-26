using Sirenix.Serialization;

public class RepeatController : IController
{
    [OdinSerialize] private IReader<int> _time;
    [OdinSerialize] private IController _controller;

    void IController.Execute()
    {
        var time = _time.Read();
        for (int i = 0; i < time; i++)
        {
            _controller.Execute();
        }
    }
}
