using System;
using UnityEngine;
internal class ApplicationContext : MonoBehaviour, IApplicationContext
{
    [SerializeField] private int _currentModule;
    private IEditionController _editionController;
    private IAccessor<Edition[]> _editions;
    int IApplicationContext.currentModule { get => _currentModule; set => _currentModule = value; }
    int[] IApplicationContext.currentEditions { get => _editionController.GetEditionsOfModule(_editions.Read(),_currentModule); set => throw new NotImplementedException(); }
}
