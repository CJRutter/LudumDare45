using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : GameComponent
{
    public override void Init()
    {
        base.Init();

        physicsBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }
    
    public void SetThrust(Vector2 requiredthrust)
    {
        float affect = Vector2.Dot(requiredthrust, NozzleDir);
        thrust = affect * MaxThrust;
        Throttle = affect;
    }

    public void SetFlightAssist(bool enabled)
    {
        FlightAssist = enabled;
    }

    public void ToggleFlightAssist()
    {
        FlightAssist = !FlightAssist;
    }

    private void PerformFlightAssist()
    {
        if (FlightAssist == false)
            return;
        if (Throttle > 0f)
            return;

        float velComp = Vector2.Dot(physicsBody.velocity, NozzleDir);
        if (velComp >= 0f)
            return;

        float correctThrust = (-velComp / physicsBody.mass);
        correctThrust = Mathf.Clamp(correctThrust, 0f, MaxThrust);
        physicsBody.AddForce(NozzleDir * correctThrust);
    }

    private void FixedUpdate()
    {
        if (Throttle > 0f)
        {
            physicsBody.AddForce(NozzleDir * thrust);
            Debug.LogFormat("NozzleDir * thrust: {0}", NozzleDir * thrust);
        }
        else
        {
            PerformFlightAssist();
        }
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public Rigidbody2D physicsBody;

    public float MaxThrust = 10f;
    public float Throttle = 0f;
    public Vector2 NozzleDir;
    private float thrust;

    public bool FlightAssist = true;
    //private CountDown commandCountdown
    #endregion Fields
}
