using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[Serializable]
public class InputMap
{
    public string Action;
    public KeyCode KeyCode;
    public bool Touch;
    public MouseButton MouseButton;
}

[CreateAssetMenu(menuName = "Context/Input Context", fileName = "Input Context")]
public class InputContext : ScriptableObject {
    public List<InputMap> InputItems = new List<InputMap>();
}
