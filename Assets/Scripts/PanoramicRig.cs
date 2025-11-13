using UnityEngine;

[DisallowMultipleComponent]
public class PanoramicRig : MonoBehaviour
{
    [Header("Target / Framing")]
    public Transform target;            // (center of diorama)
    public Vector3 lookAtOffset;        // nudge LookAt if subject sits low/high

    [Header("Orbit (Rig yaw around target)")]
    public float orbitSpeedDegPerSec = 8f;   // how fast we spin around Y
    public bool clockwise = true;

    [Header("Rig Radius (distance to target)")]
    public float radius = 8f;           // camera distance from target
    public bool pulseRadius = true;     // gentle breathing zoom
    public float radiusPulseAmplitude = 0.35f;
    public float radiusPulseSpeed = 0.35f;

    [Header("Camera Bob (gentle up/down)")]
    public float bobAmplitude = 0.25f;
    public float bobSpeed = 0.75f;

    [Header("Pitch (tilt)")]
    [Range(-25f, 40f)] public float cameraPitchDeg = 15f;

    [Header("Play / Pause")]
    public bool isPlaying = true;

    [Header("Time")]
    public bool useUnscaledTime = true; // so it animates even when game is paused

    Transform _cam;  // child camera
    float _phase;

    void Reset()
    {
        // Try to auto-find camera child
        if (transform.childCount > 0)
        {
            var c = transform.GetComponentInChildren<Camera>(true);
            if (c) _cam = c.transform;
        }
    }

    void Awake()
    {
        if (!_cam)
        {
            var c = GetComponentInChildren<Camera>(true);
            if (c) _cam = c.transform;
        }
    }

    void LateUpdate()
    {
        if (!_cam || !target) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (isPlaying)
        {
            // 1) Orbit the rig around the target on Y
            float dir = clockwise ? -1f : 1f;
            transform.RotateAround(target.position, Vector3.up, dir * orbitSpeedDegPerSec * dt);

            // 2) Gentle camera bob + optional radius pulse
            _phase += dt;
            float bob = Mathf.Sin(_phase * bobSpeed) * bobAmplitude;

            float r = radius;
            if (pulseRadius)
                r += Mathf.Sin(_phase * radiusPulseSpeed) * radiusPulseAmplitude;

            // 3) Reposition camera at 'r' units behind rig's forward, with pitch
            //    The rig's forward points from target to rig (after RotateAround call)
            Vector3 rigForward = (transform.position - target.position).normalized;
            Vector3 camPos = target.position - rigForward * r;
            camPos.y += bob; // vertical bob in world Y

            _cam.position = camPos;

            // Pitch + look at
            var lookPoint = target.position + lookAtOffset;
            _cam.rotation = Quaternion.LookRotation(lookPoint - _cam.position, Vector3.up)
                          * Quaternion.Euler(cameraPitchDeg, 0f, 0f);
        }
        else
        {
            // Keep looking at the target even if paused
            var lookPoint = target.position + lookAtOffset;
            _cam.rotation = Quaternion.LookRotation(lookPoint - _cam.position, Vector3.up)
                          * Quaternion.Euler(cameraPitchDeg, 0f, 0f);
        }
    }

    // Quick public controls can call from UI
    public void Play()  { isPlaying = true; }
    public void Pause() { isPlaying = false; }
    public void Toggle(){ isPlaying = !isPlaying; }
}
