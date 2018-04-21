using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class StaminaLeftText : MonoBehaviour
{
    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();

        _text.color = _positiveColor;
        PhaseManager.Instance.AddEventListener(PhaseManager.PlayerStaminaChanged, OnStaminaRelatedChanged);
        PhaseManager.Instance.AddEventListener(PhaseManager.CurrentStaminaCostChanged, OnStaminaRelatedChanged);
    }

    private void OnDestroy()
    {
        if (PhaseManager.Instance != null)
        {
            PhaseManager.Instance.RemoveEventListener(PhaseManager.PlayerStaminaChanged, OnStaminaRelatedChanged);
            PhaseManager.Instance.RemoveEventListener(PhaseManager.CurrentStaminaCostChanged, OnStaminaRelatedChanged);
        }
    }

    private void OnStaminaRelatedChanged(EventObject eventObject)
    {
        var phaseManager = (PhaseManager)eventObject.Sender;
        var staminaNetto = phaseManager.PlayerStamina - phaseManager.CurrentStaminaCost;
        _text.color = staminaNetto >= 0 ? _positiveColor : _negativeColor;
        _text.text = staminaNetto.ToString();
    }
}
