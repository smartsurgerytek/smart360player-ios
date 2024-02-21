using System.Linq;

internal interface IEditionController
{
    int[] GetEditionsOfModule(Edition[] model, int moduleId);
    string GetEnglishName(Edition[] model, int editionId);
}

internal class DefaultEditionController : IEditionController
{
    int[] IEditionController.GetEditionsOfModule(Edition[] model, int moduleId)
    {
        return model.Where(o => o.module == moduleId).Select(o => o.id).ToArray();
    }

    string IEditionController.GetEnglishName(Edition[] model, int editionId)
    {
        return model.First(o => o.id == editionId).englishName;
    }
}