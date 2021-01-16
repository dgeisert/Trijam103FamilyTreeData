using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    [SerializeField] Transform bg, fill, preFill;
    public float fullValue = 100, currentValue = 100;
    public bool flytext = false;
    public bool LookAtCamera
    {
        get
        {
            return lookAtCamera;
        }
        set
        {
            lookAtCamera = value;
            if (lookAtCamera)
            {
                lookTarget = Camera.main.transform;
            }
        }
    }

    [SerializeField] private bool lookAtCamera;
    public Color flytextUp, flytextDown;
    public float flytextSize;
    public float Percent
    {
        get
        {
            return percent;
        }
        set
        {
            if (value < percent)
            {
                FillValue = Mathf.Clamp01(value);
                PreFillValue = Mathf.Clamp01(percent);
            }
            else
            {
                FillValue = Mathf.Clamp01(percent);
                PreFillValue = Mathf.Clamp01(value);
            }
            transform.Shake(0.5f, Mathf.Abs(percent - value) * 2f);
            percent = Mathf.Clamp01(value);
            changeTime = Time.time;
        }
    }
    private float FillValue
    {
        get
        {
            return fillValue;
        }
        set
        {
            fillValue = value;
            fill.transform.localScale = new Vector3(
                fillValue,
                fill.transform.localScale.y,
                fill.transform.localScale.z);
        }
    }
    private float PreFillValue
    {
        get
        {
            return preFillValue;
        }
        set
        {
            preFillValue = value;
            preFill.transform.localScale = new Vector3(
                preFillValue,
                preFill.transform.localScale.y,
                preFill.transform.localScale.z);
        }
    }
    private float percent = 1, changeTime, fillValue, preFillValue, rate = 0.01f, pause = 0.2f;
    private Transform lookTarget;

    private void Start()
    {
        LookAtCamera = lookAtCamera;
    }

    private void Update()
    {
        if (LookAtCamera)
        {
            transform.LookAt(transform.position - lookTarget.position);
        }
        if (Time.time > changeTime + pause)
        {
            if (PreFillValue != Percent)
            {
                if (PreFillValue - rate < percent)
                {
                    PreFillValue = percent;
                }
                else
                {
                    PreFillValue -= rate;
                }
            }
            if (FillValue != Percent)
            {
                if (FillValue + rate > percent)
                {
                    FillValue = percent;
                }
                else
                {
                    FillValue += rate;
                }
            }
        }
    }

    public void UpdateValue(float change)
    {
        if (flytext)
        {
            if (change < 0)
            {
                Flytext.CreateFlytext(transform.position + Vector3.up * 0.2f, change.ToString("#"), flytextDown, flytextSize);
            }
            else
            {
                Flytext.CreateFlytext(transform.position + Vector3.up * 0.2f, change.ToString("#"), flytextUp, flytextSize);
            }
        }
        currentValue += change;
        currentValue = Mathf.Clamp(currentValue, 0, fullValue);
        Percent += change / fullValue;
    }
}