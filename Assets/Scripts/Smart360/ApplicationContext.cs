using System;
using UnityEngine;
internal class ApplicationContext : MonoBehaviour, IApplicationContext
{
    [SerializeField] private int _currentModule;
    //[SerializeField] private int[] _currentEditions;
    [SerializeField] private EditionManager _editionManager;
    int IApplicationContext.currentModule { get => _currentModule; set => _currentModule = value; }
    int[] IApplicationContext.currentEditions { get => _editionManager.GetEditionsOfModule(_currentModule); set => throw new NotImplementedException(); }
}
