using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    [CreateAssetMenu(menuName = "My Assets/SnakeSpites")]
    public class SnakeSprites : ScriptableObject
    {
        public Sprite HeadUp;
        public Sprite HeadDown;
        public Sprite HeadLeft;
        public Sprite HeadRight;

        public Sprite TailUp;
        public Sprite TailDown;
        public Sprite TailLeft;
        public Sprite TailRight;

        public Sprite TopLeft;
        public Sprite TopRight;
        public Sprite BottomLeft;
        public Sprite BottomRight;

        public Sprite Horizontal;
        public Sprite Vertical;

        public Sprite GetHeadSprite(LinkedListNode<SnakeSegment> head) {
            if (head.Next == null) throw new Exception("The head passed is invalid");
            Vector2Int headDirection = Utils.UtilsClass.DirectionTo(head.Next.Value.ModelPosition,head.Value.ModelPosition);

            if (headDirection == Vector2Int.up) {
                return HeadUp;
            }
            else if (headDirection == Vector2Int.down) {
                return HeadDown;
            }
            else if (headDirection == Vector2Int.left) {
                return HeadLeft;
            }
            else if (headDirection == Vector2Int.right) {
                return HeadRight;
            }
            return null;
        }
        public Sprite GetTailSprite(LinkedListNode<SnakeSegment> tail) {
            if (tail.Previous == null) throw new Exception("The tail passed is invalid");
            Vector2Int headDirection = Utils.UtilsClass.DirectionTo(tail.Value.ModelPosition,tail.Previous.Value.ModelPosition);

            if (headDirection == Vector2Int.up) {
                return TailDown;
            }
            else if (headDirection == Vector2Int.down) {
                return TailUp;
            }
            else if (headDirection == Vector2Int.left) {
                return TailRight;
            }
            else if (headDirection == Vector2Int.right) {
                return TailLeft;
            }
            return null;
        }

        public Sprite GetBodySprite(LinkedListNode<SnakeSegment> body) {
            if (body.Previous == null || body.Next == null) throw new Exception("The body passed is invalid");
            Vector2Int dirToNext = Utils.UtilsClass.DirectionTo(body.Value.ModelPosition,body.Next.Value.ModelPosition);
            Vector2Int dirFromPrevious = Utils.UtilsClass.DirectionTo(body.Previous.Value.ModelPosition,body.Value.ModelPosition);

            if (dirToNext == dirFromPrevious) {
                if (dirToNext == Vector2.up || dirToNext == Vector2.down) {
                    return Vertical;
                }
                if (dirToNext == Vector2.right || dirToNext == Vector2.left) {
                    return Horizontal;
                }
            }
            else {
                if (dirFromPrevious == Vector2.right && dirToNext == Vector2.up || dirFromPrevious == Vector2.down && dirToNext == Vector2.left) {
                    return TopLeft;
                }
                else if (dirFromPrevious == Vector2.left && dirToNext == Vector2.up || dirFromPrevious == Vector2.down && dirToNext == Vector2.right) {
                    return TopRight;
                }
                else if (dirFromPrevious == Vector2.right && dirToNext == Vector2.down || dirFromPrevious == Vector2.up && dirToNext == Vector2.left) {
                    return BottomLeft;
                }
                else if (dirFromPrevious == Vector2.left && dirToNext == Vector2.down || dirFromPrevious == Vector2.up && dirToNext == Vector2.right) {
                    return BottomRight;
                }
            }
            return null;
        }
    }
}
