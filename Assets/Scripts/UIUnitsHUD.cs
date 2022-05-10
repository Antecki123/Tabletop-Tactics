using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUnitsHUD : MonoBehaviour
{
    [Header("Unit References")]
    [SerializeField] private Unit unit;

    [Header("Panel Components")]
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private TextMeshProUGUI panelHealth;
    [SerializeField] private TextMeshProUGUI panelActionPoints;

    [SerializeField] private string unitName;
    [SerializeField] private float unitHealth;
    [SerializeField] private float unitActionPoints;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        panelName.text = unitName;
        panelHealth.text = unitHealth.ToString();
        panelActionPoints.text = unitActionPoints.ToString();
    }

    private void LateUpdate() => transform.LookAt(transform.position + mainCamera.transform.forward);
}
