using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : SingletonBehavior<MouseManager>
{	
	void Update()
	{
        previousScreenPosition = ScreenPosition;
        screenPosition = Input.mousePosition;
        previousWorldPosition = WorldPosition;
        if(Camera.main != null)
            worldPosition = Camera.main.ScreenToWorldPoint(ScreenPosition);

        scrollValue = Input.GetAxis("Mouse ScrollWheel");

        for(int button = 0; button < 3; ++button)
        {
            if(GetMouseButtonDown(button))
                downPosition[button] = Input.mousePosition;
        }
	}

    public static bool GetMouseButtonClicked(int button)
    {
        if (Input.GetMouseButtonUp(button) == false)
            return false;

        Vector2 delta = downPosition[button] - Input.mousePosition;

        float threshold = Instance.ClickThreshold * Instance.ClickThreshold;

        if (delta.sqrMagnitude <= threshold)
        {
            return true;
        }
        return false;
    }

    public static bool GetMouseButtonDown(int button)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        return Input.GetMouseButtonDown(button);
    }

    public static bool GetMouseButton(int button)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        return Input.GetMouseButton(button);
    }

    public static bool GetMouseButtonUp(int button)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        return Input.GetMouseButtonUp(button);
    }

    #region Properties
    public static Vector2 ScreenPosition
    {
        get { return Instance.screenPosition; }
    }

    public static Vector2 PreviousScreenPosition
    {
        get { return Instance.previousScreenPosition; }
    }

    public static Vector2 WorldPosition
    {
        get { return Instance.worldPosition; }
    }

    public static Vector2 PreviousWorldPosition
    {
        get { return Instance.previousWorldPosition; }
    }
    public static float ScrollValue { get { return Instance.scrollValue; } }
    #endregion Properties

    #region Fields
    private Vector2 screenPosition;
    private Vector2 previousScreenPosition;
    private Vector2 worldPosition;
    private Vector2 previousWorldPosition;
    private float scrollValue;
    public float ClickThreshold = 4f;
    private static Vector3[] downPosition = new Vector3[3];
    #endregion Fields
}
