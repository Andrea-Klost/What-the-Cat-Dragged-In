using UnityEngine;
using UnityEngine.UI;

public class FlickerText : MonoBehaviour
{
    public Text target;
    public float intensity = 0.4f; 
    public float speed = 3f;

    void Update()
    {
        if (!target) return;

        float a = 1f - (Mathf.PerlinNoise(Time.time * speed, 0f) * intensity);
        var c = target.color;
        target.color = new Color(c.r, c.g, c.b, a);
    }
}
