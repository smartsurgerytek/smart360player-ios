using UnityEngine;

public class ApplicationQuitController : IController
{
    void IController.Execute()
    {
        Application.Quit();
    }
}
