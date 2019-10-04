using Cami.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Stats : MonoBehaviour
{
    void Start()
    {
        Instance = this;
    }
    
    void Update()
    {
        if (StatsUpdated != null)
        {
            dataLock.WaitOne();
            StatsUpdated();
            dataLock.ReleaseMutex();
        }
    }

    private void StartTimerIntern(string key)
    {
        var timer = new Stopwatch();
        dataLock.WaitOne();

        dataStore[key] = timer;

        dataLock.ReleaseMutex();

        if (StatChanged != null)
            StatChanged();

        timer.Start();
    }
    
    private int StopTimerIntern(string key, string timeKey)
    {
        int time = -1;
        dataLock.WaitOne();

        Stopwatch timer = (Stopwatch)dataStore[key];
        timer.Stop();
        dataStore.Remove(key);

        time = (int)timer.ElapsedMilliseconds;
        if(timeKey != null)
            dataStore[timeKey] = time;

        dataLock.ReleaseMutex();

        if (StatChanged != null)
            StatChanged.Invoke();

        return time;
    }

    private float StopTimerAvgIntern(string key)
    {
        double milisTaken = -1;
        dataLock.WaitOne();

        Stopwatch timer = (Stopwatch)dataStore[key];
        timer.Stop();
        dataStore.Remove(key);

        long time = timer.ElapsedTicks;

        string timeKey = key + "_time";
        string countKey = key + "_count";
        string avgKey = key + "_avg";

        long totalTime = 0;
        if (dataStore.ContainsKey(timeKey))
            totalTime = (long)dataStore[timeKey];
        totalTime += time;
        dataStore[timeKey] = totalTime;

        int count = 0;
        if (dataStore.ContainsKey(countKey))
            count = (int)dataStore[countKey];
        ++count;
        dataStore[countKey] = count;

        milisTaken = ((double)totalTime / (double)Stopwatch.Frequency) * 1000.0;
        double avgTime = (milisTaken / (double)count);
        dataStore[avgKey] = avgTime;
        
        dataLock.ReleaseMutex();

        if (StatChanged != null)
            StatChanged();

        return (float)milisTaken;
    }

    private void SetValueIntern(string key, object value)
    {
        dataLock.WaitOne();

        dataStore[key] = value;

        dataLock.ReleaseMutex();
    }

    private object GetValueIntern(string key)
    {
        dataLock.WaitOne();
        object value = dataStore[key];
        dataLock.ReleaseMutex();
        return value;
    }

    public void Clear()
    {
        dataLock.WaitOne();
        dataStore.Clear();
        dataLock.ReleaseMutex();
    }

    #region Statics
    public static void StartTimer(string key)
    {
        Instance.StartTimerIntern(key);
    }

    public static int StopTimer(string key, string timeKey = null)
    {
        return Instance.StopTimerIntern(key, timeKey);
    }

    public static float StopTimerAvg(string key)
    {
        return Instance.StopTimerAvgIntern(key);
    }

    public static void SetValue(string key, object value)
    {
        Instance.SetValueIntern(key, value);
    }
    #endregion Statics

    public void Foreach(Action<string, object> action)
    {
        dataLock.WaitOne();
        var data = dataStore.ToList();
        dataLock.ReleaseMutex();

        foreach (KeyValuePair<string, object> kvp in data)
        {
            action(kvp.Key, kvp.Value);
        }
    }

    #region Properties
    public IEnumerable<KeyValuePair<string, object>> Data
    {
        get
        {
            dataLock.WaitOne();
            var data = dataStore.ToList();
            dataLock.ReleaseMutex();
            return data;
        }
    }
    #endregion Properties

    #region Fields
    private Dictionary<string, object> dataStore = new Dictionary<string, object>();
    private Mutex dataLock = new Mutex();

    public static Stats Instance;
    #endregion Fields

    #region Events
    public event System.Action StatChanged;
    public event System.Action StatsUpdated;
    #endregion Events
}
