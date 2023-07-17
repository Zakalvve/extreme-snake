using Assets.Scripts.Game.Events;
using ExtremeSnake.Game.Data;
using System.Security.Claims;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeModel : ISnakeModel
    {
        public Vector2 MoveDirection { get { return _data.MoveDirection; } }
        public Vector2 HeadPosition { get { return _data.Segments.First.Value.ModelPosition; } }

        private ISnakeData _data;

        public SnakeModel(ISnakeData data,Vector2Int spawnPosition, int layer) {
            _data = data;

            //create segments
            _data.Segments.AddFirst(CreateSegment(_data.ViewData.HeadPrefab,spawnPosition, layer));
            _data.Segments.AddLast(CreateSegment(_data.ViewData.BodyPrefab,spawnPosition + Vector2Int.RoundToInt(_data.MoveDirection * -1), layer));
            ChangeLength(_data.StartingLength - 2);
            _data.SnakeEmitter.Subscribe("OnPlayerSnakeCreated",HandleAttachToCamera);
            _data.SnakeEmitter.Subscribe<ReskinEventArgs>("OnReskin",HandleReskin);
        }


        //is this even being used??
        public void HandleAttachToCamera(object sender) {
            GameManager.Instance.GameEmitter.Emit("OnPlayerSnakeCreated",this,new CameraEventArgs(_data.Segments.First.Value.Segment.transform));
            //Camera.main.GetComponent<CameraController>().Initialize(this, new CameraOnEventArgs(_data.Segments.First.Value.Segment.transform));
        }

        public void HandleReskin(object sender, ReskinEventArgs args) {
            _data.ViewData.Sprites = args.Sprites;
            Draw();
        }

        public void Draw() {
            for (var segment = _data.Segments.First; segment != null; segment = segment.Next) {
                segment.Value.Segment.transform.position = GameManager.Instance.Level.CenterInCell(segment.Value.ModelPosition);

                if (segment == _data.Segments.First) segment.Value.Renderer.sprite = _data.ViewData.Sprites.GetHeadSprite(segment);
                else if (segment == _data.Segments.Last) segment.Value.Renderer.sprite = _data.ViewData.Sprites.GetTailSprite(segment);
                else segment.Value.Renderer.sprite = _data.ViewData.Sprites.GetBodySprite(segment);
            }
        }

        public Vector2 UpdateDirection(Vector2 newDirection) {
            _data.MoveDirection = newDirection;
            return Vector2.zero;
        }

        public void Move() {
            LevelPosition released = new LevelPosition(_data.Segments.Last.Value.ModelPosition,_data.Segments.Last.Value.Segment.layer);
            for (var segment = _data.Segments.Last; segment != null; segment = segment.Previous) {
                if (segment.Previous == null) {
                    segment.Value.ModelPosition += Vector2Int.RoundToInt(Vector2Int.RoundToInt(_data.MoveDirection));
                    continue;
                }
                if (segment.Previous.Value.Layer != segment.Value.Layer) {
                    segment.Value.ChangeLayer(segment.Previous.Value.Layer);
                }

                segment.Value.ModelPosition = segment.Previous.Value.ModelPosition;
            }
            LevelPosition claimed = new LevelPosition(_data.Segments.First.Value.ModelPosition,_data.Segments.First.Value.Segment.layer);
            GameManager.Instance.GameEmitter.Emit("OnSnakePositionsChanged",this,new SnakeMoveEventArgs(claimed,released));
        }

        public void ChangeLength(int amount) {
            if (amount > 0) {
                Grow(amount);
            }
            else if (amount < 0) {
                Shrink(Mathf.Abs(amount));
            }
        }

        private void Shrink(int amount) {
            for (int i = 0; i < amount; i++) {
                if (_data.Segments.Count == 2) {
                    GameManager.Instance.GameEmitter.Emit("OnGameOver", this);
                    return;
                }
                LevelPosition released = new LevelPosition(_data.Segments.Last.Value.ModelPosition,_data.Segments.Last.Value.Segment.layer);
                RemoveSegement();
                GameManager.Instance.GameEmitter.Emit("OnSnakePositionsChanged",this,new SnakeMoveEventArgs(null,released));
            }
        }

        private void Grow(int amount) {
            Vector2Int directionOfGrowth = _data.Segments.Last.Value.ModelPosition - _data.Segments.Last.Previous.Value.ModelPosition;
            for (int i = 0; i < amount; i++) {
                _data.Segments.AddLast(CreateSegment(_data.ViewData.BodyPrefab,_data.Segments.Last.Value.ModelPosition + directionOfGrowth,_data.Segments.Last.Value.Layer));
                LevelPosition claimed = new LevelPosition(_data.Segments.First.Value.ModelPosition,_data.Segments.First.Value.Segment.layer);
                GameManager.Instance.GameEmitter.Emit("OnSnakePositionsChanged",this,new SnakeMoveEventArgs(claimed,null));
            }
        }

        private void RemoveSegement() {
            GameObject segmentGO = _data.Segments.Last.Value.Segment;
            _data.Segments.RemoveLast();
            GameObject.Destroy(segmentGO);
        }
        private SnakeSegment CreateSegment(GameObject prefab,Vector2Int position, int layer) {
            GameObject segment = GameObject.Instantiate(prefab);
            segment.transform.localScale = GameManager.Instance.Level.Grid.cellSize;
            segment.transform.parent = _data.SnakeTransform;
            segment.transform.position = GameManager.Instance.Level.CenterInCell(position);
            segment.layer = layer;
            segment.GetComponent<SpriteRenderer>().sortingLayerName = LayerMask.LayerToName(layer);
            return new SnakeSegment(segment,position);
        }

    }
}
