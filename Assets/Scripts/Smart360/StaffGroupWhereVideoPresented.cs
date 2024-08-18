using Sirenix.Serialization;
using System.Linq;
public class StaffGroupWhereVideoContained : ArrayRouter<StaffGroup>
{
    [OdinSerialize] private IArrayReader<Staff> _staffsReader;
    [OdinSerialize] private IArrayReader<Video> _videosReader;
    public override StaffGroup[] Route(StaffGroup[] staffGroups)
    {
        var videos = _videosReader.Read();
        var staffs = _staffsReader.Read();
        return videos
            .Select(o => o.staff).Distinct()
            .Select(o => staffs[o].group).Distinct()
            .Select(o => staffGroups[o]).ToArray();
    }
}
