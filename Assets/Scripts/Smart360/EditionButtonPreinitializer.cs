using UnityEngine;

public class EditionButtonPreinitializer : MonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private IEditionModel _editionModel;
    [SerializeField] private IEditionController _controller;
    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        editionButton.title = _controller.GetEnglishName(_editionModel, editionButton.editionId);
    }
}
