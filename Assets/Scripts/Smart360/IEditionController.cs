using System.Linq;

internal interface IEditionController
{
    int[] GetEditionsOfModule(IEditionModel editionModel, int currentModule);
}

internal class DefaultEditionController : IEditionController
{
    public int[] GetEditionsOfModule(IEditionModel model, int module)
    {
        return model.data.Where(o => o.module == module).Select(o => o.id).ToArray();
    }
}