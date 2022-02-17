using System;
using System.Collections.Generic;
using UnityEngine;
using Board;

namespace IA {
  public class Dijkstra : Pathfinding {
    public int searchLength = 3;
    public override bool ValidateMovement( Tile from, Tile to ) {
      return !(
        to.content != null ||
        to.costFromOrigin > searchLength ||
        Mathf.Abs( from.floor.level - to.floor.level ) > 1
      );
    }
    public override List<Tile> Search(Tile tileStart, Func<Tile, Tile, bool> searchType) {
      ClearSeach();
      tilesSearch = new List<Tile>();
      tilesSearch.Add(tileStart);
      Queue<Tile> checkNow = new Queue<Tile>();
      Queue<Tile> checkNext = new Queue<Tile>();
      tileStart.costFromOrigin = 0;
      checkNow.Enqueue(tileStart);
      while( checkNow.Count > 0) {
        Tile current = checkNow.Dequeue();
        SearchAdjacent(current, checkNext, searchType);
        if( checkNow.Count == 0 ) {
          swapReference( ref checkNow, ref checkNext );
        }
      }
      return tilesSearch;
    }
    private void SearchAdjacent(Tile current, Queue<Tile> checkNext, Func<Tile, Tile, bool> searchType) {
      foreach (Vector3 direction in Board.Grid.directions) {
        Tile next = Board.Grid.GetTile(current.position + direction);
        if( next == null || next.costFromOrigin <= current.costFromOrigin + next.movementCost ) continue;
        next.costFromOrigin = current.costFromOrigin + next.movementCost;
        if( searchType ( current, next ) ) {
          next.previus = current;
          if( !tilesSearch.Contains(next) ) {
            checkNext.Enqueue( next );
            tilesSearch.Add(next);
          }
        }
      }
    }

  }
}
