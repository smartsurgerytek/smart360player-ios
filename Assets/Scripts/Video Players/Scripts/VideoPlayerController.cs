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
        public void Initialize()
        {
            if (_initialized) return;

            player.timeReference = VideoTimeReference.ExternalTime;
            player.Pause();
#if UNITY_IOS

            player.prepareCompleted += _players_prepareCompleted;
            player.seekCompleted += _players_seekCompleted;
#endif

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
#if UNITY_IOS
#else
#endif
            player.Play();
            player.Pause();
        }
        private void _timeline_onPlaying(float time)
        {
            player.externalReferenceTime = time;
        }
        private void _timeline_dragStart(float time)
        {
#if UNITY_IOS
            player.Pause();
            player.externalReferenceTime = time;
#else
            player.externalReferenceTime = time;
            player.time = time;
            player.Play();
            player.Pause();
#endif
        }
        private void _timeline_onDragging(float time)
        {
            player.externalReferenceTime = time;
#if UNITY_IOS
            //player.Prepare();
#else
            player.time = time;
            player.Play();
            player.Pause();
#endif
        }
        private void _timeline_dragEnd(float time)
        {
            player.time = _timeline.time;
#if UNITY_IOS
#else
#endif
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
#if UNITY_IOS
        private void _players_seekCompleted(VideoPlayer source)
        {
            //source.time = timeline.time;
            if (!source.isPlaying && _timeline.isPlaying) source.Play();
            Debug.Log("Seek Completed.");
        }
        private void _players_prepareCompleted(VideoPlayer source)
        {
            Debug.Log("Prepare!");
            source.time = timeline.time;
            source.externalReferenceTime = timeline.time;
        }
#endif
    }
}