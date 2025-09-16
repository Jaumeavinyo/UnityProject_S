using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class SmartCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public FSM_CharMov player;
    public EntityManager entityManager;

    Camera camera;

    /*una linea vertical en mitad de la pantalla, el jugador se mantiene siempre que se mueve a la izquierda
     este look ahead basado en movement direction. dar un poco de tiempo moviendose en direccion contraria para que la posicion de la camara cambie y estes a la derecha del medio
     
    una linea horizontal a 2 tercios de la pantalla pos si el jugador sube tanto ajustar la altura
     */
    /*
     Mathf.SmoothDamp() (Smooth Deceleration)
Gradually slows down as it approaches the target (e.g., camera follow).
     */
    private Vector3 originalPosBeforeShake;
    Transform targetTransform;
    public float followStrength = 2f;   // how fast the camera accelerates towards the target
    public float maxSpeed = 15f;        // optional speed cap
    private Coroutine shakeRoutine;
    public float cameraForwardOffset;
    public float cameraUpOffset;
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
        Vector3 cameraPos = transform.position;

        // Distance between player and camera
        Vector3 offset = targetPos - cameraPos;
        Vector3 velocity = offset * followStrength;

        // Clamp speed 
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Apply movement
        transform.position += velocity * Time.deltaTime;
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
        float elapsed = 0f;
        Vector3 targetOffset = Vector3.zero;

        while (elapsed < duration)
        {
            // If close enough to target offset, pick a new random offset
            if ((transform.localPosition - (originalPosBeforeShake + targetOffset)).sqrMagnitude < 0.01f)
            {
                float x = Random.Range(-left, right);   // negative = left, positive = right
                float y = Random.Range(-down, up);      // negative = down, positive = up
                targetOffset = new Vector3(x, y, 0f);
            }

            // Smooth movement toward the target offset
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                originalPosBeforeShake + targetOffset,
                shakeSpeed * Time.deltaTime
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

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
