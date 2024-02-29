using Sirenix.OdinInspector;
using UnityEngine;
public class EditionButtonVerificationStatePreinitializer : IController
{
    [SerializeField] private IReader<VerificationResult> _result;
    [SerializeField] private IReader<EditionButton> _button;
    void IController.Execute()
    {
        var button = _button.Read();
        var result = _result.Read();
        if (!result.verified) throw new System.Exception("Verification result is not verified!");
        button.verificationState = EditionButton.VerificationState.Normal;
        if(!result.TryGetTargetIndex(VerificationTarget.Edition, button.editionId, out var index)) 
        {
            button.verificationState = EditionButton.VerificationState.Unpaid;
        }
        if (result.editionHashInvalid[index]) button.verificationState = EditionButton.VerificationState.Warning;
        else if (result.editionUnpaid[index]) button.verificationState = EditionButton.VerificationState.Unpaid;
        else if (result.editionExpired[index]) button.verificationState = EditionButton.VerificationState.Expired;
    }

}
