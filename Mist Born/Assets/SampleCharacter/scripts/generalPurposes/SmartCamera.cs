using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class SmartCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public FSM_CharMov player;
    public EntityManager entityManager;
    public EnemyBoss boss;
    Camera camera;
    private Vector3 originalPosBeforeShake;
    Transform targetTransform;
    public float followStrength = 2f;   // how fast the camera accelerates towards the target
    public float maxSpeed = 15f;        // optional speed cap
    public Coroutine shakeRoutine;
    public float cameraForwardOffset;
    public float cameraUpOffset;

    // Store velocity for SmoothDamp
    private Vector3 smoothVelocity = Vector3.zero;

    // How long it takes to catch up (lower = snappier, higher = floaty)
    public float smoothTime = 0.2f;

    // Distance threshold where we stop smoothing and snap directly
    public float snapDistance = 0.05f;

    void Start()
    {
        camera = GetComponent<Camera>();


    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position;
        DebugDraws();
        
    }

    private void LateUpdate()
    {
        Vector3 targetPos = GetCameraTarget();

        springCameraMovement(targetPos);


    }

    public Vector3 GetCameraTarget()
    {
        Vector3 target = player.transform.position;
        if (boss.playerDetected /*&& boss.entranceDone*/)
        {
            target = new Vector3((player.transform.position.x + boss.transform.position.x) / 2f, target.y, target.z);
        }
        
        
        if (player.rigidBody.linearVelocityX > 0.0f)
        {
            target = new Vector3(player.transform.position.x + cameraForwardOffset, player.transform.position.y, transform.position.z);
        }
        else if (player.rigidBody.linearVelocityX < 0.0f)
        {
            target = new Vector3(player.transform.position.x - cameraForwardOffset, player.transform.position.y, transform.position.z);
        }
        target.y = player.transform.position.y + cameraUpOffset;
        return target;
    }
    public void springCameraMovement(Vector3 targetPos)
    {
        //Vector3 cameraPos = transform.position;

        //// Distance between player and camera
        //Vector3 offset = targetPos - cameraPos;
        //Vector3 velocity = offset * followStrength;

        //// Clamp speed 
        //if (velocity.magnitude > maxSpeed)
        //    velocity = velocity.normalized * maxSpeed;

        //// Apply movement
        //transform.position += velocity * Time.deltaTime;
        Vector3 currentPos = transform.position;
        Vector3 offset = targetPos - currentPos;

        // If close enough, snap instantly (prevents slow creep)
        if (offset.magnitude < snapDistance)
        {
            transform.position = targetPos;
            smoothVelocity = Vector3.zero; // reset velocity to avoid jitter
            return;
        }

        // SmoothDamp interpolates smoothly with velocity, clamped by maxSpeed
        transform.position = Vector3.SmoothDamp(
            currentPos,
            targetPos,
            ref smoothVelocity,
            smoothTime,
            maxSpeed
        );
    }

    public void DirectionalShake(float duration, float shakeSpeed,float left, float right, float up, float down)
    {
        // Stop previous shake if running
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        originalPosBeforeShake = transform.position;
        shakeRoutine = StartCoroutine(DoDirectionalShake(duration, shakeSpeed, left, right, up, down));
    }

    private IEnumerator DoDirectionalShake(float duration, float shakeSpeed, float left, float right, float up, float down)
    {

        //float elapsed = 0f;
        //Vector3 targetOffset = Vector3.zero;

        //while (elapsed < duration)
        //{
        //    // If close enough to target offset, pick a new random offset
        //    if ((transform.localPosition - (originalPosBeforeShake + targetOffset)).sqrMagnitude < 0.10f)
        //    {
        //        float x = Random.Range(-left, right);   // negative = left, positive = right
        //        float y = Random.Range(-down, up);      // negative = down, positive = up
        //        targetOffset = new Vector3(x, y, 0f);
        //        Debug.Log("target");
        //    }

        //    // Smooth movement toward the target offset
        //    transform.localPosition = Vector3.MoveTowards(
        //        transform.localPosition,
        //        originalPosBeforeShake + targetOffset,
        //        shakeSpeed * Time.deltaTime
        //    );

        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}
        float elapsed = 0f;
        Vector3 basePosition = originalPosBeforeShake;

        while (elapsed < duration)
        {
            // Apply random offset every frame for more intense shaking
            float x = Random.Range(-left, right);
            float y = Random.Range(-down, up);

            // Optional: Reduce intensity over time
            float progress = elapsed / duration;
            float currentIntensity = 1f - progress;

            transform.localPosition = basePosition + new Vector3(x, y, 0f) * currentIntensity;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosBeforeShake;

    }

    private Vector3 GetRandomOffset(float left, float right, float up, float down)
    {
        float x = Random.Range(-left, right);
        float y = Random.Range(-down, up);
        return new Vector3(x, y, 0f);
    }

    void DebugDraws()
    {
        Color lineColor = Color.red;
        float orthoHeight = camera.orthographicSize;
        float orthoWidth = orthoHeight * camera.aspect;

        Vector3 middle = camera.transform.position;
        Vector3 start = middle + Vector3.up * orthoHeight;
        Vector3 end = middle - Vector3.up * orthoHeight;

        Debug.DrawLine(start, end, lineColor, 0.1f);
    }
}
