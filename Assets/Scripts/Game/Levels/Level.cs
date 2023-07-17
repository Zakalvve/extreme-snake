using ExtremeSnake.Game.Food;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Assets.Scripts.Core;
using Assets.Scripts.Levels;
using static UnityEditor.Experimental.GraphView.GraphView;
using ExtremeSnake.Utils;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.Data;

namespace ExtremeSnake.Game.Levels
{
    [RequireComponent(typeof(Grid))]
    public class Level : InstanceTracker<Level>// MonoBehaviour
    {
        public Grid Grid;
        public LevelFood foodInLevel;
        public int spawnFrequency = 0;
        private List<LevelLayer> Layers { get; set; }
        private Dictionary<int, LevelLayer> _layers = new Dictionary<int, LevelLayer>();
        private List<SnakeSpawnPointStatus> _snakeSpawners;
        private FoodSpawner _foodSpawner;

        //globalSnakeSpeed; cells per second
        //snakeSpeed = globalSnakeSpeed * snakeSpeedModifiers;

        protected override void Awake() {
            base.Awake();
            Grid = GetComponent<Grid>();
            //Layers = new List<LevelLayer>(GetComponentsInChildren<LevelLayer>());
            GetComponentsInChildren<LevelLayer>().ToList().ForEach(layer => {
                _layers.Add(layer.gameObject.layer,layer);
            });
            _snakeSpawners = GetComponentsInChildren<SnakeSpawner>().Select(spawn => new SnakeSpawnPointStatus(spawn, false)).ToList();
            _foodSpawner = new FoodSpawner(foodInLevel);
        }
        public void Start() {
            GameManager.Instance.GameEmitter.Emit("OnLevelAwake",this);
            GameManager.Instance.GameEmitter.Subscribe("OnTick",Tick);
            GameManager.Instance.GameEmitter.Subscribe<SnakeMoveEventArgs>("OnSnakePositionsChanged",UpdateWalkables);
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
            if (spawnFrequency > 15) {
                (string layer, Vector2Int cell) = GetRandomWalkableCell();
                _foodSpawner.Spawn(layer, cell);
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

        
        //MUST be changed risk of inifinite loop??!!
        public (string, Vector2Int) GetRandomWalkableCell() {
            //risk of infinite loop!!
            Vector2Int randomCell;
            //LevelLayer randomlayer = _layers[Random.Range(0,_layers.Count)];
            LevelLayer randomlayer = UtilsClass.RandomElement(_layers.Select(kvp => kvp.Value));


            do {
                randomCell = (Vector2Int)randomlayer.GetRandomWalkablePosition();
            } while (!IsCellValid(randomCell));

            return (LayerMask.LayerToName(randomlayer.gameObject.layer),randomCell);
        }

        public void RegisterSnake(List<LevelPosition> SnakeSegmentPositions) {
            SnakeSegmentPositions.ForEach(position => {
                Debug.Log(_layers[position.Layer].RegisterPositionNotWalkable((Vector3Int)position.Position));
            });
        }

        public void UpdateWalkables(object sender, SnakeMoveEventArgs args) {
            Debug.Log("Updating collisioon map");
            if (args.Claimed != null)
                _layers[args.Claimed.Layer].RegisterPositionNotWalkable((Vector3Int)args.Claimed.Position);

            if (args.Released != null)
                _layers[args.Released.Layer].RegisterPositionWalkable((Vector3Int)args.Released.Position);
        }

        public bool IsMoveValid(Vector2Int fromPos,int layer) {
            return _layers[layer].IsPositionWalkable((Vector3Int)fromPos);
        }

        #region UpdateToRemoveUseOfRaycast
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
            if (Physics2D.GetRayIntersection(ray)) return false;
            return true;
        }
        #endregion
    }

    //used to track which spawn points have and haven't been allocated to snakes
    public class SnakeSpawnPointStatus {
        public SnakeSpawnPointStatus(ISpawner spawner, bool isAllocated) { Spawner = spawner; IsAllocated = isAllocated; }
        public ISpawner Spawner;
        public bool IsAllocated;
    }
}