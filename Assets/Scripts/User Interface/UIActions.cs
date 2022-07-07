using UnityEngine;

public class UIActions : MonoBehaviour
{
    [System.Serializable]
    public struct ActionsList
    {
        public string name;
        public GameObject action;
        public BoolVariable available;
    }

    [SerializeField] private ActionsList[] actions;

    public void UpdateActions()
    {
        foreach (var a in actions)
        {
            a.action.SetActive(a.available.value);
        }
    }
}