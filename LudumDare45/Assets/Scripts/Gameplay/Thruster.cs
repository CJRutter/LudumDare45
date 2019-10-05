using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : GameComponent
{
    public override void Init()
    {
        base.Init();

        physicsBody = GetComponent<Rigidbody2D>();

        NozzleDir.Normalize();
    }

    void Update()
    {
        //DebugText.Add($"thruster {NozzleDir}:");
        //DebugText.Add($"  throttle: {Throttle}");
        //DebugText.Add($"  thrust: {thrust}");
        //DebugText.Add($"  speed: {speed} ");

    }

    public void SetThrust(ThrustControl thrustControl)
    {
        this.desiredThrust = thrustControl;

        float maxInfluence = Vector2.Dot(thrustControl.Dir * thrustControl.Throttle, -NozzleDir);
        if (maxInfluence > 0f)
        {
            Throttle = maxInfluence;
        }
        else
        {
            Throttle = 0f;
        }
    }

    public void SetFlightAssist(bool enabled)
    {
        FlightAssistOn = enabled;
    }

    public void ToggleFlightAssist()
    {
        FlightAssistOn = !FlightAssistOn;
    }

    private void FixedUpdate()
    {
        PerformFlightAssist();
        if (Throttle > 0f)
        {
            thrust = Throttle * MaxThrust;
            physicsBody.AddForce(-NozzleDir * thrust);
        }
        else
        {
            thrust = 0f;
            Throttle = 0f;
        }
    }
    
    private void PerformFlightAssist()
    {
        if (FlightAssistOn == false) return;
        if (desiredThrust.Throttle > 0f) return;

        travelDir = physicsBody.velocity;
        speed = travelDir.magnitude;
        travelDir /= speed;

        if (speed < CorrectionThreshold) return;

        float maxDecel = MaxThrust / physicsBody.mass;
        float throttle = Mathf.Max(speed / maxDecel, 1f);

        float maxInfluence = Vector2.Dot(-travelDir * throttle, -NozzleDir);
        if (maxInfluence <= 0f) return;
        if (maxInfluence < CorrectionThreshold)
        {
            Throttle = 0f;
            return;
        }
        Throttle = maxInfluence;
    }

    #region Properties
        #endregion Properties        

    #region Fields
    public Rigidbody2D physicsBody;

    public float MaxThrust = 10f;
    public float Throttle = 0f;
    public float CorrectionThreshold = 0.1f;
    public Vector2 NozzleDir;
    private float thrust;
    private Vector2 travelDir;
    private float speed;

    public bool FlightAssistOn = true;
    private ThrustControl desiredThrust;
    #endregion Fields
}

public struct ThrustControl
{
    public ThrustControl(Vector2 dir, float throttle)
    {
        this.Dir = dir;
        this.Throttle = throttle;
    }

    public static ThrustControl None
    {
        get { return new ThrustControl(Vector2.zero, 0); }
    }

    public Vector2 Dir;
    public float Throttle;
}
