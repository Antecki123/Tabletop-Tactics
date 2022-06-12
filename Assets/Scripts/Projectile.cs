using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Action OnTargetHit;

    internal Vector3 startPoint;
    internal Vector3 targetPoint;
    internal float velocity = 15f;
    [Space]
    private List<Vector3> path = new();

    public void Start()
    {
        CalculatePath();
        LaunchMissile();
    }

    private void CalculatePath()
    {
        var resolution = 10;

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / resolution)
        {
            var tangentLineVertex1 = Vector3.Lerp(startPoint, MiddlePosition(), ratio);
            var tangentLineVertex2 = Vector3.Lerp(MiddlePosition(), targetPoint, ratio);

            var bazierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            path.Add(bazierPoint);
        }
        path.Add(targetPoint);
    }

    private Vector3 MiddlePosition()
    {
        var posX = ((targetPoint.x - startPoint.x) / 2) + startPoint.x;
        var posY = Vector3.Distance(startPoint, targetPoint) / 4;
        var posZ = ((targetPoint.z - startPoint.z) / 2) + startPoint.z;

        return new Vector3(posX, posY, posZ);
    }

    private async void LaunchMissile()
    {
        for (int i = 0; i < path.Count; i++)
        {
            while (Vector3.Distance(transform.position, path[i]) > 0)
            {
                var direction = path[i] - transform.position;
                if (direction == Vector3.zero) continue;

                //transform.position = Vector3.MoveTowards(transform.position, path[i], velocity * Time.deltaTime);
                //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, path[i], velocity * Time.deltaTime),
                    Quaternion.LookRotation(direction, Vector3.up));

                await System.Threading.Tasks.Task.Yield();
            }
        }

        OnTargetHit?.Invoke();
        Destroy(gameObject, 1f);
    }
}