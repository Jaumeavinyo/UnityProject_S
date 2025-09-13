using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

    private float screenCenter;

    void Start()
    {
        camera = GetComponent<Camera>();

       


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        DebugDraws();
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
