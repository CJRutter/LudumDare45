using UnityEngine;
using System.Collections;
using CamiFramwork.Gui;
using CamiFramwork.ConsoleUtil;

public class GameInput : SingletonBehavior<GameInput>
{
	void Update()
	{
	}

    public static bool GetKeyDown(KeyCode key)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetKeyDown(key);
    }

    public static bool GetKey(KeyCode key)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetKey(key);
    }

    public static bool GetKeyUp(KeyCode key)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetKeyUp(key);
    }

    public static bool GetMouseButton(int button)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return MouseManager.GetMouseButton(button);
    }
    
    public static bool GetMouseButtonDown(int button)
    {
        //if (GuiConsole.ConsoleOpen)
        //    return false;

        return MouseManager.GetMouseButtonDown(button);
    }
    
    public static bool GetMouseButtonUp(int button)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return MouseManager.GetMouseButtonUp(button);
    }

    public static bool GetMouseClicked(int button)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return MouseManager.GetMouseButtonClicked(button);
    }
    
    public static float GetAxis(string axisName)
    {
        Console.Log("GetAxis {0} {1}", axisName, Input.GetAxis(axisName));
        if (GuiConsole.ConsoleOpen)
            return 0f;

        return Input.GetAxis(axisName);
    }

    public static bool GetButtonDown(string name)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetButtonDown(name);
    }

    public static bool GetButton(string name)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetButton(name);
    }

    public static bool GetButtonUp(string name)
    {
        if (GuiConsole.ConsoleOpen)
            return false;

        return Input.GetButtonUp(name);
    }
    
    #region Properties
    
    #region Camera Controls
    public static bool CameraZoomOut
    {
        get { return MouseManager.ScrollValue < 0 || Input.GetKey(KeyCode.Z); }
    }

    public static bool CameraZoomIn
    {
        get { return MouseManager.ScrollValue > 0 || Input.GetKey(KeyCode.X); }
    }

    public static bool MaxCameraZoom
    {
        get { return Input.GetKeyDown(KeyCode.Alpha2); }
    }

    public static bool MinCameraZoom
    {
        get { return Input.GetKeyDown(KeyCode.Alpha1); }
    }

    public static bool CameraDragButton
    {
        get { return MouseManager.GetMouseButton(2); }
    }

    public static bool CameraDragButtonDown
    {
        get { return MouseManager.GetMouseButtonDown(2); }
    }

    public static bool CameraDragButtonUp
    {
        get { return MouseManager.GetMouseButtonUp(2); }
    }
    
    public static bool CameraMoveUp
    {
        get { return Input.GetKey(KeyCode.W); }
    }
    public static bool CameraMoveDown
    {
        get { return Input.GetKey(KeyCode.S); }
    }
    public static bool CameraMoveLeft
    {
        get { return Input.GetKey(KeyCode.A); }
    }
    public static bool CameraMoveRight
    {
        get { return Input.GetKey(KeyCode.D); }
    }

    public static bool CameraInput
    {
        get
        { 
            return MinCameraZoom || MaxCameraZoom || 
                CameraZoomIn || CameraZoomOut || 
                CameraMoveUp || CameraMoveDown || 
                CameraMoveLeft || CameraMoveRight ||
                CameraDragButtonDown || CameraDragButton;
        }
    }
    #endregion Camera Controls

    #region Van Controls
    public static float VanThrottle
    {
        get { return MouseManager.GetMouseButton(0) ? 1f : 0f; }
    }
    public static bool VanBrake { get { return MouseManager.GetMouseButton(1); } }
    public static bool VanReverseToggle { get { return Input.GetKeyDown(KeyCode.R); } }
    #endregion Van Controls

    #region Packing Controls
    public static bool RestartPacking { get { return Input.GetKeyDown(KeyCode.F2); } }
    public static bool RotateShapeClockwise { get { return Input.GetKey(KeyCode.E); } }
    public static bool RotateShapeAntiClockwise { get { return Input.GetKey(KeyCode.Q); } }
    public static bool CompletePacking { get { return Input.GetKeyDown(KeyCode.Space); } }
    #endregion Packing Controls

    #region Transit Controls
    public static bool CompleteDelivery { get { return Input.GetKeyDown(KeyCode.Space); } }
    #endregion Transit Controls
    
    #region Evaluation Controls
    public static bool CompleteEvaluation { get { return Input.GetKeyDown(KeyCode.Space); } }
    #endregion Evaluation Controls

    public static bool SelectionButtonDown
    {
        get { return MouseManager.GetMouseButtonDown(0); }
    }
    public static bool SelectionButtonUp { get { return MouseManager.GetMouseButtonUp(0); } }
    public static bool SelectionButton { get { return MouseManager.GetMouseButton(0); } }
    
    public static bool CancelDown { get { return MouseManager.GetMouseButtonDown(1); } }
    #endregion Properties

    #region Fields
    #endregion Fields
}
