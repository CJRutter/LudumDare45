using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : GameComponent
{
    public override void Init()
    {
        base.Init();
		
    }

    void Update()
    {
        if (Target != null)
        {
            Position2 = Target.position;
        }
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public Transform Target;
    #endregion Fields
}
