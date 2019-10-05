using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : SingletonBehavior<DebugText>
{
	void Update()
	{
    }

    void LateUpdate()
    {
#if DEBUG
        TextDisplay.text = debugTextBuilder.ToString();
#endif
    }

    public void AddDebugText(string text)
    {
        debugTextBuilder.AppendLine(text);
    }

    public void AddDebugText(string format, params object[] args)
    {
        debugTextBuilder.AppendFormat(format, args);
        debugTextBuilder.AppendLine();
    }

    public void ClearDebugText()
    {
        debugTextBuilder.Length = 0;
    }

    public static void Add(string text)
    {
        Instance.AddDebugText(text);
    }

    public static void Add(string format, params object[] args)
    {
        Instance.AddDebugText(format, args);
    }
	
    public static void Clear()
    {
        Instance.ClearDebugText();
    }

    #region Properties
    #endregion Properties

    #region Fields
    private StringBuilder debugTextBuilder = new StringBuilder();
    public Text TextDisplay;
    #endregion Fields
}
