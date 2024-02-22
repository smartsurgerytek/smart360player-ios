public interface IMasterModel
{
    IApplicationModel application { get; }
    IVideoModel video { get; }
    IModuleModel module { get; }
    IAccessor<Edition[]> edition { get; }
}