using UnityEngine;

[CreateAssetMenu(fileName = "Manifest Manager", menuName = "Managers/Manifest Manager")]
public class ManifestManager : ScriptableObject
{
    [SerializeField] Manifest menifest;
}
