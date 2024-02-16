using System;
using UnityEngine;
using UnityEngine.Events;

internal class EditionButtonsView : MonoBehaviour
{
    [SerializeField] private EditionButton[] _editionButtons;
    [SerializeField] private EditionButton _editionButtonPrefab;
    [SerializeField] private ExactPositionLayout _editionButtonsLayout;
    internal void AddEditionButton(int index, bool enable, bool isUnpaid, bool isExpired, string name, UnityAction<int> editionButton_onClick)
    {
        var state = EditionButton.VerificationState.Normal;
        if (!enable) state = EditionButton.VerificationState.Unenabled;
        else if (isUnpaid) state = EditionButton.VerificationState.Unpaid;
        else if (isExpired) state = EditionButton.VerificationState.Expired;

        _editionButtons[index] = Instantiate(_editionButtonPrefab);
        _editionButtons[index].clickButton.AddListener(editionButton_onClick);
        //_editionButtons[index].Initialize(index, name, state);

        _editionButtonsLayout.Layout(index, _editionButtons[index].transform);
    }

    internal void CleanUp()
    {
        throw new NotImplementedException();
    }
}