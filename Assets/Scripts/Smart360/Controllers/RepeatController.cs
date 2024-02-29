using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;


public class ForeachController : IController
{
    [OdinSerialize] private IReader<IEnumerable<int>> _enumerable;
    [OdinSerialize] private IController _innerController;
    [OdinSerialize] private IWriter<int> _item;
    void IController.Execute()
    {
        var enumerable = _enumerable.Read();
        foreach (var item in enumerable)
        {
            _item?.Write(item);
            _innerController.Execute();
        }
        
    }
}
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
