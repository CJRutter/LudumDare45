using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponent : BaseBehaviour
{
    public override void Init()
    {
        base.Init();
		
    }

    #region Properties
    public GameManager GameManager { get { return GameManager.Instance; } }
    public Player Player { get { return GameManager.Player; } }
    public World World { get { return GameManager.World; } }
    #endregion Properties

    #region Fields
    #endregion Fields
}
