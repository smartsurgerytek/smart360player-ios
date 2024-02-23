using Sirenix.OdinInspector;
using UnityEngine;
public class EditionButtonVerificationStatePreinitializer : SerializedMonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private IProvider<VerificationResult> _resultProvider;

    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        var result = _resultProvider.Get();
        if (!result.verified) throw new System.Exception("Verification result is not verified!");
        editionButton.verificationState = EditionButton.VerificationState.Normal;
        if(!result.TryGetTargetIndex(VerificationTarget.Edition, editionButton.editionId, out var index)) 
        {
            editionButton.verificationState = EditionButton.VerificationState.Unpaid;
        }
        if (result.editionHashInvalid[index]) editionButton.verificationState = EditionButton.VerificationState.Warning;
        else if (result.editionUnpaid[index]) editionButton.verificationState = EditionButton.VerificationState.Unpaid;
        else if (result.editionExpired[index]) editionButton.verificationState = EditionButton.VerificationState.Expired;
    }

}
