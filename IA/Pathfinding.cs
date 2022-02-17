using System;
using System.Collections.Generic;
using UnityEngine;
using Board;
using GameController;

namespace IA {
  public abstract class Pathfinding : MonoBehaviour {

    public Transform prefabPathContainer;
    public Transform prefabPath;
    public Vector2Int initialPosition;
    public List<Tile> tilesSearch = new List<Tile>();
    public abstract List<Tile> Search(Tile tileStart, Func<Tile, Tile, bool> searchType);

    public virtual bool ValidateMovement( Tile from, Tile to ) {
      Debug.Log("to.content: " + to.content);
      Debug.Log("from.floor.level: " + from.floor.level);
      Debug.Log("to.floor.level: " + to.floor.level);
      return !(
        to.content != null ||
        Mathf.Abs( from.floor.level - to.floor.level ) > 1
      );
    }

    public static void ClearSeach() {
      foreach( Tile tile in Board.Grid.instance.tiles.Values ) {
        if( tile.content != null ) continue;
        tile.costFromOrigin = int.MaxValue;
        tile.costToObjective = int.MaxValue;
        tile.costTotal = int.MaxValue;
        tile.previus = null;
      }
    }
    public List<Tile> BuildPath( Tile lastTile ) {
      List<Tile> path = new List<Tile>();
      Tile tile = lastTile;
      while (tile.previus != null && tile != Turn.unit.tile) {
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
