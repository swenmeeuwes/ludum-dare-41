using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LayoutGroup LayoutGroup;
    public Transform[] PossibleTargets;
    public bool ShouldReturn = true;

    private MaskableGraphic _graphic;

    public DropTarget CurrentDropTarget { get; set; }

    private bool _isBeingDragged;
    private bool IsBeingDragged {
        set
        {
            _isBeingDragged = value;
            if (LayoutGroup)
                LayoutGroup.enabled = !value;
        }
        get { return _isBeingDragged; }
    }    
    private Vector3 _startPosition;

    protected virtual void Start()
    {
        _graphic = GetComponent<MaskableGraphic>();
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        IsBeingDragged = true;        

        _startPosition = transform.position;

        _graphic.raycastTarget = false;
        ShowTargets(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;        
    }

    public void OnEndDrag(PointerEventData pointerEventData = null)
    {
        IsBeingDragged = false;

        if (ShouldReturn)
            transform.position = _startPosition;

        _graphic.raycastTarget = true;
        ShowTargets(false);
    }

    private void ShowTargets(bool show)
    {
        foreach (var maskableGraphic in PossibleTargets)
        {
            maskableGraphic.gameObject.SetActive(show);
        }
    }
}
