using UnityEngine;

public class WiggleText : MonoBehaviour
{
    public float speed = 3f;
    public float amount = 5f;

    void Update()
    {
        float r = Mathf.Sin(Time.time * speed) * amount;
        transform.rotation = Quaternion.Euler(0, 0, r);
    }
}
