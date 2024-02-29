using Sirenix.Serialization;

public class VerificationViewController : IController
{
    [OdinSerialize] private IReader<int> _editionId;
    [OdinSerialize] private IReader<VerificationView> _view;
    [OdinSerialize] private IReader<VerificationResult> _result;

    public void Execute()
    {
        var view = _view.Read();
        if (TryGetVerificationView(out var viewToShow))
        {
            view.needToShowView = true;
            view.viewToShow = viewToShow;
        }
    }
    private bool TryGetVerificationView(out VerificationView.Views viewId)
    {
        var result = _result.Read();
        var isUnpaid = result.applicationUnpaid;
        var isExpired = result.applicationExpired;
        var isOtherInvalid = result.applicationHashInvalid || result.deviceInvalid || result.lastTimeLoginInvalid;
        viewId = default;
        if (isUnpaid)
        {
            viewId = VerificationView.Views.Purchase;
            return true;
        }
        else if (isExpired)
        {
            viewId = VerificationView.Views.Expired;
            return true;
        }
        else if (isOtherInvalid)
        {
            viewId = VerificationView.Views.Warning;
            return true;
        }
        return false;
    }
}

