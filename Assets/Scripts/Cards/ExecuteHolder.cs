using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DropTarget))]
public class ExecuteHolder : MonoBehaviour
{
    private DropTarget _dropTarget;

    public Card[] Cards
    {
        get { return _dropTarget.Draggables.Cast<Card>().ToArray(); }
    }

    private void Awake()
    {
        _dropTarget = GetComponent<DropTarget>();
    }

    private void Start()
    {
        _dropTarget.AddEventListener(DropTarget.PileChanged, OnPileChanged);
    }

    private void OnDestroy()
    {
        if (_dropTarget != null)
            _dropTarget.RemoveEventListener(DropTarget.PileChanged, OnPileChanged);
    }

    private void OnPileChanged(EventObject eventObject)
    {
        var staminaCostOfPile = ((List<Draggable>)eventObject.Data).Select(draggable => draggable.GetComponent<Card>()).Sum(card => card.Cost);

        PhaseManager.Instance.CurrentStaminaCost = staminaCostOfPile;
    }
}
