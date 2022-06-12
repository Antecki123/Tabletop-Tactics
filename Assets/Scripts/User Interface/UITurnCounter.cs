using TMPro;
using UnityEngine;

public class UITurnCounter : MonoBehaviour
{
    [SerializeField] private FloatVariable counterValue;
    [SerializeField] private TextMeshProUGUI counterText;

    public void UpdateCounter()
    {
        counterText.text = ((int)counterValue.value).ToString();
    }
}