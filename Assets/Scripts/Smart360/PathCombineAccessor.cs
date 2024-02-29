using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
public class PathCombineAccessor : IReader<string>
{
    [OdinSerialize] private IReader<string>[] _elements;
    string IReader<string>.Read()
    {
        if (_elements == null) return "";
        var count = _elements.Length;
        var paths = new string[count];
        for (int i = 0; i < count; i++)
        {
            try
            {
                paths[i] = _elements[i]?.Read() ?? "";
            }
            catch
            {
                paths[i] = "";
            }
        }
        return Path.Combine(paths).Replace('/', '\\').Replace('\\', Path.DirectorySeparatorChar);
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
