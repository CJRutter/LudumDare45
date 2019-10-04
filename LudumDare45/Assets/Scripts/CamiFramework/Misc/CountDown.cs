using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountDown
{
    public CountDown(float startTime = 0)
    {
        this.startTime = startTime;
        Reset();
    }

    public bool Update(float timeStep)
    {
        currentTime -= timeStep;
        if (currentTime <= 0f)
        {
            Finish();
            return true;
        }

        return false;
    }

    public void Reset(float startTime)
    {
        this.startTime = startTime;
        currentTime = startTime;
        overTime = 0f;
    }

    public void Reset()
    {
        currentTime = startTime;
        overTime = 0f;
    }

    public void ResetOverTime()
    {
        overTime = 0f;
    }

    public void Finish()
    {
        overTime = -currentTime;
        currentTime = 0f;
    }

    #region Properties
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    public float StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }
    public bool Finished { get { return currentTime <= 0f; } }
    public float T
    {
        get { return startTime  != 0.0f ? 1f - (currentTime / startTime) : 1.0f; }
        set
        {
            currentTime = startTime * (1f - Mathf.Clamp(value, 0, 1f));
        }
    }
    public float OverTime { get { return overTime; } }
    #endregion Properties

    #region Fields
    private float startTime;
    private float currentTime;
    private float overTime;
    #endregion Fields
}
