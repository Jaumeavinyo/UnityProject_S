using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    [Range(0.2f, 0.6f)] public float innerZoneWidth = 0.4f;
    [Range(0.2f, 0.6f)] public float innerZoneHeight = 0.4f;
    [Range(1f, 20f)] public float maxFollowSpeed = 10f;
    [Range(0f, 1f)] public float accelerationCurve = 0.5f;

    private Camera cam;
    private Vector3 currentVelocity;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (!player) return;

        Vector3 viewportPos = cam.WorldToViewportPoint(player.position);
        Vector3 targetPos = transform.position;

        // Calculate normalized distance from center (0-1 range)
        float xDistFromCenter = Mathf.Abs(viewportPos.x - 0.5f) * 2;
        float yDistFromCenter = Mathf.Abs(viewportPos.y - 0.5f) * 2;

        // Calculate follow strength (0 when centered, 1 at screen edge)
        float xFollowStrength = Mathf.Clamp01(
            (xDistFromCenter - innerZoneWidth) / (1 - innerZoneWidth)
        );

        float yFollowStrength = Mathf.Clamp01(
            (yDistFromCenter - innerZoneHeight) / (1 - innerZoneHeight)
        );

        // Apply non-linear response curve
        xFollowStrength = Mathf.Pow(xFollowStrength, 1 + accelerationCurve);
        yFollowStrength = Mathf.Pow(yFollowStrength, 1 + accelerationCurve);

        // Only follow if outside inner zone
        if (xDistFromCenter > innerZoneWidth)
        {
            targetPos.x = player.position.x;
            currentVelocity.x = xFollowStrength * maxFollowSpeed;
        }

        if (yDistFromCenter > innerZoneHeight)
        {
            targetPos.y = player.position.y;
            currentVelocity.y = yFollowStrength * maxFollowSpeed;
        }

        // Smooth movement with velocity control
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref currentVelocity,
            0.1f,
            maxFollowSpeed
        );
    }
}
