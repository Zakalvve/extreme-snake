//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace ExtremeSnake.Game.Snakes
//{
//    public class SnakeEventArgs
//    {
//        //properties
//        public LinkedList<Vector2Int> Model { get; private set; }
//        public Vector2 MoveDirection { get; private set; }
//        public int Value { get; set; }


//        public SnakeEventArgs() : this(null, Vector2.zero) { }
//        public SnakeEventArgs(int foodValue) : this(null,Vector2.zero,foodValue) { }
//        public SnakeEventArgs(LinkedList<Vector2Int> model) : this(model, Vector2.zero) { }
//        public SnakeEventArgs(Vector2 moveDirection) : this(null,moveDirection) { }
//        public SnakeEventArgs(LinkedList<Vector2Int> model, Vector2 moveDirection, int foodValue = 0) {
//            Model = model;
//            MoveDirection = moveDirection;
//            Value = foodValue;
//        }
//    }
//}
