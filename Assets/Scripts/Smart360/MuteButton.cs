using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class MuteButton : MonoBehaviour
{
    //[SerializeField] private bool _muted;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _audioOnIcon;
    [SerializeField] private Sprite _audioOffIcon;
    [SerializeField] private AudioSource _audioSource;

    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }
    public void SetIcons(Sprite on, Sprite off)
    {
        _audioOnIcon = on;
        _audioOffIcon = off;
    }
    private void Reset()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        _button?.onClick?.AddListener(_button_onClick);
    }

    private void OnDisable()
    {
        _button?.onClick?.RemoveListener(_button_onClick);
    }
    private void _button_onClick()
    {
        _audioSource.mute = !_audioSource.mute;
        _image.sprite = _audioSource.mute ? _audioOffIcon : _audioOnIcon;
    }
}
