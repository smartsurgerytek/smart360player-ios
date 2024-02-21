using System.Linq;
using UnityEngine;
public class EditionManagerSaverLoader : ISaverLoader<EditionContext>
{
    [SerializeField] private EditionManager _manager;
    void ISaver<EditionContext>.Save(EditionContext data)
    {
        _manager.data = data.data.ToArray();
    }

    EditionContext ILoader<EditionContext>.Load()
    {
        var context = new EditionContext
        {
            data = _manager.data.ToArray(),
        };
        return context;
    }
}
