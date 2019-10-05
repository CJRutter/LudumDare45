using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameState
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
        if (World == null)
            GameManager.CreateNewWorld();

        ArtInt = AddChild<ArtInt>(GameManager.ArtIntPrefab);

        Droid droid = SpawnDroid();
        ArtIntNode node = droid.GetComponent<ArtIntNode>();
        ArtInt.SetNode(node);

        GameManager.GameCam.Target = ArtInt.transform;
    }

    public override void OnExitState()
    {
    }

    public Droid SpawnDroid()
    {
        Droid droid = AddChild<Droid>(GameManager.DroidPrefab);

        return droid;
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public ArtInt ArtInt;
    #endregion Fields
}
