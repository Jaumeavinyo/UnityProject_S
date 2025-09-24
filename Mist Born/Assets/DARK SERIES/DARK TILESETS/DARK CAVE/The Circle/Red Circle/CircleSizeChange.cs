using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class CircleSizeChange : MonoBehaviour
{

    public SmartCamera cam;
    public FSM_CharMov player;
    public GameObject newPos;// where to teleport

    public GameObject paralax;

    float startTime;

    public float waitTimeTofadeIn;

    public float fadeToBlackDuration;


    public float GrowthFactor;
    float currentGrowth = 0f;
    public float maxScale;

  

    public bool portalActivated;
    public bool teleportNow;
    public bool teleported = false;


    public Image screenOverlay;

    void Start()
    {
        portalActivated = false;
        //initialScale = transform.localScale;
        startTime = -1.0f;
    }

    void Update()
    {
        if (!teleportNow)
        {
            if (!portalActivated)
            {
                return;

            }
            else if (portalActivated && startTime == -1.0f)
            {
                startTime = Time.time;
            }
            float sin = Mathf.Abs(Mathf.Sin(Time.time));//value always from 0 to 1      
            float currTime = Time.time - startTime;
            currentGrowth = currTime * GrowthFactor;

            float finalScale = currentGrowth + currentGrowth * sin;

            transform.localScale = new Vector3(finalScale, finalScale, finalScale);
            screenOverlay.color = new Vector4(screenOverlay.color.r, screenOverlay.color.g, screenOverlay.color.b, finalScale / maxScale - 0.3f);
            //Check if growth reached max
            if (finalScale >= maxScale)
            {
                teleportNow = true;
            }
        }
        else if(teleportNow && !teleported)
        {
            StartCoroutine( FadeOutRoutine(fadeToBlackDuration));
        }

  
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float time = 0f;
        Color startColor = screenOverlay.color;
        Color targetColor = Color.black;
       
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            // Lerp from current color to black
            screenOverlay.color = Color.Lerp(startColor, targetColor, t);            
            yield return null;
        }

        if (!teleported)
        {
            player.disabled = true;
            player.transform.SetPositionAndRotation(newPos.transform.position, transform.rotation);
            cam.transform.position = newPos.transform.position;
            cam.cameraUpOffset = 6.5f;
            Debug.Log("TP");
            teleported = true;
            paralax.SetActive(false);
            
        }
       
        screenOverlay.color = targetColor;
        
      
        
        StartCoroutine(FadeInRoutine(waitTimeTofadeIn, duration));

    }

    private IEnumerator FadeInRoutine(float waitTime,float duration)
    {
        float time = 0f;
        Color startColor = screenOverlay.color;
        Color targetColor = Color.clear;
        while (time < waitTime)
        {
            time += Time.deltaTime;
        }
        time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            // Lerp from current color to black
            screenOverlay.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        player.disabled = false;
        // Ensure final color is exactly black
        screenOverlay.color = targetColor;
        
    }

}
