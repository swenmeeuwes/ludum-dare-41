using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonController : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Text _endTurnButtonText;
    [SerializeField] private Player _player;
    [SerializeField] private ExecuteHolder _executeHolder;

    private void Update()
    {
        var phaseManager = PhaseManager.Instance;
        var staminaNetto = phaseManager.PlayerStamina - phaseManager.CurrentStaminaCost;
        _endTurnButton.interactable = !(staminaNetto < 0 || _player.Movement.IsMoving || phaseManager.CurrentPhase != Phase.Player);

        if (_endTurnButton.interactable)
            _endTurnButtonText.text = _executeHolder.Cards.Length == 0 ? "Shuffle" : "End turn";
        else
            _endTurnButtonText.text = "...";

        //Debug.Log(string.Format("{0}  {1}   {2}", staminaNetto < 0, _player.Movement.IsMoving, phaseManager.CurrentPhase != Phase.Player));
    }

    public void Handle()
    {
        _endTurnButtonText.text = "...";

        if (_executeHolder.Cards.Length == 0)
        {
            // Shuffle
            PhaseManager.Instance.ShuffleCards();
            SoundManager.Instance.Play(Sound.Shuffle);
        }
        else
        {
            // Execute cards
            PhaseManager.Instance.ExecuteCards();
        }
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
