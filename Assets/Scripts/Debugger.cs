using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private UnitActions actions;
    [Space]
    [SerializeField] private TextMeshProUGUI activePlayerHUD;

    private void Awake()
    {
        actions = FindObjectOfType<UnitActions>();
    }


    private void Update()
    {
        if (actions.ActiveUnit)
            activePlayerHUD.text = $"Active Unit: {actions.ActiveUnit.name}";
    }
}
