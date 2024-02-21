using UnityEngine;

public class MonoEditionModel : MonoBehaviour, IEditionModel
{
    [SerializeField] private Edition[] _data;
    Edition[] IEditionModel.data { get => _data; set => _data = value; }
}