using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 30)] private float panSpeed = 20f;
    [SerializeField, Range(1, 30)] private float scrollSpeed = 20f;

    [SerializeField] private QueueBehavior queueBehavior;

    private Unit activeUnit;
    private Unit lastActiveUnit;

    private void Update()
    {
        Vector3 position = transform.position;

        // Start this on new activ unit selected
        //StartCoroutine(SetCameraOnNewActiveUnit());

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            position.z += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            position.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            position.x -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            position.x += panSpeed * Time.deltaTime;

        position.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * 100f * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -10f, 20f);
        position.y = Mathf.Clamp(position.y, 3f, 15f);
        position.z = Mathf.Clamp(position.z, -10f, 20f);
        transform.position = position;
    }

    private IEnumerator SetCameraOnNewActiveUnit()
    {
        activeUnit = queueBehavior.UnitsQueue[0];
        if (activeUnit && !activeUnit.Equals(lastActiveUnit))
        {
            Vector3 positionOffset = new(0f, 10f, -5f);
            Vector3 cameraPosition = activeUnit.transform.position + positionOffset;

            lastActiveUnit = activeUnit;

            while (Vector3.Distance(transform.position, cameraPosition) >= .5f)
            {
                transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime);
                yield return null;
            }
        }
    }
}