using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : GameComponent
{
    public override void Init()
    {
        base.Init();

        physicsBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    public void RotateToDir(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x);
        physicsBody.MoveRotation(Mathf.Rad2Deg * angle);
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public Rigidbody2D physicsBody;
    #endregion Fields
}
