using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtInt : GameComponent
{
    public override void Init()
    {
        base.Init();

        CrossHair.enabled = false;
    }

    void Update()
    {
        if (currentNode != null)
        {
            float throttle = 0f;
            var thrustDir = new Vector2();
            if (Input.GetKey(KeyCode.W))
            {
                throttle = 1f;
                thrustDir.y = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                throttle = 1f;
                thrustDir.y = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                throttle = 1f;
                thrustDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                throttle = 1f;
                thrustDir.x = 1f;
            }
            var thrustControl = new ThrustControl()
            {
                Dir = thrustDir.normalized,
                Throttle = throttle
            };
            currentNode.SendMessage("SetThrust", thrustControl);

            if (Input.GetKeyDown(KeyCode.F))
                currentNode.SendMessage("ToggleFlightAssist");

            Vector2 aimDir = MouseManager.WorldPosition - currentNode.Position2;
            aimDis = aimDir.magnitude;
            aimDir /= aimDis;

            aimDis = Mathf.Min(aimDis, MaxAimDis);

            CrossHair.Position = currentNode.Position + (Vector3)(aimDir * aimDis);

            currentNode.SendMessage("RotateToDir", aimDir);
        }

        DebugText.Add($"desiredThurst: {desiredThurst}");
    }

    public void MoveToNode(ArtIntNode node)
    {
        SetNode(node);
    }

    public void SetNode(ArtIntNode node)
    {
        if (currentNode != null)
        {
            currentNode.ExitArtInt();
            transform.SetParent(World.transform);
        }

        CrossHair.enabled = false;
        currentNode = node;

        if (currentNode == null)
            return;

        transform.SetParent(currentNode.transform);
        currentNode.EnterArtInt(this);
        CrossHair.enabled = true;
        currentNode.SendMessage("SetFlightAssist", true);
    }

    #region Properties
    public ArtIntNode CurrentNode { get { return currentNode; } }
    #endregion Properties        

    #region Fields
    public FocusCrossHair CrossHair;

    private ArtIntNode currentNode;
    public float MaxAimDis = 5f;
    private Vector2 aimDir;
    private float aimDis;
    private Vector2 desiredThurst;  
    #endregion Fields
}
