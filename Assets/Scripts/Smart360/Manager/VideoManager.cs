using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

#if UNITY_EDITOR
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo(Constants.EditorAssemblyName)]
#endif

[CreateAssetMenu(fileName = "Video Manager", menuName = "Managers/Video Manager")]
public class VideoManager : ScriptableObject
{
    [SerializeField] private LoadDataMethod _loadDataMethod;
    [SerializeField, TableList] private VideoModel[] _data;
    [SerializeField, TableList] private SurroundingVideoModel[] _surroundingData;
    public VideoModel[] data { get => _data?.ToArray(); internal set => _data = value; }
    public SurroundingVideoModel[] surroundingData { get => _surroundingData?.ToArray(); internal set => _surroundingData = value; }
    public LoadDataMethod loadDataMethod { get => _loadDataMethod; }

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }
    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].index = i;
        }
        for (int i = 0; i < _surroundingData.Length; i++)
        {
            _surroundingData[i].index = i;
        }
    }

    public int GetCount()
    {
        return _data?.Length ?? 0;
    }
    public VideoClip[] GetVideos()
    {
        if (_data == null) return null;
        return _data?.Select(videoData => videoData.clip).ToArray();
    }
    public VideoModel[] GetVideoModelsByEdition(int edition)
    {
            return _data.Where(video => video.edition == edition).ToArray();
    }

}
