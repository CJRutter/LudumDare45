using UnityEngine;
using System.Collections.Generic;

public class DelegateStateMachine<T>
{
    public DelegateStateMachine()
    {
    }

    public void Update(float timeStep)
    {
        CheckStateChange();
        if (HasState == false) return;

        states[current].Update(timeStep);
        timeInCurrentState += timeStep;
        
    }

    public void ChangeState(T newState)
    {
        if (states.ContainsKey(newState) == false) return;

        next = newState;
        changeRequested = true;
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
        if (changeRequested == false) return;
        
        if (HasState)
            states[current].Exit();

        previous = current;
        current = next;
        changeRequested = false;
        HasState = true;

        timeInCurrentState = 0f;

        states[current].Enter();
    }

    #region Properties
    public T Current { get { return current; } }
    public T Next  { get { return next; } }
    public T Previous { get { return previous; } }
    public float TimeInCurrentState { get { return timeInCurrentState; } }
    public bool HasState { get; private set; }
    #endregion Properties

    #region Fields
    private T current;
    private T next;
    private T previous;
    private Dictionary<T, State> states = new Dictionary<T, State>();
    private float timeInCurrentState = 0f;
    private bool changeRequested = false;
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