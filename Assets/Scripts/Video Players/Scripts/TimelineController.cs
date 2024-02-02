using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

namespace SmartSurgery.VideoControllers
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private bool _initialzeOnEnable;

        [SerializeField] private double _length = 0;
        [SerializeField] private float _fastForwardSeconds= 5.0f;
        [SerializeField] private float _rewindSeconds = 5.0f;
        [Header("Components")]
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _rewindButton;
        [SerializeField] private Button _playPauseButton;
        [SerializeField] private Button _fastForwardButton;
        [SerializeField] private Sprite _playIcon;
        [SerializeField] private Sprite _pauseIcon;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField, ReadOnly] private EventTrigger _eventTrigger;
        [Header("Events")]
        [SerializeField] private UnityEvent<float> _dragging;
        [SerializeField] private UnityEvent<float> _dragStart;
        [SerializeField] private UnityEvent<float> _dragEnd;
        [SerializeField] private UnityEvent<bool> _playPuase;
        [SerializeField] private UnityEvent _play;
        [SerializeField] private UnityEvent<float> _playing;
        [SerializeField] private UnityEvent _pause;
        [SerializeField] private UnityEvent _stop;
        [Header("States")]
        [SerializeField, ReadOnly] private bool _isDragging = false;
        [SerializeField, ReadOnly] private bool _isPlaying = false;
        [SerializeField, ReadOnly] private float _time;

        [ShowInInspector, NonSerialized, ReadOnly] private bool _initialized;
        private EventTrigger.Entry[] _pointerEntries;

        public UnityEvent<float> dragStart { get => _dragStart; }
        public UnityEvent<float> dragging { get => _dragging; }
        public UnityEvent<float> dragEnd { get => _dragEnd; }
        public UnityEvent play { get => _play; }
        public UnityEvent<float> playing { get => _playing; }
        public UnityEvent pause { get => _pause; }
        public double length { get => _length; set => _length = value; }
        public bool isDragging { get => _isDragging; private set => _isDragging = value; }
        public bool isPlaying { get => _isPlaying; private set => _isPlaying = value; }
        public float time { get => _time; }
        public UnityEvent stop { get => _stop; set => _stop = value; }

        public void Initialize()
        {
            if (_initialized) return;
            //_multiRangeController = new MultiRangeController();
            //_multiRangeController.enter += _multiRangeController_enter;
            //_multiRangeController.exit += _multiRangeController_exit;
            //_multiRangeController.update += _multiRangeController_update;
            //_multiRangeController.getCurrentValue += _multiRangeController_getCurrentValue;

            isDragging = false;
            _eventTrigger = _slider.GetComponent<EventTrigger>();
            if (!_eventTrigger) _eventTrigger = _slider.gameObject.AddComponent<EventTrigger>();

            _slider.enabled = true;
            _slider.maxValue = (float)_length;

            _pointerEntries = new EventTrigger.Entry[2];

            _pointerEntries[0] = new EventTrigger.Entry();
            _pointerEntries[1] = new EventTrigger.Entry();

            _pointerEntries[0].eventID = EventTriggerType.PointerUp;
            _pointerEntries[1].eventID = EventTriggerType.PointerDown;

            _pointerEntries[0].callback.AddListener(Slider_OnPointerUp);
            _pointerEntries[1].callback.AddListener(Slider_OnPointerDown);

            _eventTrigger.triggers.Add(_pointerEntries[0]);
            _eventTrigger.triggers.Add(_pointerEntries[1]);

            _playPauseButton?.onClick.AddListener(_playPauseButton_OnClick);
            _stopButton?.onClick.AddListener(_stopButton_OnClick);
            _fastForwardButton?.onClick.AddListener(_fastForwardButton_OnClick);
            _rewindButton?.onClick.AddListener(_rewindButton_OnClick);

            _initialized = true;
        }
        private void _playPauseButton_OnClick()
        {
            if (isPlaying)
            {
                pause?.Invoke();
            }
            else
            {
                play?.Invoke();
            }

            isPlaying = !isPlaying;
            UpdatePlayPauseButtonUI();
        }
        private void _stopButton_OnClick()
        {
            Stop();
        }
        private void _fastForwardButton_OnClick()
        {
            _time += _fastForwardSeconds;
        }

        private void _rewindButton_OnClick()
        {
            _time -= _rewindSeconds;
        }
        private void OnEnable()
        {
            if(_initialzeOnEnable) Initialize();
        }
        private void OnDisable()
        {
            Destroy(_eventTrigger);
        }
        private void Update()
        {
            if (!_initialized) return;
            if (isPlaying)
            {
                _time += UnityEngine.Time.deltaTime;
                playing?.Invoke(_time);
            }

            if (_isDragging)
            {
                _time = _slider.value;
                dragging?.Invoke(_time);
            }
            else
            {
                _slider.value = _time;
            }

            _timeText.text = $"{ToTimeString(time)} / {ToTimeString(length)}";
        }
        private string ToTimeString(double seconds)
        {
            var dateTime = new DateTime().AddSeconds(seconds);
            return dateTime.ToString("mm:ss");
        }

        public void Slider_OnPointerDown(BaseEventData pointerEventData)
        {
            if (isDragging) return;
            isDragging = true;
            _time = _slider.value;
            dragStart?.Invoke(_time);
        }
        public void Slider_OnPointerUp(BaseEventData pointerEventData)
        {
            if (!isDragging) return;
            isDragging = false;
            _time = _slider.value;
            dragEnd?.Invoke(_time);
        }


        private void UpdatePlayPauseButtonUI()
        {
            if (isPlaying)
            {
                _playPauseButton.GetComponent<Image>().sprite = _pauseIcon;
            }
            else
            {
                _playPauseButton.GetComponent<Image>().sprite = _playIcon;
            }
        }
        public void Stop()
        {
            _time = 0;
            isPlaying = false;
            stop?.Invoke();
            UpdatePlayPauseButtonUI();
        }

    }
}