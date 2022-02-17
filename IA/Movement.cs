using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Board;
using FX;
using Character;

namespace IA {
  public class Movement : MonoBehaviour {
    public float distanceToChangeWaypoint = .0f;
    public Pathfinding pathfinding;
    public float speed;
    public float jumpHeight = 1.0f;
    private Vector3 GravityVelocity;
    private Vector3 currentPosition;
    public Vector3 objectivePosition;
    List<Tile> path;
    // Rigidbody _rigidbody;
    int currentWayPoint;
    bool followingPath;

    Unit unit;
    Tile tileCurrent;
    CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    public SkillSoundFX soundFX;

    private void Awake() {
      characterController = GetComponent<CharacterController>();
      unit = GetComponent<Unit>();
      pathfinding = FindObjectOfType<Pathfinding>();
    }

    private void FixedUpdate() {
      Gravity();
    }

    void Gravity() {
      GravityVelocity.y += Physics.gravity.y * Time.deltaTime;
      characterController.Move(GravityVelocity * Time.deltaTime * speed);
    }

    IEnumerator Move() {
      while (followingPath) {
        moveDirection = path[currentWayPoint].position - transform.position;
        if (characterController.isGrounded && GravityVelocity.y < 0) {
          GravityVelocity.y = 0f;
        }
        yield return null;
        characterController.Move(moveDirection.normalized * Time.deltaTime * speed);
        if(moveDirection.sqrMagnitude <= distanceToChangeWaypoint) {
          yield return StartCoroutine( CheckWaypoint() );
        }
      }
    }

    IEnumerator CheckWaypoint() {
      if (currentWayPoint < path.Count - 1) {
        SFX();
        Tile tile = path[currentWayPoint+1];
        unit.tile = path[currentWayPoint];
        tileCurrent = unit.tile;
        unit.direction = tileCurrent.GetDirection( tile );
        if( tileCurrent.floor != tile.floor ) {
          if( tileCurrent.floor.level < tile.floor.level ) {
            GravityVelocity.y = Mathf.Sqrt(-jumpHeight * Physics.gravity.y);
            unit.animationController.JumpUp();
          }else{
            unit.animationController.JumpDown();
          }
        } else {
          unit.animationController.Run();
        }
        currentPosition = path[currentWayPoint].position;
        currentWayPoint++;
      } else {
        followingPath = false;
        currentPosition = path[currentWayPoint].position;
        unit.animationController.Idle();
        transform.position = currentPosition;
        Debug.Log("Has reached its destination");
      }
      yield return null;
    }

    void SFX(){
      if( soundFX != null ) {
        soundFX.Play();
      }
    }

    [ContextMenu("Follow Path")]
    public IEnumerator BuildPath() {
      StopAllCoroutines();
      Pathfinding.ClearSeach();
      Tile currentTile = Board.Grid.GetTileToWorldPosition(transform.position);
      Tile targetTile = Board.Grid.GetTile(objectivePosition);
      List<Tile> tiles = pathfinding.Search(currentTile, pathfinding.ValidateMovement );
      pathfinding.SelectTiles(tiles, unit.color);
      if( pathfinding.tilesSearch.Contains(targetTile) ) {
        currentWayPoint = 0;
        path = pathfinding.BuildPath(targetTile);
        pathfinding.SelectTiles(path, Color.green);
        Tile tile = path[currentWayPoint];
        unit.tile = tile;
        followingPath = true;
        yield return StartCoroutine( Move() );
      } else {
        Debug.Log("Objective not found");
      }
      yield return null;
    }
  }
}
