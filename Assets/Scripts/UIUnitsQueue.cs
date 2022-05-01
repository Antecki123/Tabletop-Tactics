using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitsQueue : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private QueueBehavior queueBehavior;
    [SerializeField] private GameObject unitImageSlot;

    private int UIQueueLenght = 5;

    private void Start()
    {
        Invoke("CreateUnitImage", .5f);
    }

    private void CreateUnitImage()
    {
        for (int i = 0; i < UIQueueLenght; i++)
        {
            var unitPriorityObject = Instantiate(new GameObject(queueBehavior.UnitsQueue[i].name));
            unitPriorityObject.transform.SetParent(this.transform, false);


            var priorityImage = unitPriorityObject.AddComponent<Image>();
            priorityImage.sprite = queueBehavior.UnitsQueue[i].unitBaseStats.unitImage;
        }
    }
}