using UnityEngine;
using System.Collections;

public class FaceCamera : BaseBehaviour
{
    public override void Init()
    {
        base.Init();

        if(Camera == null)
        {
            Camera = Camera.main;
        }
	}
	
	public void Update()
	{
        Vector3 dirToCamera;
        if(Camera.orthographic)
        {
            dirToCamera = -Camera.transform.forward;
        }
        else
        {
            dirToCamera = transform.position - Camera.transform.position;
        }
        if (dirToCamera == Vector3.zero)
            dirToCamera = new Vector3(0.01f, 0, 0);

        if (XAxis == false)
            dirToCamera.x = 0f;
        if (YAxis == false)
            dirToCamera.y = 0f;
        if (ZAxis == false)
            dirToCamera.z = 0f;
        
        transform.rotation = Quaternion.LookRotation(-dirToCamera);
	}

    #region Properties
    #endregion Properties

    #region Fields
    public Camera Camera;
    public bool XAxis = true;
    public bool YAxis = true;
    public bool ZAxis = true;
	#endregion Fields
}
