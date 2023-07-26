using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.Levels;
using ExtremeSnake.Utils;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.Data;
using System;

namespace ExtremeSnake.Game.Levels
{
    [RequireComponent(typeof(Grid))]
    public class Level : InstanceTracker<Level>// MonoBehaviour
    {
        public GameObject debugGO;
        public Grid Grid;
        public LevelFood foodInLevel;
        public int spawnFrequency = 0;
        private List<LevelLayer> Layers { get; set; }
        private Dictionary<int, LevelLayer> _layers = new Dictionary<int, LevelLayer>();
        private List<SnakeSpawnPointStatus> _snakeSpawners;
        private FoodSpawner _foodSpawner;
        private float elapsed = 0f;
        public bool debug = false;

        protected override void Awake() {
            base.Awake();
            Grid = GetComponent<Grid>();
            Layers = new List<LevelLayer>(GetComponentsInChildren<LevelLayer>());
            GetComponentsInChildren<LevelLayer>().ToList().ForEach(layer => {
                _layers.Add(layer.gameObject.layer,layer);
            });
            _snakeSpawners = GetComponentsInChildren<SnakeSpawner>().Select(spawn => new SnakeSpawnPointStatus(spawn, false)).ToList();
            _foodSpawner = new FoodSpawner(foodInLevel);
        }
        public void Start() {
            GameManager.Instance.GameEmitter.Subscribe("OnTick",Tick);
            GameManager.Instance.GameEmitter.Subscribe<SnakeMoveEventArgs>("OnSnakePositionsChanged",UpdateWalkables);
            GameManager.Instance.GameEmitter.Emit("OnLevelStartComplete",this);
            GameManager.Instance.GameEmitter.Subscribe("OnLoadComplete",(object sender) => {
                for (int i = 0; i < 5; i++) {
                    try {
                        (string layer, Vector2Int cell) = GetRandomWalkableCell();
                        _foodSpawner.Spawn(layer,cell);
                        _layers[LayerMask.NameToLayer(layer)].RegisterPositionNotWalkable((Vector3Int)cell);
                    } catch (Exception e) {
                        Debug.LogWarning($"Caught and recovered from error. {e}");
                    }
                }
            });
        }

        private void Update() {
            elapsed += Time.deltaTime;
            if (elapsed > 0.05f) {
                DrawDebug();
                elapsed = 0f;
            }
        }

        [ContextMenu("Debug")]
        public void DrawDebug() {
            Layers[0].DrawDebug(debugGO,this,Color.red,debug);
            Layers[1].DrawDebug(debugGO,this,Color.blue,debug);
        }

        public ISpawner GetSnakeSpawner() {

            SnakeSpawnPointStatus spStatus = UtilsClass.RandomElement(_snakeSpawners
                .Where(spawner => !spawner.IsAllocated)
                .ToList());

            spStatus.IsAllocated = true;

            return spStatus.Spawner;
        }

        public void Tick(object sender) {
            spawnFrequency++;
            FoodDifficulty diff = GameManager.Instance.Settings.ActiveSession.DifficultySettings.FoodScarcity;
            int numActors = GameManager.Instance.Settings.ActiveSession.Actors.Count;
            int numFoodInLevel = InstanceTracker<Food>.Instances.Count;
            if (spawnFrequency > diff.FoodSpawnRate - (3 * (numActors - 1))) {
                if (numFoodInLevel < (diff.MaxFood*numActors)
                 || diff.MaxFood == 0
                ) {
                    try {
                        (string layer, Vector2Int cell) = GetRandomWalkableCell();
                        _foodSpawner.Spawn(layer,cell);
                        _layers[LayerMask.NameToLayer(layer)].RegisterPositionNotWalkable((Vector3Int)cell);
                    } catch (Exception e) {
                        Debug.LogWarning($"Caught and recovered from error. {e}");
                    }
                }
                else {
                    Debug.Log("Max food reached");
                }
                spawnFrequency = 0;
            }
            
        }

        public Vector2 CenterInCell(int x,int y) {
            return Grid.CellToWorld(new Vector3Int(x,y,1)) + Grid.cellSize / 2;
        }
        public Vector2 CenterInCell(Vector2Int cell) {
            return Grid.CellToWorld(new Vector3Int(cell.x,cell.y,1)) + Grid.cellSize / 2;
        }
        public Vector3 CenterInCell(Vector3Int cell) {
            return Grid.CellToWorld(new Vector3Int(cell.x,cell.y,1)) + Grid.cellSize / 2;
        }

        //can throw error if there is no walkable spaces left
        public (string, Vector2Int) GetRandomWalkableCell() {
            Vector2Int randomCell;
            LevelLayer randomlayer = UtilsClass.RandomElement(_layers.Select(kvp => kvp.Value));
            randomCell = (Vector2Int)randomlayer.GetRandomWalkablePosition();
            return (LayerMask.LayerToName(randomlayer.gameObject.layer),randomCell);
        }

        public void RegisterSnake(List<LevelPosition> SnakeSegmentPositions) {
            SnakeSegmentPositions.ForEach(position => {
                _layers[position.Layer].RegisterPositionNotWalkable((Vector3Int)position.Position);
            });
        }

        public void UpdateWalkables(object sender, SnakeMoveEventArgs args) {
            if (args.Claimed != null)
                _layers[args.Claimed.Layer].RegisterPositionNotWalkable((Vector3Int)args.Claimed.Position);

            if (args.Released != null)
                _layers[args.Released.Layer].RegisterPositionWalkable((Vector3Int)args.Released.Position);
        }

        public bool IsMoveValid(Vector2Int fromPos,int layer) {
            return _layers[layer].IsPositionWalkable((Vector3Int)fromPos);
        }

        public bool IsMoveValid(Vector2 fromPos,Vector2 dir,float distance,int layer) {
            //raycast in the move direction to check if the new cell is valid to move into
            int layerMask = (1 << layer | 1 << LayerMask.NameToLayer("Solid"));
            var hit = Physics2D.Raycast(fromPos,dir,distance,layerMask);

            if (hit.collider != null) {
                if (hit.collider.tag != "Food" && !hit.collider.isTrigger)
                    return false;
            }
            return true;
        }

        //Returns true if the cell given is empty
        public bool IsCellValid(Vector2Int cell) {
            Vector3 cellWorld = CenterInCell(cell);
            Ray ray = new Ray(Camera.main.transform.position,cellWorld - Camera.main.transform.position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null) {
                //debug.Add(() => Debug.DrawRay(Camera.main.transform.position,cellWorld - Camera.main.transform.position,Color.red));
                return false;
            } else {
                //debug.Add(() => Debug.DrawRay(Camera.main.transform.position,cellWorld - Camera.main.transform.position,Color.green));
            }
            return true;
        }
    }

    //used to track which spawn points have and haven't been allocated to snakes
    public class SnakeSpawnPointStatus {
        public SnakeSpawnPointStatus(ISpawner spawner, bool isAllocated) { Spawner = spawner; IsAllocated = isAllocated; }
        public ISpawner Spawner;
        public bool IsAllocated;
    }
}