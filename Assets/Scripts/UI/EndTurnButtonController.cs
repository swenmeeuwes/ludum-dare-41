using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonController : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Player _player;
    [SerializeField] private ExecuteHolder _executeHolder;

    private void Update()
    {
        var phaseManager = PhaseManager.Instance;
        var staminaNetto = phaseManager.PlayerStamina - phaseManager.CurrentStaminaCost;
        _endTurnButton.interactable = !(staminaNetto < 0 || _player.Movement.IsBusy || phaseManager.CurrentPhase != Phase.Player || _executeHolder.Cards.Length == 0);

        //Debug.Log(string.Format("{0}  {1}   {2}", staminaNetto < 0, _player.Movement.IsBusy, phaseManager.CurrentPhase != Phase.Player));
    }

    //private void Start()
    //{
    //    PhaseManager.Instance.AddEventListener(PhaseManager.PlayerStaminaChanged, OnStaminaRelatedChanged);
    //    PhaseManager.Instance.AddEventListener(PhaseManager.CurrentStaminaCostChanged, OnStaminaRelatedChanged);
    //}

    //private void OnDestroy()
    //{
    //    if (PhaseManager.Instance != null)
    //    {
    //        PhaseManager.Instance.RemoveEventListener(PhaseManager.PlayerStaminaChanged, OnStaminaRelatedChanged);
    //        PhaseManager.Instance.RemoveEventListener(PhaseManager.CurrentStaminaCostChanged, OnStaminaRelatedChanged);
    //    }
    //}

    //private void OnStaminaRelatedChanged(EventObject eventObject)
    //{
    //    var phaseManager = (PhaseManager)eventObject.Sender;
    //    var staminaNetto = phaseManager.PlayerStamina - phaseManager.CurrentStaminaCost;
    //    var enable = staminaNetto >= 0;
    //    foreach (var item in _disableWhenNegativeStamina)
    //    {
    //        item.interactable = enable;
    //    }
    //}
}
