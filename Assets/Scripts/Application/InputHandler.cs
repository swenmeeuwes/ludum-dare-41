using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class InputHandler : MonoSingleton<InputHandler>
{
    [SerializeField] private InputContext _inputContext;

    public Vector2 Axis { get; private set; }
    public Vector2 RawAxis { get; private set; }

    public Touch[] Touches { get; private set; }

    public Vector2 MousePosition { get; private set; }
    public Vector2 MouseScrollDelta { get; private set; }

    public Vector2 GenericInputPosition {
        get
        {
            if (Input.touchCount > 0)
                return Touches[0].position;
            return MousePosition;
        }
    }

    private List<string> Actions { get; set; }
    private List<string> ActionsDown { get; set; }
    private List<string> ActionsUp { get; set; }

    public bool GetAction(string actionLiteral)
    {
        return Actions.Any(action => action == actionLiteral);
    }

    public bool GetActionDown(string actionLiteral)
    {
        return ActionsDown.Any(action => action == actionLiteral);
    }

    public bool GetActionUp(string actionLiteral)
    {
        return ActionsUp.Any(action => action == actionLiteral);
    }

    public void EatActionEvent(string actionLiteral)
    {
        Actions.Remove(actionLiteral);
        ActionsDown.Remove(actionLiteral);
        ActionsUp.Remove(actionLiteral);
    }

    private void Awake()
    {
        DefineSingleton(this);

        Actions = new List<string>();
        ActionsDown = new List<string>();
        ActionsUp = new List<string>();
    }

    private void Update()
    {
        Actions.Clear();
        ActionsDown.Clear();
        ActionsUp.Clear();

        Axis = new Vector2(Input.GetAxis(InputAxesLiterals.Horizontal), Input.GetAxis(InputAxesLiterals.Vertical));
        RawAxis = new Vector2(Input.GetAxisRaw(InputAxesLiterals.Horizontal), Input.GetAxisRaw(InputAxesLiterals.Vertical));

        Touches = Input.touches;

        MousePosition = Input.mousePosition;
        MouseScrollDelta = Input.mouseScrollDelta;

        _inputContext.InputItems.ForEach(ResolveInput);
    }

    private void ResolveInput(InputMap inputMap)
    {
        // Keyboard
        if (Input.GetKeyDown(inputMap.KeyCode))
        {
            ActionsDown.Add(inputMap.Action);
            return;
        }

        if (Input.GetKeyUp(inputMap.KeyCode))
        {
            ActionsUp.Add(inputMap.Action);
            return;
        }

        if (Input.GetKey(inputMap.KeyCode))
        {
            Actions.Add(inputMap.Action);
            return;
        }

        // Touch
        if (inputMap.Touch && Input.touchCount > 0)
        {
            var firstTouch = Input.GetTouch(0);
            switch (firstTouch.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Actions.Add(inputMap.Action);
                    return;
                case TouchPhase.Began:
                    ActionsDown.Add(inputMap.Action);
                    return;
                case TouchPhase.Ended:
                    ActionsUp.Add(inputMap.Action);
                    return;
            }
        }

        // Mouse
        if (Input.GetMouseButtonDown((int) inputMap.MouseButton))
        {
            ActionsDown.Add(inputMap.Action);
            return;
        }

        if (Input.GetMouseButtonUp((int) inputMap.MouseButton))
        {
            ActionsUp.Add(inputMap.Action);
            return;
        }

        if (Input.GetMouseButton((int)inputMap.MouseButton))
        {
            Actions.Add(inputMap.Action);
            return;
        }
    }
}
