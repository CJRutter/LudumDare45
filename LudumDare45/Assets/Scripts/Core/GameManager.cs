using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseBehaviour
{
    public override void Init()
    {
        base.Init();

        gameStates = new DelegateStateMachine<System.Type>();

        Instance = this;

        CreateGameStates();
        GotoPlay();

        Player = AddChild<Player>(PlayerPrefab);
    }

    void Update()
    {
        gameStates.Update(Time.deltaTime);
    }   

    private void FixedUpdate()
    {
        FixedUpdateState();
    }

    #region Game States
    private void CreateGameStates()
    {
        gameStates.Add(typeof(PlayState), EnterState, UpdateState, ExitState);
        gameStates.Add(typeof(MainMenuState), EnterState, UpdateState, ExitState);
    }

    private void EnterState()
    {
        if (currentState != null)
            Destroy(currentState.gameObject);

        if (gameStates.Current == typeof(PlayState))
        {
            currentState = AddChild<PlayState>(PlayPrefab);
        }
        else if (gameStates.Current == typeof(MainMenuState))
        {
            currentState = AddChild<MainMenuState>(MainMenuPrefab);
        }
        currentState.EnterState();
    }

    private void UpdateState(float timeStep)
    {
        if (currentState == null) return;

        currentState.UpdateState(timeStep);
    }

    private void FixedUpdateState()
    {
        if (currentState == null) return;

        currentState.FixedUpdateState();
    }

    private void ExitState()
    {
        if (currentState == null) return;

        currentState.ExitState();
    }

    public void GotoPlay()
    {
        gameStates.ChangeState(typeof(PlayState));
    }

    public void GotoMainMenu()
    {
        gameStates.ChangeState(typeof(MainMenuState));
    }
    #endregion Game States

    public World CreateNewWorld()
    {
        if (World != null)
            Destroy(World.gameObject);

        World = AddChild<World>(WorldPrefab);
        return World;
    }

    #region Properties
    public static GameManager Instance { get; private set; }
    #endregion Properties        

    #region Fields
    public GameCamera GameCam;
    public Player Player;
    public World World;

    private DelegateStateMachine<System.Type> gameStates;
    private GameState currentState;

    // Prefabs
    public GameObject MainMenuPrefab;
    public GameObject PlayPrefab;
    public GameObject WorldPrefab;
    public GameObject PlayerPrefab;
    public GameObject ArtIntPrefab;
    public GameObject DroidPrefab;
    #endregion Fields
}
