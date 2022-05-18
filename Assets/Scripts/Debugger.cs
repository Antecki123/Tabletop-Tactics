using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI activePlayerHUD;

    private UnitActions actions;

    private void Awake()
    {
        actions = FindObjectOfType<UnitActions>();
    }


    private void Update()
    {
        if (actions)
            activePlayerHUD.text = $"Active Unit: {actions.ActiveUnit.name}";
    }
}
