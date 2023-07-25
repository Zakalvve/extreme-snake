using ExtremeSnake.Utils;
using Mono.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts.Core;
using ExtremeSnake.Game.Levels;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Assets.Scripts.Levels
{
    public class LevelLayer : MonoBehaviour
    {
        //we will be add to and removing from _walkablePositions as things move
        //by storing the initial state of the layer we can cross reference additions to ensure they are allowed
        private HashSet<Vector3Int> _initialState = new HashSet<Vector3Int>();
        private HashSet<Vector3Int> _walkablePositions;

        private void Awake() {
            List<Tilemap> maps = new List<Tilemap>(GetComponentsInChildren<Tilemap>());
            List<Tilemap>  BackgroundMaps = maps.Where(map => map.tag == "Background").ToList();
            List<Tilemap> SolidMaps = maps.Where(map => map.tag == "Solid").ToList();
            foreach (Tilemap t in BackgroundMaps) {
                _initialState.UnionWith(GetTilePositions(t));
            }
            foreach(Tilemap t in SolidMaps) {
                _initialState.ExceptWith(GetTilePositions(t));
            }

            _walkablePositions = new HashSet<Vector3Int>(_initialState);
        }

        public Vector3Int GetRandomWalkablePosition() {
            HashSet<Vector3Int> walkable = new HashSet<Vector3Int>(_walkablePositions);
            HashSet<Vector3Int> foodPositions = new HashSet<Vector3Int>(InstanceTracker<Food>.Instances.Select(food => Vector3Int.FloorToInt(food.gameObject.transform.position)));
            walkable.ExceptWith(foodPositions);
            if (walkable.Count <= 1) {
                throw new System.Exception("No walkable positions");
            }
            return UtilsClass.RandomElement(walkable);
        }

        private List<GameObject> debugObjects = new List<GameObject>();
        public void DrawDebug(GameObject debugPrefab, object level, Color c, bool debug) {
            if (debugObjects.Count > 0) {
                debugObjects.ForEach(go => {
                    GameObject.Destroy(go);
                });
            }
            if (debug) {
                Level l = (Level)level;
                _walkablePositions.ToList().ForEach(position => {
                    GameObject go = GameObject.Instantiate(debugPrefab,l.CenterInCell(Vector3Int.FloorToInt((Vector3)position)),Quaternion.identity);
                    go.GetComponent<SpriteRenderer>().color = c;
                    go.layer = LayerMask.NameToLayer("Layer 3");
                    go.GetComponent<SpriteRenderer>().sortingLayerName = LayerMask.LayerToName(go.layer);
                    debugObjects.Add(go);
                });
            }
        }

        public bool IsPositionWalkable(Vector3Int position) {
            return _walkablePositions.Contains(position);
        }

        public bool RegisterPositionWalkable(Vector3Int position) {
            if (_initialState.Contains(position))
                return _walkablePositions.Add(position);

            return false;
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
