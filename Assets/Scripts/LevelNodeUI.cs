using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNodeUI : MonoBehaviour
{
    [Header("Setup")]
    public string sceneName = "Level1"; // set uniquely per node
    public Text label;                  // the child "Label"
    public GameObject lockOverlay;      // the child "Lock"
    public Button button;               // this node's Button

    [Header("State")]
    public bool unlocked = false;

    public void ApplyState()
    {
        if (lockOverlay) lockOverlay.SetActive(!unlocked);
        if (button) button.interactable = unlocked;

        var img = GetComponent<Image>();
        if (img)
        {
            // orange when unlocked, gray when locked
            img.color = unlocked
                ? new Color(1f, 0.6f, 0.2f, 1f)
                : new Color(0.6f, 0.6f, 0.6f, 1f);
        }
    }

    public void OnClick()
    {
        if (!unlocked) return;
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene("Level1");
    }
}
