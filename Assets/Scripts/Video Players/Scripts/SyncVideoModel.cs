using System;
using UnityEngine;
using UnityEngine.Video;

namespace SmartSurgery.VideoControllers
{
    [Serializable]
    public struct SyncVideoModel
    {
        [SerializeField] private int _index;
        [SerializeField] private double _startTime;
        [SerializeField] private string _title;
        [SerializeField] private Sprite _icon;
        [SerializeField] private VideoClip _clip;
        [SerializeField] private string _url;
        [SerializeField] private double _duration;

        public SyncVideoModel(int index, double startTime, string title, Sprite icon, VideoClip clip, string url, double duration)
        {
            _index = index;
            _startTime = startTime;
            _title = title;
            _icon = icon;
            _clip = clip;
            _url = url;
            _duration = duration;
        }

        public int index { get => _index; internal set => _index = value; }
        public double startTime => _startTime;
        public string title => _title;
        public Sprite icon => _icon;
        public VideoClip clip => _clip;
        public string url => _url;

        public double duration  => _duration; 
    }
}