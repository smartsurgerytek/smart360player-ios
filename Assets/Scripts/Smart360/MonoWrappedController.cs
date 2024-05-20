using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Eason/MVC/Controller")]
public class MonoWrappedController : MonoWrapper<IController>, IController
{
    [Button("Execute")]
    void IController.Execute()
    {
        innerData.Execute();
    }
    public void Execute()
    {
        ((IController)this).Execute();
    }
}
