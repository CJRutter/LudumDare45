using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioPlay
{
    void PlayNext();
    void StopCurrent();
    AudioSource GetNext();
    AudioSource GetCurrent();

    bool IsPlaying { get; }
}
