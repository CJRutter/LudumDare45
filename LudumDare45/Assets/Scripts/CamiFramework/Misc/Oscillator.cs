using UnityEngine;
using System.Collections;

public class Oscillator : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        float angleIncrease = Frequency * MathsHelper.TAU * Time.deltaTime;
        Angle += angleIncrease;
        Angle = MathsHelper.WrapAngle(Angle);
        value = Mathf.Sin(Angle) * Amplitude;
    }

    #region Properties
    public float Value { get { return this.value; } }
    #endregion Properties

    #region Fields
    public float Angle = 0f;
    public float Amplitude = 1f;
    public float Frequency = 1f;

    private float value;
    #endregion Fields
}