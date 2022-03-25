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

        if (Input.GetKey("w") || Input.GetKey("up"))
            position.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey("s") || Input.GetKey("down"))
            position.z += panSpeed * Time.deltaTime;
        if (Input.GetKey("a") || Input.GetKey("left"))
            position.x += panSpeed * Time.deltaTime;
        if (Input.GetKey("d") || Input.GetKey("right"))
            position.x -= panSpeed * Time.deltaTime;

        position.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -5f, 5f);
        position.y = Mathf.Clamp(position.y, 3f, 10f);
        position.z = Mathf.Clamp(position.z, -8f, 12f);
        transform.position = position;
    }
}