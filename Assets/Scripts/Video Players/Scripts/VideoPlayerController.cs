using System;
using UnityEngine;
using UnityEngine.Video;
namespace SmartSurgery.VideoControllers
{
    public class VideoPlayerController : MonoBehaviour
    {
        [SerializeField] private bool _initializeOnEnable;
        [SerializeField] private VideoPlayer _player;
        [SerializeField] private TimelineController _timeline;
        [SerializeField] private VideoSource _videoSource;

        private bool _initialized;

        public VideoPlayer player { get => _player; }
        public TimelineController timeline
        {
            get => _timeline;
            set
            {
                if (_initialized) throw new SetPropertyAfterInitializationException();
                _timeline = value;
            }
        }
        //public VideoSource videoSource
        //{
        //    get => _videoSource;
        //    set
        //    {
        //        if (_initialized) throw new SetPropertyAfterInitializationException();
        //        _videoSource = value;
        //    }
        //}
        public void Initialize()
        {
            if (_initialized) return;

            player.timeReference = VideoTimeReference.ExternalTime;
            player.Pause();

            _timeline.play.AddListener(_timeline_onPlay);
            _timeline.pause.AddListener(_timeline_onPause);
            _timeline.dragStart.AddListener(_timeline_dragStart);
            _timeline.dragging.AddListener(_timeline_onDragging);
            _timeline.dragEnd.AddListener(_timeline_dragEnd);
            _timeline.playing.AddListener(_timeline_onPlaying);
            _timeline.stop.AddListener(_timeline_onStop);
            _initialized = true;
        }
        private void OnEnable()
        {
            if (_initializeOnEnable) Initialize();
        }

        private void _timeline_onStop()
        {
            player.time = 0;
            player.externalReferenceTime = 0;
            player.Play();
            player.Pause();
        }
        private void _timeline_onPlaying(float time)
        {
            player.externalReferenceTime = time;
        }
        private void _timeline_dragStart(float time)
        {
            player.externalReferenceTime = time;
            player.time = time;
            player.Play();
            player.Pause();
        }
        private void _timeline_onDragging(float time)
        {
            player.externalReferenceTime = time;
            player.time = time;
            player.Play();
            player.Pause();
        }
        private void _timeline_dragEnd(float time)
        {
            player.externalReferenceTime = time;
            player.time = time;
            if (_timeline.isPlaying) player.Play();
        }

        private void _timeline_onPlay()
        {
            player.Play();
        }
        private void _timeline_onPause()
        {
            player.Pause();
        }
    }
}