using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "User Manager", menuName = "Managers/User Manager")]
public class UserManager : ScriptableObject
{

    [SerializeField, TableList] private UserModel[] _data;
    public UserModel[] data { get => _data?.ToArray(); internal set => _data = value; }

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateId();
    }
    private void ValidateId()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }
}
