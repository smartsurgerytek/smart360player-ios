using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EditionButtonVerificationStatePreinitializer : MonoBehaviour, IEditionButtonPreinitializer
{
    [SerializeField] private VerificationResult _verificationResult;
    void IEditionButtonPreinitializer.OnPreInitialize(EditionButton editionButton)
    {
        editionButton.verificationState = EditionButton.VerificationState.Normal;
        if (_verificationResult.editionHashInvalid[editionButton.EditionId]) editionButton.verificationState = EditionButton.VerificationState.Unenabled;
        else if (_verificationResult.editionUnpaid[editionButton.EditionId]) editionButton.verificationState = EditionButton.VerificationState.Unpaid;
        else if (_verificationResult.editionExpired[editionButton.EditionId]) editionButton.verificationState = EditionButton.VerificationState.Expired;
    }
}
