using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPlayerStates : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public FSM_CharMov CharacterRef;
    public TextMeshProUGUI prevStateText; // Changed to TMP
    public TextMeshProUGUI currStateText; // Changed to TMP
    public Image currStateSquare;
    public Image prevStateSquare;

    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color changedColor = Color.red;
    public float flashDuration = 0.5f;

    private string _strPrevState;
    private string _strCurrState;
    private float _currStateFlashTimer;
    private float _prevStateFlashTimer;

    void Update()
    {
        // Get current states
        string newCurrState = CharacterRef.getCurrState().name;
        string newPrevState = CharacterRef.previousState.name;

        // Check for current state change
        if (newCurrState != _strCurrState)
        {
            _strCurrState = newCurrState;
            currStateText.text = _strCurrState;
            _currStateFlashTimer = flashDuration;
            currStateSquare.color = changedColor;
        }

        // Check for previous state change
        if (newPrevState != _strPrevState)
        {
            _strPrevState = newPrevState;
            prevStateText.text = _strPrevState;
            _prevStateFlashTimer = flashDuration;
            prevStateSquare.color = changedColor;
        }

        // Update flash timers
        if (_currStateFlashTimer > 0)
        {
            _currStateFlashTimer -= Time.deltaTime;
            if (_currStateFlashTimer <= 0)
            {
                currStateSquare.color = defaultColor;
            }
        }

        if (_prevStateFlashTimer > 0)
        {
            _prevStateFlashTimer -= Time.deltaTime;
            if (_prevStateFlashTimer <= 0)
            {
                prevStateSquare.color = defaultColor;
            }
        }
    }
}
