using Sirenix.OdinInspector;
using UnityEngine;
interface IWriter<T>
{
    void Write(T target);
}
public class EditionButtonVerificationStatePreinitializer : SerializedMonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private IProvider<VerificationResult> _resultProvider;

    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        var result = _resultProvider.Get();
        if (!result.verified) throw new System.Exception("Verification result is not verified!");
        editionButton.verificationState = EditionButton.VerificationState.Normal;
        if (result.editionHashInvalid[editionButton.editionId]) editionButton.verificationState = EditionButton.VerificationState.Warning;
        else if (result.editionUnpaid[editionButton.editionId]) editionButton.verificationState = EditionButton.VerificationState.Unpaid;
        else if (result.editionExpired[editionButton.editionId]) editionButton.verificationState = EditionButton.VerificationState.Expired;
    }

}
