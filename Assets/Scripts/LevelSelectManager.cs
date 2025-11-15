using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public LevelNodeUI[] nodes;              // assign in order (1,2,3)
    public string startScreenScene = "start screen"; // scene name

    void Start()
    {
        // Default state: Level 1 unlocked, others locked
        for (int i = 0; i < nodes.Length; i++)
        {
            int level = i + 1;

            // First run defaults:
            bool defaultUnlocked = (i == 0); // only level 1 unlocked
            bool unlocked = PlayerPrefs.GetInt($"lvl{level}_unlocked", defaultUnlocked ? 1 : 0) == 1;

            nodes[i].unlocked = unlocked;
            nodes[i].ApplyState();
        }
    }

    // Back button calls this
    public void GoBack()
    {
        SceneManager.LoadScene(startScreenScene);
    }

    // Call this from gameplay when a level is completed
    public static void UnlockNextLevel(int justFinishedLevelIndex1Based)
    {
        string key = $"lvl{justFinishedLevelIndex1Based + 1}_unlocked";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }
}
