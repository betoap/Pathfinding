using System;
using System.Collections.Generic;
using UnityEngine;
using Board;

namespace IA {
  public class AStar : Pathfinding {
    public float heuristicsForce = 2f;
    public Vector3 objectivePosition;
    private Tile targetTile;

    private void Awake() {
      targetTile = Board.Grid.GetTile(objectivePosition);
    }
    public override List<Tile> Search(Tile tileStart, Func<Tile, Tile, bool> searchType) {
      ClearSeach();
      tilesSearch = new List<Tile>();
      Tile current;
      List<Tile> openSet = new List<Tile>();
      openSet.Add(tileStart);
      tileStart.costFromOrigin = 0;
      while( openSet.Count > 0 ) {
        openSet.Sort( ( Tile tileOld, Tile tileCurrent ) => tileOld.costTotal.CompareTo( tileCurrent.costTotal ) );
        current = openSet[0];
        if( current == targetTile ) {
          tilesSearch.Add(current);
          Debug.Log("Found the objective");
          break;
        }
        openSet.RemoveAt(0);
        tilesSearch.Add(current);
        SearchAdjacent(current, targetTile, openSet, searchType);
      }
      return tilesSearch;
    }
    public void SearchAdjacent(Tile current, Tile objective, List<Tile> openSet, Func<Tile, Tile, bool> searchType) {
      foreach (Vector3 item in Board.Grid.directions) {
        Tile next = Board.Grid.GetTile(current.position + item);
        if( next == null || next.costFromOrigin <= current.costFromOrigin + next.movementCost ) continue;
        if( searchType ( current, next ) ) {
          next.costFromOrigin = current.costFromOrigin + next.movementCost;
          next.previus = current;
          next.costToObjective = Vector3.Distance(next.position, objective.position) * heuristicsForce;
          next.costTotal = next.costToObjective + next.costFromOrigin;
          if( !tilesSearch.Contains(next) ) {
            openSet.Add(next);
          }
        }
      }
    }

  }
}
