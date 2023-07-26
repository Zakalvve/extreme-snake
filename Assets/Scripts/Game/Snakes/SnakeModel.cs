using Assets.Scripts.Game.Events;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Core;
using UnityEngine;
using Assets.Scripts.Core.Instance_Control_Patterns;
using System.Collections;
using System.Collections.Generic;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeModel : ISnakeModel
    {
        public Vector2 MoveDirection { get { return _data.MoveDirection; } }
        public Vector2 HeadPosition { get { return _data.Segments.First.Value.ModelPosition; } }
        private int _growth = 0;

        private ISnakeData _data;
        private bool canDraw = false;
        private Vector2Int lastPosition;
        private InstancePooler<SnakeSegment> SegmentPool;

        public SnakeModel(ISnakeData data,Vector2Int spawnPosition, int layer) {
            _data = data;

            SegmentPool = new InstancePooler<SnakeSegment>(() => {
                return CreateEmptySegment(_data.ViewData.BodyPrefab);
            });

            //create segments
            _data.Segments.AddFirst(CreateSegment(CreateEmptySegment(_data.ViewData.HeadPrefab),spawnPosition, layer));
            _data.Segments.AddLast(CreateSegment(SegmentPool.Get(),spawnPosition + Vector2Int.RoundToInt(_data.MoveDirection * -1), layer));
            _growth = _data.StartingLength - 2;
            _data.SnakeEmitter.Subscribe<SnakeCreatedEventArgs>("SnakeCreated",HandleReskin);
        }

        public void AddGrowth(int amount) {
            _growth += amount;
        }

        public void HandleReskin(object sender, SnakeCreatedEventArgs args) {
            _data.ViewData.Sprites = args.Skin; 
            canDraw = true;
            Draw();
        }

        public void Draw() {
            if (!canDraw) return;
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
            lastPosition = _data.Segments.Last.Value.ModelPosition;
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
            if (_growth > 0) {
                _growth--;
                ChangeLength(1);
                released = null;
            }
            LevelPosition claimed = new LevelPosition(_data.Segments.First.Value.ModelPosition,_data.Segments.First.Value.Segment.layer);
            GameManager.Instance.GameEmitter.Emit("OnSnakePositionsChanged",this,new SnakeMoveEventArgs(claimed,released));
        }

        public void Clear() {
            for (int i = 0; i < _data.Segments.Count; i++) {
                RemoveSegement();
            }
        }
        public bool ChangeLength(int amount, bool releaseSegment = false) {
            if (amount > 0) {
                Grow(amount);
            }
            else if (amount < 0) {
                if (_data.Invulnerable) return true;
                return Shrink(Mathf.Abs(amount), releaseSegment);
            }
            return true;
        }

        private bool Shrink(int amount, bool releaseSegment = false) {
            for (int i = 0; i < amount; i++) {
                if (_data.Segments.Count == 2) {
                    _data.IsAlive = false;
                    GameManager.Instance.GameEmitter.Emit("OnSnakeDeath",this, new StringEventArgs(_data.UUID));
                    return false;
                }
                LevelPosition released = new LevelPosition(_data.Segments.Last.Value.ModelPosition,_data.Segments.Last.Value.Segment.layer);
                RemoveSegement(releaseSegment);
                GameManager.Instance.GameEmitter.Emit("OnSnakePositionsChanged",this,new SnakeMoveEventArgs(null,released));
            }
            if (!releaseSegment) _data.SnakeEmitter.Emit("OnFlash",this);
            Draw();
            return true;
        }

        private void Grow(int amount) {
            for (int i = 0; i < amount; i++) {
                _data.Segments.AddLast(CreateSegment(SegmentPool.Get(),lastPosition,_data.Segments.Last.Value.Layer));
            }
        }

        private void RemoveSegement(bool releaseSegment = false) {
            SnakeSegment segment = _data.Segments.Last.Value;
            _data.Segments.RemoveLast();
            if (releaseSegment) {
                if (segment.Renderer != null) {
                    segment.Segment.GetComponent<BoxCollider2D>().isTrigger = true;
                    GameManager.Instance.StartCoroutine(FadeAndDestroy(segment));
                    return;
                }
            }
            segment.Segment.SetActive(false);
            SegmentPool.Return(segment);
        }

        private SnakeSegment CreateEmptySegment(GameObject prefab) {
            return new SnakeSegment(GameObject.Instantiate(prefab));
        }

        private SnakeSegment CreateSegment(SnakeSegment segment,Vector2Int position, int layer) {
            segment.Segment.SetActive(true);
            segment.Segment.transform.localScale = GameManager.Instance.Level.Grid.cellSize * 1.01f;
            segment.Segment.transform.parent = _data.SnakeTransform;
            segment.Segment.transform.position = GameManager.Instance.Level.CenterInCell(position);
            segment.ChangeLayer(layer);
            segment.ModelPosition = position;
            Color color = segment.Renderer.material.color;
            color.a = 1f;
            segment.Renderer.color = color;
            return segment;
        }

        private IEnumerator FadeAndDestroy(SnakeSegment segment) {
           
            for (float fade = 1f; fade >= 0; fade -= 0.1f) {
                Color color = segment.Renderer.material.color;
                color.a = fade;
                segment.Renderer.color = color;
                yield return new WaitForSeconds(0.05f);
            }

            segment.Segment.SetActive(false);
            SegmentPool.Return(segment);
        }
    }
}
