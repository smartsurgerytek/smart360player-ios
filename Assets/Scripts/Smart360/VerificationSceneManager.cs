using UnityEngine;

public class VerificationSceneManager : MonoBehaviour
{
    [SerializeField] private VerificationResult _verificationResult;
    [SerializeField] private VerificationView _verificationView;
    public VerificationResult verificationResult { get => _verificationResult; set => _verificationResult = value; }
    public VerificationView verificationView
    {
        get => _verificationView;
    }

    internal void Initialize()
    {
        _verificationView.back.AddListener(_=>Application.Quit());
        VerificationView.Views view = default;
        if (verificationResult.deviceInvalid || verificationResult.applicationHashInvalid)
        {
            view = VerificationView.Views.Warning;
        }
        else if (verificationResult.applicationUnpaid)
        {
            view = VerificationView.Views.Purchase;
        } else if (verificationResult.applicationExpired){
            view = VerificationView.Views.Expired;
        }
        _verificationView.ShowView(view);
    }
}
