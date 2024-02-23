using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
public class PathCombineAccessor : IAccessor<string>
{
    [OdinSerialize] private IAccessor<string>[] _elements;
    string IReader<string>.Read()
    {
        var count = _elements.Length;
        var paths = new string[count];
        for (int i = 0; i < count; i++)
        {
            try
            {
                paths[i] = _elements[i].Read();
            }
            catch
            {
                paths[i] = "";
            }
        }
        return Path.Combine(paths);
    }

    void IWriter<string>.Write(string value)
    {
        throw new NotImplementedException();
    }

    [ShowInInspector, LabelText("Path")] private string odinPath
    {
        get
        {
            try
            {

                return ((IReader<string>)this).Read();
            }
            catch 
            {
                return "";
            }

        }

    }
}
