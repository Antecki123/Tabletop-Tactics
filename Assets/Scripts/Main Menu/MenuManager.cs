using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public async void LoadNewGame()
    {
        var loadScene = SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Single);

        while (!loadScene.isDone)
            await Task.Yield();

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScreen"));

        var skirmishScene = SceneManager.LoadSceneAsync("Battlefield", LoadSceneMode.Additive);
        var UIScene = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);

        skirmishScene.allowSceneActivation = false;
        UIScene.allowSceneActivation = false;

        while (!skirmishScene.isDone && !UIScene.isDone)
        {
            await Task.Yield();
            print("LOADING...");

            if (skirmishScene.progress >= 0.9f && UIScene.progress >= 0.9f)
            {
                skirmishScene.allowSceneActivation = true;
                UIScene.allowSceneActivation = true;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battlefield"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("LoadingScreen"));
    }
}