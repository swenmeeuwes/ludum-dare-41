using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PhaseText : MonoBehaviour
{
    private Text _textField;

    private void Start()
    {
        _textField = GetComponent<Text>();
        _textField.text = PhaseManager.Instance.CurrentPhase.ToString();

        PhaseManager.Instance.AddEventListener(PhaseManager.PhaseChanged, OnPhaseChanged);
    }

    private void OnDestroy()
    {
        if (PhaseManager.Instance != null)
            PhaseManager.Instance.RemoveEventListener(PhaseManager.PhaseChanged, OnPhaseChanged);
    }

    private void OnPhaseChanged(EventObject eventObject)
    {
        _textField.text = eventObject.Data.ToString();
    }
}
