using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : GameComponent
{
    public override void Init()
    {
        base.Init();

    }
    
    public virtual void ReleaseState()
    {
    }

    public void EnterState()
    {
        OnEnterState();

        if (EnteredState != null)
            EnteredState(this);

        gameObject.SetActive(true);
    }

    public virtual void UpdateState(float timeStep)
    {
    }

    public virtual void FixedUpdateState()
    {
    }

    public void ExitState()
    {
        OnExitState();

        if (ExitedState != null)
            ExitedState(this);

        gameObject.SetActive(false);
        DestroyAllChildren();
    }

    public virtual void OnEnterState()
    {
    }

    public virtual void OnExitState()
    {
    }

    #region Properties
    #endregion Properties

    #region Fields
    #endregion Fields

    #region Events
    public delegate void GameStateEventHandler(GameState stae);

    public event GameStateEventHandler EnteredState;
    public event GameStateEventHandler ExitedState;
    #endregion Events

    #region Contants
    #endregion Contants
}
