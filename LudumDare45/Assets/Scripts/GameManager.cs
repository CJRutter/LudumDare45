using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseBehaviour
{
    public override void Init()
    {
        base.Init();

        CreateGameStates();
    }

    void Update()
    {
    }

    private void CreateGameStates()
    {
        gameStates = new StateMachine<System.Type>();


    }

    #region Properties
    #endregion Properties        

    #region Fields
    private StateMachine<System.Type> gameStates;
    #endregion Fields
}
