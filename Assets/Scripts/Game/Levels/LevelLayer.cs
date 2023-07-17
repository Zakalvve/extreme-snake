using ExtremeSnake.Utils;
using Mono.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Levels
{
    public class LevelLayer : MonoBehaviour
    {
        private HashSet<Vector3Int> _walkablePositions = new HashSet<Vector3Int>();

        private void Awake() {
            List<Tilemap> maps = new List<Tilemap>(GetComponentsInChildren<Tilemap>());
            List<Tilemap>  BackgroundMaps = maps.Where(map => map.tag == "Background").ToList();
            List<Tilemap> SolidMaps = maps.Where(map => map.tag == "Solid").ToList();
            foreach (Tilemap t in BackgroundMaps) {
                _walkablePositions.UnionWith(GetTilePositions(t));
            }
            foreach(Tilemap t in SolidMaps) {
                _walkablePositions.ExceptWith(GetTilePositions(t));
            }
        }

        public Vector3Int GetRandomWalkablePosition() {
            return UtilsClass.RandomElement(_walkablePositions);
        }

        public bool IsPositionWalkable(Vector3Int position) {
            return _walkablePositions.Contains(position);
        }

        public bool RegisterPositionWalkable(Vector3Int position) {
            return _walkablePositions.Add(position);
        }

        public bool RegisterPositionNotWalkable(Vector3Int position) {
            return _walkablePositions.Remove(position);
        }

        private HashSet<Vector3Int> GetTilePositions(Tilemap tilemap) {
            int totalTiles = tilemap.GetTilesRangeCount(tilemap.cellBounds.min,tilemap.cellBounds.max);
            TileBase[] tiles = new TileBase[totalTiles];
            Vector3Int[] positions = new Vector3Int[totalTiles];
            tilemap.GetTilesRangeNonAlloc(tilemap.cellBounds.min,tilemap.cellBounds.max,positions,tiles);
            return new HashSet<Vector3Int>(positions);
        }
    }
}
