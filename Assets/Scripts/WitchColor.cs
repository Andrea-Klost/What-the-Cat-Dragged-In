using UnityEngine;
using UnityEngine.UI;

public class WitchColor : MonoBehaviour
{
    public Text target;

    [Header("Palette & Timing")]
    public float cycleSpeed = 0.8f;       // how fast colors blend
    public float randomJitter = 0.15f;    // how much color randomness is allowed
                                          

    [Header("Magic Flash")]
    public float flashChance = 0.05f;     // probability per second of triggering a flash
    public float flashIntensity = 1.35f;  // brightness multiplier when flashing
    public float flashDuration = 0.2f;    // how long flash lasts

    // colors
    private Color[] palette = new Color[]
    {
        new Color(0.65f, 0.45f, 1.0f), // purple
        new Color(0.45f, 0.95f, 0.65f), // green
        new Color(1.00f, 0.85f, 0.45f), // gold
        new Color(0.55f, 0.35f, 0.95f), // deep indigo
    };

    int index = 0;
    int nextIndex = 1;
    float t = 0f;

    bool flashing = false;
    float flashEndTime = 0f;

    void Update()
    {
        if (!target) return;

        // NORMAL COLOR TRANSITIONS 
        t += Time.deltaTime * cycleSpeed;
        Color baseColor = Color.Lerp(palette[index], palette[nextIndex], t);

        if (t >= 1f)
        {
            t = 0f;
            index = nextIndex;
            nextIndex = (nextIndex + 1) % palette.Length;
        }

        // RANDOM COLOR VARIATION
        float rJ = Random.Range(-randomJitter, randomJitter);
        float gJ = Random.Range(-randomJitter, randomJitter);
        float bJ = Random.Range(-randomJitter, randomJitter);

        baseColor = new Color(
            Mathf.Clamp01(baseColor.r + rJ),
            Mathf.Clamp01(baseColor.g + gJ),
            Mathf.Clamp01(baseColor.b + bJ)
        );

        // FLASH TRIGGER 
        if (!flashing)
        {
            float chanceThisFrame = flashChance * Time.deltaTime;
            if (Random.value < chanceThisFrame)
            {
                flashing = true;
                flashEndTime = Time.time + flashDuration;
            }
        }

        // APPLY FLASH
        if (flashing)
        {
            baseColor *= flashIntensity;

            if (Time.time > flashEndTime)
            {
                flashing = false;
            }
        }

        target.color = baseColor;
    }
}
