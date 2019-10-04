using UnityEngine;
using System.Collections.Generic;

public class StateMachine<T>
{
    public StateMachine()
    {
    }

	public void Update(float timeStep)
    {
        if (phase != Phases.NoState)
        {
            states[current].UpdateState(timeStep);
            timeInState += timeStep;
        }

        if (Transitioning)
        {
            transitionCountdown.Update(timeStep);

            if (phase == Phases.TransitionIn)
            {
                if (transitionCountdown.Finished)
                {
                    GotoNextState();
                    transitionCountdown.Reset();

                    phase = Phases.TransitionOut;
                    if (OnPhaseChange != null)
                        OnPhaseChange(phase);
                }
            }
            else if (phase == Phases.TransitionOut)
            {
                if (transitionCountdown.Finished)
                {
                    if (!beganInitialState)
                    {
                        beganInitialState = true;
                    }

                    if (states.ContainsKey(current))
                    {
                        phase = Phases.State;
                    }
                    else
                    {
                        phase = Phases.NoState;
                    }
                    if (OnPhaseChange != null)
                        OnPhaseChange(phase);
                }
            }
        }
	}

    public void FixedUpdate()
    {
        if (phase != Phases.NoState && beganInitialState)
        {
            states[current].FixedUpdateState();
        }
    }

    public void ChangeState(T newState, float transitionTime = 0.0f)
    {
        if (!states.ContainsKey(newState) || Transitioning)
            return;

        next = newState;

        transitionCountdown.Reset(transitionTime / 2f);
        if (beganInitialState)
        {
            phase = Phases.TransitionIn;
        }
        else
        {
            GotoNextState();
            phase = Phases.TransitionOut;
        }

        if (OnPhaseChange != null)
            OnPhaseChange(phase);
    }

    public void Add(T stateKey, IState state)
    {
        states.Add(stateKey, state);
    }

    private void GotoNextState()
    {
        if (beganInitialState)
        {
            if (states.ContainsKey(current))
                states[current].ExitState();
        }

        previous = current;
        current = next;
        timeInState = 0f;

        IState newState = null;
        if (states.ContainsKey(current))
        {
            newState = states[current];
            newState.EnterState();
        }

        if (OnStateChange != null)
            OnStateChange(current, newState);
    }

    public void ForeachState(StateProcess process)
    {
        foreach (KeyValuePair<T, IState> kvp in states)
            process(kvp.Key, kvp.Value);
    }

    public bool ContainsState(T key)
    {
        return states.ContainsKey(key);
    }

	#region Properties
    public T Current { get { return current; } }
    public IState CurrentState { get { return states[current]; } }
    public T Previous { get { return previous; } }
    public IState PreviousState { get { return states[previous]; } }
    public T Next { get { return next; } }
    public IState NextState { get { return states[next]; } }
    public IState this[T key] { get { return states[key]; } }
    public Phases Phase { get { return phase; } }
    public float TransitionT { get { return transitionCountdown.T; } }
    public float TransitionCurrentTime { get { return transitionCountdown.CurrentTime; } }
    public bool Transitioning { get { return phase == Phases.TransitionIn || phase == Phases.TransitionOut; } }
    public float TimeInState { get { return timeInState; } }
    #endregion Properties

    #region Fields
    private T current;
    private T next;
    private T previous;
    private Dictionary<T, IState> states = new Dictionary<T, IState>();
    private Phases phase;
    private CountDown transitionCountdown = new CountDown();
    private bool beganInitialState = false;
    private float timeInState;

    public delegate void PhaseChangeCallback(Phases phase);
    public event PhaseChangeCallback OnPhaseChange;
    public delegate void StateCallback(T key, IState state);
    public event StateCallback OnStateChange;

    public delegate void StateProcess(T key, IState state);
    #endregion Fields

    #region Enums
    public enum Phases
    {
        NoState,
        State,
        TransitionIn,
        TransitionOut
    }
    #endregion Enums
}
