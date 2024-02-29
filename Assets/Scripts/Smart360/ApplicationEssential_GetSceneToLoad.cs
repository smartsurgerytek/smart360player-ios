using Sirenix.Serialization;

public class ApplicationEssential_GetSceneToLoad : Router<VerificationResult, string>
{
    [OdinSerialize] private IReader<string> _verificationScene;
    [OdinSerialize] private IReader<string> _initialSceneToLoad;
    public override string Route(VerificationResult result)
    {
        var rt = "";
        if (result.applicationInvalid)
        {
            rt = _verificationScene.Read();
        }
        else
        {
            rt = _initialSceneToLoad.Read();
        }
        return rt;
    }
}

