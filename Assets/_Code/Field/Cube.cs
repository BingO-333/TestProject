using System;
using UnityEngine;

namespace _Code.Field
{
    public class Cube : MonoBehaviour
    {
        public Action<int> OnChangePoints;
        
        [SerializeField] private float _maxMoveSpeed = 3;
        [SerializeField] private float _minMoveSpeed = 0.5f;
        [SerializeField] private float _rotateSpeed = 4;
        
        [Space] 
        [SerializeField] private float _sphereDistanceForDestroy = 0.5f;
        [SerializeField] private float _distanceForSlowDown = 3f;
        
        [Space] [SerializeField] private ParticleSystem _particle;

        private Sphere _currentSphere;

        private int _points;

        private float _currentSpeed;

        private void OnEnable()
        {
            SphereSpawner.Instance.OnAddNewSphere += WaitForNewSphere;
        }

        private void OnDestroy()
        {
            SphereSpawner.Instance.OnAddNewSphere -= WaitForNewSphere;
        }

        private void Update()
        {
            if (_currentSphere == null)
            {
                WaitForNewSphere();
                return;
            }

            SpeedControl();

            Move();
            RotateToSphere();
            
            CheckDistanceForCurrentSphere();
        }

        private void Move()
        {
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
        }

        private void RotateToSphere()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_currentSphere.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
        }

        private void WaitForNewSphere()
        {
            _currentSphere = SphereSpawner.Instance.GetClosestSphere(transform.position);
        }

        private void CheckDistanceForCurrentSphere()
        {
            if (Vector3.Distance(transform.position, _currentSphere.transform.position) <= _sphereDistanceForDestroy)
            {
                Destroy(_currentSphere.gameObject);
                
                AddPoint();
            }
        }

        private void AddPoint()
        {
            _particle.Play();
            
            _points++;
            OnChangePoints?.Invoke(_points);
        }

        private void SpeedControl()
        {
            float speed = 0;
            float distance = Vector3.Distance(transform.position, _currentSphere.transform.position);

            speed = distance < _distanceForSlowDown ? _minMoveSpeed : _maxMoveSpeed;
            
            _currentSpeed = Mathf.Lerp(_currentSpeed, speed, Time.deltaTime);
        }
    }
}
