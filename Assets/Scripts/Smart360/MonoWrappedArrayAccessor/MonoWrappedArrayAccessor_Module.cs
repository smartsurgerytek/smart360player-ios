using UnityEngine;

[AddComponentMenu("Array Accessor - Module")]
public class MonoWrappedArrayAccessor_Module : MonoWrappedArrayAccessor<Module>
{

}

public class HelloWorldController : IController
{
    void IController.Execute()
    {
        Debug.Log("Hello, World!");
    }
}