using System.Collections.Generic;

public static class GlobalContext
{
    private static readonly Dictionary<string, object> _map = new Dictionary<string, object>();
    public static bool TryGet<T>(string key, out T value)
    {
        value = default(T);
        try
        {
            var found = _map.TryGetValue(key, out var uncastValue);
            if (found && uncastValue is T)
            {
                value = (T)uncastValue;
                return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
    public static T Get<T>(string key)
    {
        if (!(_map[key] is T)) throw new KeyNotFoundException();
        return (T)_map[key];
    }
    public static void Set<T>(string key, T value)
    {
        _map[key] = value;
    }
}
