using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropTarget : MonoEventDispatcher, IDropHandler
{
    public static readonly string Dropped = "DropTarget.Dropped";

    [SerializeField] private LayoutGroup _container;

    public void OnDrop(PointerEventData pointerEventData)
    {
        var droppedDraggable = pointerEventData.pointerDrag.GetComponent<Draggable>();
        if (droppedDraggable == null)
            return;

        droppedDraggable.LayoutGroup = _container;
        droppedDraggable.transform.SetParent(_container.transform, false);

        //Dispatch(new EventObject
        //{
        //    Sender = this,
        //    Type = Dropped,
        //    Data = pointerEventData.pointerDrag
        //});
    }
}
