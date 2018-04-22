using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropTarget : MonoEventDispatcher, IDropHandler
{
    public static readonly string Dropped = "DropTarget.Dropped";
    public static readonly string PileChanged = "DropTarget.PileChanged";

    [SerializeField] private LayoutGroup _container;

    private readonly List<Draggable> _draggables = new List<Draggable>();
    public List<Draggable> Draggables { get { return _draggables; } }

    public void OnDrop(PointerEventData pointerEventData)
    {
        var droppedDraggable = pointerEventData.pointerDrag.GetComponent<Draggable>();
        if (droppedDraggable == null)
            return;

        droppedDraggable.OnEndDrag();

        Register(droppedDraggable);

        Dispatch(new EventObject
        {
            Sender = this,
            Type = Dropped,
            Data = pointerEventData.pointerDrag
        });
    }

    public void Register(Draggable draggable)
    {
        draggable.CurrentDropTarget.Remove(draggable);
        draggable.CurrentDropTarget = this;
        draggable.LayoutGroup = _container;
        draggable.transform.SetParent(_container.transform, false);

        _draggables.Add(draggable);

        Dispatch(new EventObject
        {
            Sender = this,
            Type = PileChanged,
            Data = Draggables
        });
    }

    public void Remove(Draggable draggable)
    {
        _draggables.Remove(draggable);        

        Dispatch(new EventObject
        {
            Sender = this,
            Type = PileChanged,
            Data = Draggables
        });
    }    
}
