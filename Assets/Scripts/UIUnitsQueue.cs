using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitsQueue : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private QueueBehavior queueBehavior;

    private int UIQueueLenght = 5;
    [SerializeField] private List<Image> unitImages = new();

    private void Start()
    {
        Invoke("CreateUnitImages", 4f);
        InvokeRepeating("UpdateUIQueue", 5f, .1f);
    }

    private void CreateUnitImages()
    {
        for (int i = 0; i < UIQueueLenght; i++)
        {
            var UIpriorityQueueObject = new GameObject("Slot " + i);
            UIpriorityQueueObject.transform.SetParent(this.transform, true);

            var UIpriorityQueueImage = UIpriorityQueueObject.AddComponent<Image>();
            UIpriorityQueueImage.sprite = queueBehavior.UnitsQueue[i].UnitBaseStats.unitImage;

            unitImages.Add(UIpriorityQueueImage);
        }
    }

    private void UpdateUIQueue()
    {
        // TODO: problem when array is shorter than UI panel count
        for (int i = 0; i < unitImages.Count; i++)
        {
            if (queueBehavior.UnitsQueue[i])
            {
                unitImages[i].sprite = queueBehavior.UnitsQueue[i].UnitBaseStats.unitImage;
                unitImages[i].enabled = true;
            }
            else
                unitImages[i].enabled = false;
        }
    }
}