using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UIMerger : MonoBehaviour
{
    [Serializable]
    struct UIMergeData
    {
        [SerializeField] private RectTransform _socket;
        [SerializeField] private RectTransform _item;

        public UIMergeData(RectTransform socket, RectTransform item)
        {
            _socket = socket;
            _item = item;
        }

        public RectTransform socket { get => _socket; }
        public RectTransform item { get => _item; }
    }
    [SerializeField, TableList] private List<UIMergeData> _datas = new List<UIMergeData>();


    private void Update()
    {
        MergeAll();
    }

    private void MergeAll()
    {
        foreach (var data in _datas)
        {
            Merge(data);
        }
    }
    private void Merge(UIMergeData data)
    {
        if (!data.item || !data.socket) return;
        data.item.position = data.socket.position;
        data.item.rotation = data.socket.rotation;
    }
    public void AddMergeItem(RectTransform socket, RectTransform item)
    {
        _datas.Add(new UIMergeData(socket, item));
    }
}
