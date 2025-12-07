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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel == null) return;

            bool show = !pausePanel.activeSelf;
            pausePanel.SetActive(show);

            // Pause game when panel is open, resume when closed
            Time.timeScale = show ? 0f : 1f;
        }
    }
}
