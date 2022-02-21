using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA {
  public abstract class Pathfinding : MonoBehaviour {

    public Board board;

    public Transform prefabPathContainer;
    public Transform prefabPath;
    public Vector2Int initialPosition;
    public List<Tile> tilesSearch = new List<Tile>();
    public abstract List<Tile> Search(Tile tileStart, Func<Tile, Tile, bool> searchType);

    public virtual bool ValidateMovement( Tile from, Tile to ) {
      return (
        to.obj != null
      );
    }

    public void ClearSeach() {
      foreach( Tile tile in board.tiles.Values ) {
        // if( tile.obj != null ) continue;
        tile.costFromOrigin = int.MaxValue;
        tile.costToObjective = int.MaxValue;
        tile.costTotal = int.MaxValue;
        tile.previus = null;
      }
    }
    public List<Tile> BuildPath( Tile lastTile ) {
      List<Tile> path = new List<Tile>();
      Tile tile = lastTile;
      while (tile.previus != null) {
        path.Add(tile);
        tile = tile.previus;
      }
      path.Add(tile);
      path.Reverse();
      return path;
    }

    public void SelectTiles(List<Tile> tiles, Color color) {
      DeSelectTiles(tiles, color);
      foreach( Tile tile in tiles ) {
        Transform instantiated = Instantiate(prefabPath, tile.position, Quaternion.identity, prefabPathContainer);
        instantiated.Find("LED").transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        instantiated.name = instantiated.name.Replace("(Clone)", "");
        tile.color = color;
      }
    }

    public void DeSelectTiles(List<Tile> tiles, Color color) {
      int i = 0;
      foreach(Transform child in prefabPathContainer){
        Destroy(child.gameObject);
        i++;
      }
      foreach( Tile tile in tiles ) {
        tile.color = color;
      }
    }

    protected void swapReference( ref Queue<Tile> checkNow, ref Queue<Tile> checkNext ) {
      Queue<Tile> temp = checkNow;
      checkNow = checkNext;
      checkNext = temp;
    }

  }
}
