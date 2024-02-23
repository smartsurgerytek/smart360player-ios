using Sirenix.Serialization;
using UnityEngine;

[AddComponentMenu("Eason/MVC/Controller")]
public class MonoWrappedController : MonoWrapper<IController>, IController
{
    void IController.Execute()
    {
        innerData.Execute();
    }
}

