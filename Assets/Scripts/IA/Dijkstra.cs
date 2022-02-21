using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace IA {
  public class Dijkstra : Pathfinding {
    public int searchLength = 30;
    private Vector2[] directions = new Vector2[4]{
      Vector2.up,
      Vector2.right,
      Vector2.down,
      Vector2.left
    };
    private int findType;
    public override bool ValidateMovement( Tile from, Tile to ) {
      return !(
        to.item.type != from.item.type ||
        to.costFromOrigin > searchLength
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
      foreach (Vector2 direction in directions) {
        Tile next = board.GetTile(current.position + direction);
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

    [ContextMenu("Follow Path")]
    public void BuildPath() {
      Debug.Log("Iniciar");
      Tile currentTile = board.GetTile(initialPosition);
      Tile targetTile = board.GetTile(Vector2.zero);
      List<Tile> tiles = Search(currentTile, ValidateMovement );
      SelectTiles(tiles, Color.red);
      Debug.Log(tiles.Count);
      if( tilesSearch.Contains(targetTile) ) {
        SelectTiles(tiles, Color.red);
      } else {
        Debug.Log("Objective not found");
      }
    }

  }
}
