using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollResultsPanel : MonoBehaviour
{
    public static RollResultsPanel instance;

    [Header("Component References")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject diceResultPrefab;
    [SerializeField] private Sprite[] diceImages = new Sprite[6];

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public async void ShowResult(int result, string name)
    {
        resultPanel.SetActive(true);

        var dice = Instantiate(diceResultPrefab, resultPanel.transform);
        dice.transform.SetParent(resultPanel.transform, false);

        dice.transform.Find("Image").GetComponent<Image>().sprite = diceImages[result - 1];
        dice.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = name;
        Destroy(dice, 5f);

        await Task.Delay(5000);
        resultPanel.SetActive(false);
    }
}