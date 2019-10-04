using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour, IAudioPlay
{
    public void PlayNext()
    {
        AudioSource source = GetNext();
        current = Next;
        source.Play();

        MoveToNext();
    }

    private void MoveToNext()
    {
        switch (NextType)
        {
            case NextTypes.Static:
                // Do nothing
                break;
            case NextTypes.RoundRobin:
                Next = (Next + 1) % Sources.Length;
                break;
            case NextTypes.Random:
                Next = Random.Range(0, Sources.Length);
                break;
        }
    }

    public void StopCurrent()
    {
        if (IsPlaying)
            GetCurrent().Stop();
    }

    public AudioSource GetNext()
    {
        Next = Mathf.Clamp(Next, 0, Sources.Length - 1);
        return Sources[Next];
    }

    public AudioSource GetCurrent()
    {
        if (current < 0)
            return null;

        return Sources[current];
    }

    #region Properties
    public bool IsPlaying
    {
        get
        {
            AudioSource source = GetCurrent();
            if (source == null)
                return false;

            return source.isPlaying;
        }
    }
    #endregion Properties

    #region Fields
    public AudioSource[] Sources;
    public int Next = 0;
    public NextTypes NextType;
    private int current = -1;
    #endregion Fields

    public enum NextTypes
    {
        Static,
        RoundRobin,
        Random
    }
}
