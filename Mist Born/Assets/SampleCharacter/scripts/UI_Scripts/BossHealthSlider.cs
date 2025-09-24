using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthSlider : MonoBehaviour
{
    public EnemyBoss boss;

    public bool grow;
    public int growFactor;
    public Slider backSliderBar;
    public Slider frontSliderBar;
    public int sliderValue = 100;// %
    int newSliderValue;

    public float valueChangeTime;

    private void Awake()
    {
        sliderValue = 100;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSliderValue(int substract)
    {
        newSliderValue = sliderValue - substract;
        frontSliderBar.value = newSliderValue;
        if (newSliderValue > 0)
        {
            StartCoroutine(ChangeValueOverTime(sliderValue, newSliderValue, valueChangeTime, UpdateSliderHealth));
        }
        else
        {
            StartCoroutine(ChangeValueOverTime(sliderValue, newSliderValue, valueChangeTime, UpdateSliderHealth));
            boss.Die();
        }

    }
    void UpdateSliderHealth(int value)
    {
        sliderValue = value;
        backSliderBar.value = value;
    }
    IEnumerator ChangeValueOverTime(int startValue, int endValue, float duration, System.Action<int> onValueChanged)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, easedT));
            onValueChanged(currentValue);
            yield return null;
        }

        onValueChanged(endValue);
    }
}
