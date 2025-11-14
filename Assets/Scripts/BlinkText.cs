using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    public Text target;
    public float speed = 1.5f;

    void Update()
    {
        if (!target) return;
        float a = 0.5f + 0.5f * Mathf.PingPong(Time.time * speed, 1f);
        var c = target.color;
        target.color = new Color(c.r, c.g, c.b, a);
    }
}
