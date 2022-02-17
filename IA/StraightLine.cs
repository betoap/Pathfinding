using UnityEngine;

//strategy
namespace IA {
  public class StraightLine : MonoBehaviour {
    public Vector3Int initialPosition;
    public Vector3Int targetPosition;

    [ContextMenu("Search")]
    void Search() {
      int xMovement = targetPosition.x > initialPosition.x ? 1 : -1;
      int yMovement = targetPosition.y > initialPosition.y ? 1 : -1;
      Vector3Int currentPosition = initialPosition;
      while (currentPosition.x != targetPosition.x) {
        Board.Tile tile = Board.Grid.GetTile(currentPosition);
        tile.color = Color.green;
        currentPosition.x += xMovement;
      }
      while (currentPosition.y != targetPosition.y) {
        Board.Tile tile = Board.Grid.GetTile(currentPosition);
        tile.color = Color.yellow;
        currentPosition.y += yMovement;
      }
    }
  }
}
