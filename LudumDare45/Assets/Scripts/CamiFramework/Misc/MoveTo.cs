using UnityEngine;
using System.Collections;

public class MoveTo : BaseBehaviour
{
	public void Start()
	{
    }
    
	public void Update()
	{
        moveTransform.Update(Time.deltaTime);

        if (moveTransform.Finished)
            enabled = false;
	}

    public void MoveOverTime(Vector3 position, float time, bool adjustForOvertime)
    {
        moveTransform.Transform = transform;
        moveTransform.MoveOverTime(position, time, adjustForOvertime);
        enabled = true;
    }

    #region Properties
    public bool Finished { get { return moveTransform.Finished; } }
    #endregion Properties

    #region Fields
    private MoveTransform moveTransform = new MoveTransform();
	#endregion Fields
}
