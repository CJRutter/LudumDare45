using UnityEngine;
using System.Collections.Generic;

public interface ICameraTarget
{
    Vector2 CameraTarget { get; }
    float TargetZoom { get; }
}
