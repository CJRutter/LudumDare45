﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameState
{
    public override void Init()
    {
        base.Init();

    }

    public override void UpdateState(float timeStep)
    {
    }

    public override void OnEnterState()
    {
        GameManager.CreateNewWorld();
    }

    public override void OnExitState()
    {
    }

    #region Properties
    #endregion Properties        

    #region Fields
    #endregion Fields
}
