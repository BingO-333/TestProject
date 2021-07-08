using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Code.Field
{
    public class Sphere : MonoBehaviour
    {
        public Action<Sphere> OnDestroyAction;
    
        public void SetRandomPosition(float offset)
        {
            Vector3 pos = Vector3.zero;

            pos.x = Random.Range(-offset, offset);
            pos.z = Random.Range(-offset, offset);

            transform.position = pos;
        }

        private void OnDestroy() => OnDestroyAction?.Invoke(this);
    }
}
