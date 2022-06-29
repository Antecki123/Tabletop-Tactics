using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public Collider[] Colliders { get; private set; }

    private void Start() => Colliders = FindObjectsOfType<Collider>();
}