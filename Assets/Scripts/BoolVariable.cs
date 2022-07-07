using UnityEngine;

[CreateAssetMenu(fileName = "New Bool", menuName = "Scriptable Objects/Bool Variable")]
public class BoolVariable : ScriptableObject
{
    public bool value;
    [TextArea] public string discription;
}