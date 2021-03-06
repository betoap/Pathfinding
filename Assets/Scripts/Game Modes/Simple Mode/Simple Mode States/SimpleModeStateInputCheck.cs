using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleModeStateInputCheck : SimpleModeState
{

    private Vector2 _grabStartedAt;

    public SimpleModeStateInputCheck(SimpleMode simpleMode) : base(simpleMode)
    {
    }

    /// <summary>
    /// Ticked on every frame
    /// </summary>
    public override void Tick()
    {

    }

    /// <summary>
    /// Ticked on every physics update
    /// </summary>
    public override void FixedTick()
    {

    }
    /// <summary>
    /// Ticked when the state machine enter this state
    /// </summary>
    public override void OnEnter()
    {
        Debug.Log("Entered Input Check State");
        InputHandler.onStartGrab += this.OnGrabStart;
        InputHandler.onFinishedGrab += this.OnGrabFinish;
    }

    /// <summary>
    /// Ticked when the state machine exit this state
    /// </summary>
    public override void OnExit()
    {
        InputHandler.onStartGrab -= this.OnGrabStart;
        InputHandler.onFinishedGrab -= this.OnGrabFinish;
    }

    public void OnGrabStart(Vector2 positionOnGrid)
    {
        this._grabStartedAt = positionOnGrid;
    }

    public void OnGrabFinish(Vector2 positionOnGrid)
    {
        Vector2 distance = (positionOnGrid - this._grabStartedAt).normalized;

        //Store initial tile and final tile in a variable
        Tile from = _simpleMode.core.board.GetTile(this._grabStartedAt);
        Tile to = _simpleMode.core.board.GetTile(this._grabStartedAt + distance);

        //If any of the tiles is null, or they are the same, do nothing
        if (from == null || to == null || (from.position == to.position)) return;

        _simpleMode.core.board.SwapTilesItems(from, to);

        _simpleMode.stateMachine.SetActiveState(_simpleMode.simpleModeStateEvaluateMatches);
    }
}
