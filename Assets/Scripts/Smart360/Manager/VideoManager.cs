using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

#if UNITY_EDITOR
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo(Constants.EditorAssemblyName)]
#endif

[CreateAssetMenu(fileName = "Video Manager", menuName = "Managers/Video Manager")]
public class VideoManager : ScriptableObject, IArrayReader<SurroundingVideo>, IArrayReader<Video>
{
    [SerializeField] private LoadDataMethod _loadDataMethod;
    [SerializeField, TableList] private Video[] _data;
    [SerializeField, TableList] private SurroundingVideo[] _surroundingData;
    public Video[] data { get => _data?.ToArray(); internal set => _data = value; }
    public SurroundingVideo[] surroundingData { get => _surroundingData?.ToArray(); internal set => _surroundingData = value; }
    public LoadDataMethod loadDataMethod { get => _loadDataMethod; }
    int ICountProvider.count => surroundingData.Length;

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
    public Video[] GetVideoModelsByEdition(int edition)
    {
            return _data.Where(video => video.edition == edition).ToArray();
    }


    SurroundingVideo[] IReader<SurroundingVideo[]>.Read()
    {
        return _surroundingData.ToArray();
    }

    Video[] IReader<Video[]>.Read()
    {
        return _data?.ToArray(); 
    }
}
