using System.Collections.Generic;
using UnityEngine;

public class ExactPositionLayout : MonoBehaviour
{
    [SerializeField] private Transform[] _sockets;
    [SerializeField] private Transform _parent;
    [SerializeField] private bool _setParent;
    [SerializeField] private readonly Dictionary<int,List<Transform>> _itemBuckets = new Dictionary<int, List<Transform>>();
    public void Layout(int index, Transform item)
    {
        if (_setParent) item.parent = _parent;
        if (!_itemBuckets.ContainsKey(index)) _itemBuckets[index] = new List<Transform>();
        _itemBuckets[index].Add(item);
    }
    public void Remove(int index, Transform item)
    {
        if (!_itemBuckets.ContainsKey(index)) return;
        _itemBuckets[index].Remove(item);
    }
    private void Update()
    {
        foreach (var index in _itemBuckets.Keys) 
        {
            UpdateItem(index);
        }
    }
    public void UpdateItem(int index)
    {
        var cached = _itemBuckets[index].ToArray();
        for (int i = 0; i < cached.Length; i++)
        {
            var item = cached[i];
            if (!item)
            {
                
                _itemBuckets.Remove(i);
            }
            item.position = _sockets[index].position;
            item.rotation = _sockets[index].rotation;
            item.localScale = Vector3.one;
        }
        foreach (var item in cached)
        {
        }
    }
}
