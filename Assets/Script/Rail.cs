using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool _isLoop;

    private float _lenght;

    public List<Transform> _transformList;

    private void Start()
    {
        for (int i = 0; i < _transformList.Count - 1; i++)
        {
            _lenght += Vector3.Distance(_transformList[i].position, _transformList[i + 1].position);
        }
    }

    public float GetLength() { return _lenght; }

    public Vector3 GetPosition(float distance)
    {
        float lastLenght = 0;
        for (int i = 0; i < _transformList.Count - 1; i++)
        {
            float nextLenght = Vector3.Distance(_transformList[i].position, _transformList[i + 1].position);
            if (lastLenght + nextLenght >= _lenght)
            {
                float percent = (distance - lastLenght) / (nextLenght - lastLenght);
                return Vector3.Lerp(_transformList[i].position, _transformList[i + 1].position, percent);
            }
            else
            {
                lastLenght += nextLenght;
            }
        }

        return _transformList[_transformList.Count - 1].position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _transformList.Count - 1; i++)
        {
            Gizmos.DrawLine(_transformList[i].position, _transformList[i + 1].position);
        }
    }
}
