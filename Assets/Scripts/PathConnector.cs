using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PathConnector : MonoBehaviour
{
    public RectTransform from;   // LevelNode_1
    public RectTransform to;     // LevelNode_2
    public float thickness = 12f;

    RectTransform _rect;

    void Awake() { _rect = GetComponent<RectTransform>(); }

    void LateUpdate()
    {
        if (!_rect || !from || !to) return;

        // Both nodes should share the same parent space (NodesArea).
        Vector2 a = from.anchoredPosition;
        Vector2 b = to.anchoredPosition;

        Vector2 dir = b - a;
        float dist = dir.magnitude;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Make the image stretch from 'from' to 'to'
        _rect.pivot = new Vector2(0f, 0.5f);          // left-center
        _rect.sizeDelta = new Vector2(dist, thickness);
        _rect.anchoredPosition = a;                   // start at 'from'
        _rect.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
