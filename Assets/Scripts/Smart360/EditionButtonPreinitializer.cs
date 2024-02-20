using UnityEngine;

public class EditionButtonPreinitializer : MonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private EditionManager _editionManager;
    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        editionButton.title = _editionManager.data[editionButton.editionId].englishName + " Edtion";
    }
}
