using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class energySlider : MonoBehaviour
{

    private int maxValue_;
    public int currValue_;

    public Slider sliderBar;

    public FSM_CharMov character;

    public bool grow;
    public int growFactor;
    public int originalGrowFactor;
    void Start()
    {
        maxValue_ = 1000;
        currValue_ = 0;
        growFactor = 3;
        originalGrowFactor = growFactor;
        grow = true;
        sliderBar.maxValue = maxValue_;
        sliderBar.value = maxValue_;
    }


   
    void FixedUpdate()
    {

        //sliderBar.SetValueWithoutNotify(currValue_);
        string message = sliderBar.value.ToString();
        //ebug.Log("SLIDER VALUE = " + message);
        if (grow)
        {
            if (currValue_ < maxValue_)
            {
                sliderBar.value = currValue_;
                currValue_ += 1*growFactor;
            }
        }

    }

    public void modifyEnergyValue(int value)
    {
        currValue_ += value;
    }

    
}
