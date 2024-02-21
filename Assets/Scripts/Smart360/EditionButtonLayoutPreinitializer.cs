using UnityEngine;

public class EditionButtonLayoutPreinitializer : MonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private ExactPositionLayout _layout;
    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        _layout.Layout(editionButton.index, editionButton.transform);
    }
}
