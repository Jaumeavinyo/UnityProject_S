using UnityEngine;

public class Paralax : MonoBehaviour
{
    public FSM_CharMov player;
    public float moveSpeedMultiplyer;
    [SerializeField] bool scrollLeft;

    private float singleTextureWidth;
    private Vector3 previousPlayerPosition;
    private float totalOffset;

    void Start()
    {
        SetUpTexture();
        if (player != null)
        {
            previousPlayerPosition = player.transform.position;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate movement based on player position change
        float playerDeltaX = player.transform.position.x - previousPlayerPosition.x;
        float parallaxDelta = playerDeltaX * moveSpeedMultiplyer;

        if (scrollLeft) parallaxDelta = -parallaxDelta;

        // Apply movement
        transform.position += new Vector3(parallaxDelta, 0f, 0f);
        totalOffset += parallaxDelta;

        // Check for reset
        if (Mathf.Abs(totalOffset) >= singleTextureWidth)
        {
            float resetAmount = Mathf.Sign(totalOffset) * singleTextureWidth;
            transform.position -= new Vector3(resetAmount, 0f, 0f);
            totalOffset -= resetAmount;
        }

        previousPlayerPosition = player.transform.position;
    }

    void SetUpTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }
}