using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Panel that appears when you press Esc (with the Back button inside).")]
    public GameObject pausePanel;

    void Start()
    {
        // Start unpaused, panel hidden
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pausePanel == null) return;
            if (OrderSystem.LEVEL_ENDED()) return;

            if (pausePanel.activeSelf) {
                ClosePauseMenu();
            }
            else {
                OpenPauseMenu();
            }
        }
    }

    public void ClosePauseMenu() {
        if (pausePanel == null)
            return;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Unpause game
    }

    public void OpenPauseMenu() {
        if (pausePanel == null)
            return;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }
}
