using _Code.Field;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class PointsUI : MonoBehaviour
    {
        [SerializeField] private Text _points;
        [SerializeField] private Cube _cube;

        private void Start()
        {
            _cube.OnChangePoints += UpdatePointsUI;
        }

        private void UpdatePointsUI(int count)
        {
            _points.text = count.ToString();
        }
    }
}
