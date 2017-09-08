using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NostrumGames
{
    public class CameraManager : Singleton<CameraManager>
    {
        protected CameraManager() { }

        public float TerrainAngle { get; set; }

        [SerializeField]
        private bool _useManualSpeed = false;

        [SerializeField]
        private Vector2 _cameraSpeed;
        // private Vector2 _playerSpeed;
        [SerializeField]
        private Transform _player;

        public List<Transform> TargetPoints;

        [SerializeField]
        private int _startingIndex;
        private Transform _currentTarget;
        private int _currentTargetIndex;

        private bool _isInitialized;

        void Awake()
        {
            this.Reload();
            // _playerSpeed = new Vector2(Global.PlayersSpeed, 0);
            _isInitialized = false;
        }

        void Start()
        {
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (_isInitialized) MoveToNextTarget(ref _currentTarget, ref _currentTargetIndex, TargetPoints);
                })
                .AddTo(this);
        }


        //TODO: Change it to pooling later
        private void MoveToNextTarget(ref Transform currentTarget, ref int currentTargetIndex, List<Transform> targetPoints)
        {

            var velocity = AngleSpeed(transform.position, currentTarget.position, GetProperSpeed(_useManualSpeed).x) * Time.deltaTime;

            var distance = currentTarget.transform.position.x - this.transform.position.x;
            if (currentTarget.transform.position.x > this.transform.position.x && velocity < distance)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, velocity);
            }
            else
            {
                if (targetPoints[targetPoints.Count - 1] == currentTarget) return;
                RemoveFromTargetPoints(currentTarget);
                Destroy(currentTarget.gameObject);
                currentTarget = targetPoints[currentTargetIndex];
                // currentTargetIndex += 1;
                this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, velocity);
            }
        }

        private float AngleSpeed(Vector2 currentTarget, Vector2 nextTarget, float vx)
        {
            var angle = Mathf.Abs(AngleBetweenVector2(currentTarget, nextTarget));

            var v0 = vx / Mathf.Cos(angle * Mathf.Deg2Rad);
            var vy = v0 * Mathf.Sin(angle * Mathf.Deg2Rad);

            return Mathf.Sqrt((float)Math.Pow(vx, 2) + (float)Math.Pow(vy, 2));
        }

        private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
        {
            Vector2 diference = vec2 - vec1;
            float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
            return Vector2.Angle(Vector2.right, diference) * sign;
        }

        private void TranslateSpeed(Vector2 speed)
        {
            if (TerrainAngle != 0) this.transform.Translate(Quaternion.Euler(0, 0, TerrainAngle) * speed * Time.deltaTime);
            else this.transform.Translate(speed * Time.deltaTime);
        }

        private Vector2 GetProperSpeed(bool useManual)
        {
            return useManual ? _cameraSpeed : GetPlayerSpeed();
        }

        private Vector2 GetPlayerSpeed()
        {
            return new Vector2(Global.PlayersSpeed, 0);
        }

        public void AddToTargetPoints(Transform newTarget)
        {
            TargetPoints.Add(newTarget);
        }
        private void RemoveFromTargetPoints(Transform oldTarget)
        {
            TargetPoints.Remove(oldTarget);
        }

        public void InitTargets()
        {
            _currentTargetIndex = _startingIndex;
            _currentTarget = TargetPoints[_currentTargetIndex];
            _isInitialized = true;
        }



    }
}
