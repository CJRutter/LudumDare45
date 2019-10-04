using UnityEngine;
using System.Collections;

public interface IState
{
    void InitState();
    void ReleaseState();
    void EnterState();
    void UpdateState(float timeStep);
    void FixedUpdateState();
    void ExitState();
}
