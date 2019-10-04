using UnityEngine;
using System.Collections;

public class MoveTransform
{
    public MoveTransform()
    {
    }

    public MoveTransform(Transform transform)
    {
        this.transform = transform;
    }

	public void Update(float timeStep)
	{
        if (countdown.Finished || transform == null)
            return;

        countdown.Update(timeStep);

        transform.position = Vector3.Lerp(startPosition, TargetPosition, countdown.T);
	}
    
    public void Move(bool adjustForOvertime)
    {
        if(UseSpeed)
        {
            Vector3 dif = TargetPosition - transform.position;
            MoveTime = dif.magnitude / Speed;
        }

        MoveOverTime(adjustForOvertime);
    }

    public void MoveOverTime(Vector3 position, float time, bool adjustForOvertime)
    {
        TargetPosition = position;
        MoveTime = time;
        MoveOverTime(adjustForOvertime);
    }

    public void MoveOverTime(bool adjustForOvertime)
    {
        float time = MoveTime;
        if (adjustForOvertime)
            time -= countdown.OverTime;

        startPosition = transform.position;

        countdown.Reset(time);
    }

    public void MoveAtSpeed(float speed, bool adjustForOvertime)
    {
        Speed = speed;
        Move(adjustForOvertime);
    }

    #region Properties
    public Vector3 TargetPosition { get; set; }
    public Transform Transform
    {
        get { return transform; }
        set { transform = value; }
    }
    public float MoveTime { get; set; }
    public bool Finished { get { return countdown.Finished; } }
    public bool UseSpeed { get; set; }
    public float Speed { get; set; }
    #endregion Properties

    #region Fields
    private Transform transform;
    private CountDown countdown = new CountDown();
    private Vector3 startPosition;
	#endregion Fields
}
