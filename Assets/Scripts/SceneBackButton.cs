using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBackButton : MonoBehaviour
{
    public enum BackTarget
    {
        MainMenu,
        LevelSelect
    }

    [Header("Where should this Back button go?")]
    public BackTarget backTarget = BackTarget.MainMenu;

    [Header("Scene names")]
    public string mainMenuSceneName = "start screen";   // name of main scene 
    public string levelSelectSceneName = "LevelSelect"; // exact scene name

    public void GoBack()
    {
        string sceneToLoad;

        switch (backTarget)
        {
            case BackTarget.MainMenu:
                sceneToLoad = mainMenuSceneName;
                break;

            case BackTarget.LevelSelect:
            default:
                sceneToLoad = levelSelectSceneName;
                break;
        }

        // Just in case we’re coming from a paused state
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneToLoad);
    }
}
