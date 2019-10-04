using UnityEngine;
using System.Collections.Generic;

public class DelegateStateMachine<T>
{
    public DelegateStateMachine()
    {
    }

    public void Update(float timeStep)
    {
        if (states.ContainsKey(current))
        {
            CheckStateChange();
            if (hasState)
            {
                states[current].Update(timeStep);
                timeInCurrentState += timeStep;
            }
        }
    }

    public void ChangeState(T newState, bool forceStateChange = false)
    {
        if (!states.ContainsKey(newState))
            return;

        this.forceStateChange = forceStateChange;
        next = newState;
    }

    public void Add(T state, EnterState enter, UpdateState update, ExitState exit)
    {
        states.Add(state, new State
        {
            Enter = enter,
            Update = update,
            Exit = exit
        });
    }

    private void CheckStateChange()
    {
        if (hasState && current.Equals(next) && !forceStateChange)
            return;

        forceStateChange = false;

        if (hasState)
        {
            states[current].Exit();
        }
        else
        {
            hasState = true;
        }

        previous = current;
        current = next;

        timeInCurrentState = 0f;

        states[current].Enter();
    }

    #region Properties
    public T Current { get { return current; } }
    public T Next  { get { return next; } }
    public T Previous { get { return previous; } }
    public float TimeInCurrentState { get { return timeInCurrentState; } }
    #endregion Properties

    #region Fields
    private T current;
    private T next;
    private T previous;
    private Dictionary<T, State> states = new Dictionary<T, State>();
    private bool hasState = false;
    private bool forceStateChange = false;
    private float timeInCurrentState = 0f;
    #endregion Fields

    #region Delegates
    public delegate void EnterState();
    public delegate void UpdateState(float timeStep);
    public delegate void ExitState();
    #endregion Delegates

    class State
    {
        public EnterState Enter;
        public UpdateState Update;
        public ExitState Exit;
    }
}