using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Call this from Btn_LevelSelect
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect"); // make sure the scene exists & is in Build Settings
    }

    // Call this from Btn_Quit
    public void QuitGame()
    {
        Debug.Log("Quit game requested.");
        Application.Quit();
    }
}
