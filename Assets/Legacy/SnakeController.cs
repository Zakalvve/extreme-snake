//using ExtremeSnake.Core;
//using UnityEngine;

//namespace ExtremeSnake.Game.Snakes
//{
//    public class SnakeController : MonoBehaviour
//    {
//        [SerializeField]
//        private GameObject SegmentPrefab;
//        [SerializeField]
//        private GameObject HeadSegmentPrefab;
//        public EventEmitter<SnakeEventArgs> Emitter { get; private set; }

//        private Vector2Int _startPosition;
//        private Vector2 moveDirection = Vector2.right;
//        private Vector2 newDirection = Vector2.zero;
//        private SnakeModelv1 _snakeModel;
//        private SnakeView _snakeView;
//        private EventEmitter<Vector2> _controller;
//        // Start is called before the first frame update
//        void Start() {
//            Emitter = new EventEmitter<SnakeEventArgs>();
//            _startPosition = (Vector2Int)GameManager.Instance.Level.Grid.WorldToCell(gameObject.transform.position);
//            _snakeView = new SnakeView(gameObject,HeadSegmentPrefab,SegmentPrefab,Emitter);
//            _snakeModel = new SnakeModelv1(_startPosition,Vector2.right,Emitter);
//            _controller = GameManager.Instance.RequestController();
//            _controller.Subscribe("OnChangeDirection",HandleChangeDirection);
//            GameManager.Instance.GameEmitter.Subscribe("OnTick",HandleTick);
//        }

//        public void HandleTick() {
//            if (newDirection != Vector2.zero) {
//                moveDirection = newDirection;
//                newDirection = Vector2.zero;
//            }
//            Emitter.Emit("OnMoveSnake",new SnakeEventArgs(moveDirection));
//        }

//        public void HandleChangeDirection(Vector2 direction) {
//            if (moveDirection * -1 != direction) {
//                newDirection = direction;
//            }
//        }
//    }
//}
