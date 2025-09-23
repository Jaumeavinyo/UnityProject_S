using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   

    public CircleSizeChange circle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!circle.portalActivated)
        {
            if (collision.CompareTag("Player"))
            {
                circle.portalActivated = true;
                MusicManager.Instance.StopMusic(4.0f);
            }
        }
        else
        {
            this.enabled = false;
        }
        
    }
}
