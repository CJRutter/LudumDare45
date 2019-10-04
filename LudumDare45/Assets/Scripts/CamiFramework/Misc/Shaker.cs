using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Oscillator))]
public class Shaker : MonoBehaviour
{
	void Awake()
    {
        oscillator = GetComponent<Oscillator>();
        Oscillator.enabled = false;
	}

	void Update ()
    {
        if (!ShakeActive)
            return;

        if (timeLeft > 0f || ShakeLength == 0f)
        {
            bool updateOffset = true;


            if (ShakeLength > 0f)
            {
                timeLeft -= Time.deltaTime;

                updateOffset = timeLeft > 0f;
            }
            else
            {
            }

            if (updateOffset)
            {
                Oscillator.Amplitude -= (Damping * Time.deltaTime);
                ShakeOffset = (ShakeVector * Oscillator.Value);
            }
            else
            {
                StopShake();
            }

            if (UpdatePosition)
            {
                transform.localPosition = ShakeOffsetFinal;
            }
        }
    }

    public void StartShake()
    {
        if (!ShakeActive)
        {
            Oscillator.Angle = 0f;
            Oscillator.enabled = true;
            ShakeActive = true;
        }

        if (ShakeLength <= 0f)
        {
            Damping = 0f;
        }
        else
        {
            Damping = Amplitude / ShakeLength;
        }

        timeLeft = ShakeLength;
        Oscillator.Amplitude = Amplitude;
        
        if (VlambeerMode)
        {
            Oscillator.Frequency = Frequency * VlambeerModeFreqCoef;
        }
        else
        {
            Oscillator.Frequency = Frequency;
        }
    }

    public void StopShake()
    {
        ShakeActive = false;
        Oscillator.enabled = false;
        ShakeOffset = Vector3.zero;
    }

    #region Properties
    public bool ShakeActive { get; private set; }
    public Vector3 ShakeOffset
    {
        get { return shakeOffset; }
        set { shakeOffset = value; }
    }

    public Vector3 ShakeOffsetFinal
    {
        get
        {
            if (VlambeerMode)
            {
                return shakeOffset * VlambeerModeOffsetCoef;
            }
            else
            {
                return shakeOffset;
            }
        }
        private set
        {
            shakeOffset = value;
        }
    }
    
    private Oscillator Oscillator
    {
        get
        {
            if (oscillator == null)
                oscillator = GetComponent<Oscillator>();

            return oscillator;
        }
    }
    #endregion Properties

    #region Fields
    public bool VlambeerMode = false;
    public float ShakeLength = 1f;
    public Vector3 ShakeVector = Vector3.left;
    public bool UpdatePosition = false;

    private Oscillator oscillator;
    private Vector3 shakeOffset = Vector3.zero;
    private float timeLeft;
    public float Amplitude = 1f;
    public float Frequency = 1f;
    public float Damping = 0.9f;

    private const float VlambeerModeOffsetCoef = 10f;
    private const float VlambeerModeFreqCoef = 3f;
    #endregion Fields
}
