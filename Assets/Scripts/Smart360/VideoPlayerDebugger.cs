using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerDebugger : MonoBehaviour
{
    [SerializeField] private VideoPlayer _player;
    [ShowInInspector] private double _player_time => _player.time;
    [ShowInInspector] private double _player_externalReferenceTime => _player.externalReferenceTime;
    [ShowInInspector] private VideoTimeReference _player_timeReference => _player.timeReference;
    [ShowInInspector] private bool _player_isPlaying => _player.isPlaying;
    [ShowInInspector] private double _player_length => _player.length;

    [Button("Play")]
    private void OdinPlayButtonClick()
    {
        _player.Play();
    }

    private void Reset()
    {
        _player = GetComponent<VideoPlayer>();
    }
}
