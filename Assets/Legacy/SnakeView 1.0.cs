//using ExtremeSnake.Core;
//using System.Collections.Generic;
//using UnityEngine;

//namespace ExtremeSnake.Game.Snakes
//{
//    public class SnakeView
//    {
//        private EventEmitter<SnakeEventArgs> _emitter;
//        private GameObject _snakeRoot;
//        //the head prefab is the segment that has a rigidbody
//        private GameObject _headPrefab;
//        private GameObject _segmentPrefab;
//        private LinkedList<GameObject> _snakeSegments = new LinkedList<GameObject>();
//        public SnakeView(GameObject root,GameObject headPrefab,GameObject segmentPrefab,EventEmitter<SnakeEventArgs> emitter) {
//            _snakeRoot = root;
//            _segmentPrefab = segmentPrefab;
//            _emitter = emitter;
//            _headPrefab = headPrefab;
//            //create the head of the snake
//            CreateSegment(_headPrefab);
//            _emitter.Subscribe("OnModelChanged",HandleSync);
//        }
//        public void HandleSync(SnakeEventArgs args) {
//            if (args.Model == null) throw new System.Exception("No snake model to view");

//            if (_snakeSegments.Count != args.Model.Count) {
//                int difference = args.Model.Count - _snakeSegments.Count;
//                AlterSegments(difference);
//            }

//            DrawSnake(args.Model);
//        }
//        public void DrawSnake(LinkedList<Vector2Int> snakeModel) {
//            LinkedListNode<Vector2Int> modelNode = snakeModel.First;
//            foreach (var segment in _snakeSegments) {
//                segment.transform.position = GameManager.Instance.Level.CenterInCell(modelNode.Value.x,modelNode.Value.y);
//                modelNode = modelNode.Next;
//            }
//        }
//        private void AlterSegments(int number) {
//            if (number > 0) {
//                for (int i = 0; i < number; i++) {
//                    CreateSegment(_segmentPrefab);
//                }
//            }
//            else if (number < 0) {
//                number = Mathf.Abs(number);
//                for (int i = 1; i <= number; i++) {
//                    GameObject toRemove = _snakeSegments.Last.Value;
//                    _snakeSegments.RemoveLast();
//                    GameObject.Destroy(toRemove);
//                }
//            }
//        }
//        private void CreateSegment(GameObject prefab) {
//            GameObject segment = GameObject.Instantiate(prefab);
//            segment.transform.localScale = GameManager.Instance.Level.Grid.cellSize;
//            segment.transform.parent = _snakeRoot.transform;
//            _snakeSegments.AddLast(segment);
//        }
//    }
//}