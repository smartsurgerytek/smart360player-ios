public interface IMasterModel
{
    IApplicationModel application { get; }
    IVideoModel video { get; }
    IModuleModel module { get; }
    IEditionModel edition { get; }
}