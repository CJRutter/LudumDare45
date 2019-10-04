using UnityEngine;
using System.Collections;
using System;

public class RelayState : IState
{
    public void InitState()
    {
        if (InitAction != null)
            InitAction();
    }

    public void ReleaseState()
    {
        if (ReleaseAction != null)
            ReleaseAction();
    }
        
    public void EnterState()
    {
        if (EnterAction != null)
            EnterAction();
    }

    public void ExitState()
    {
        if (ExitAction != null)
            ExitAction();
    }

    public void UpdateState(float timeStep)
    {
        if (UpdateAction != null)
            UpdateAction(timeStep);
    }

    public void FixedUpdateState()
    {
        if (FixedUpdateAction != null)
            FixedUpdateAction();
    }

    #region Properties
    public Action InitAction { get; set; }
    public Action ReleaseAction { get; set; }
    public Action EnterAction { get; set; }
    public Action ExitAction { get; set; }
    public Action<float> UpdateAction { get; set; }
    public Action FixedUpdateAction { get; set; }
    #endregion Properties

    #region Fields
    #endregion Fields
}
