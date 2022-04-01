using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack
{
    [Header("Component References")]
    private Camera mainCamera;

    public MeleeAttack() => mainCamera = Camera.main;

    public void UpdateAction()
    {
        Debug.Log("Melee Attack");
    }
}
