using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 30)] private float panSpeed = 20f;
    [SerializeField, Range(1, 30)] private float scrollSpeed = 20f;

    private void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            position.z += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            position.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            position.x -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            position.x += panSpeed * Time.deltaTime;

        position.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, 0f, 20f);
        position.y = Mathf.Clamp(position.y, 3f, 12f);
        position.z = Mathf.Clamp(position.z, -3f, 13f);
        transform.position = position;
    }
}