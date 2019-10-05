using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtIntNode : GameComponent
{
    public override void Init()
    {
        base.Init();
		
    }

    void Update()
    {
    }

    public void EnterArtInt(ArtInt artInt)
    {
        this.artInt = artInt;
    }

    public void ExitArtInt()
    {
        artInt = null;
    }

    #region Properties
    #endregion Properties        

    #region Fields
    private ArtInt artInt;
    #endregion Fields
}
