using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Code.Field
{
    public class SphereSpawner : MonoBehaviour
    {
        #region Singleton

        public static SphereSpawner Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning($"The extra {name}, has been destroyed");
                Destroy(gameObject);
            }
        }

        #endregion

        public Action OnAddNewSphere;
        
        [SerializeField] private float _fieldSize = 8;
        [SerializeField] private float _spheresMaxCount = 20;
        [Space]
        [SerializeField] private Sphere _spherePrefab;
    
        private readonly List<Sphere> _spheres = new List<Sphere>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                SpawnSphere();
        }

        public Sphere GetClosestSphere(Vector3 pos)
        {
            if (_spheres.Count == 0)
                return null;
            
            Sphere potentiallyClosestSphere = _spheres[0];
            float smallestDistance = Vector3.Distance(pos, _spheres[0].transform.position);
            
            foreach (var sphere in _spheres)
            {
                float currentDistance = Vector3.Distance(pos, sphere.transform.position);
                
                if (currentDistance < smallestDistance)
                {
                    smallestDistance = currentDistance;
                    potentiallyClosestSphere = sphere;
                }
            }

            return potentiallyClosestSphere;
        }

        private void RemoveSphere(Sphere sphere)
        {
            _spheres.Remove(sphere);
        }

        private void SpawnSphere()
        {
            if (_spheres.Count > _spheresMaxCount)
                return;
        
            Sphere sphere = Instantiate(_spherePrefab, transform);
        
            sphere.SetRandomPosition(_fieldSize);
            sphere.OnDestroyAction += RemoveSphere;
        
            _spheres.Add(sphere);

            OnAddNewSphere?.Invoke();
        }
    }
}
