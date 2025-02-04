using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCalculator : MonoBehaviour
{
    [Header("Grid Size")]
    [SerializeField] private Vector2 _gridSize = new Vector2(0.36f, 0.18f);
    [SerializeField] private float _worldYOffset = -0.7f;

    private Vector3 mXDirection;
    private Vector3 mYDirection;
    private Vector3 mZDirection;
    private Vector3 mVerticalDirection;
    private float mUnit;

    private void Awake()
    {
        CalculateFloatingGridConst();
    }

    private void CalculateFloatingGridConst()
    {
        Vector2 displacement = new Vector2(_gridSize.x / 2.0f, _gridSize.y / 2.0f);
        float angleX = Mathf.Atan(displacement.y / displacement.x);
        mUnit = _gridSize.y / Mathf.Cos(angleX);
        angleX *= 180.0f / Mathf.PI;
        Quaternion rotation = Quaternion.Euler(0, 0, angleX);
        mXDirection = rotation * new Vector3(1, 0, 0);
        mYDirection = mXDirection;
        mYDirection.x *= -1;
        mZDirection = new Vector3(0, 1, 0);
        mVerticalDirection = new Vector3(0, 1, 0);
        mVerticalDirection.Normalize();
    }

    public Vector3 GridToWorldSpace(Vector2 pos)
    {
        //Project x and y down to grid height 0
        float zDistance = 0;
        float gridX = pos.x + zDistance;
        float xDistance = gridX * mUnit;
        Vector3 ptOnX = mXDirection * xDistance;

        float gridY = pos.y + zDistance;
        float yDistance = gridY * mUnit;
        Vector3 ptOnY = mYDirection * yDistance;

        //2 ray intersection to find the world pos
        Vector3 worldPos = GetIntersect(ptOnX, mYDirection, ptOnY, mXDirection);

        //worldPos.z = (1 + zDistance) * _cellToWorldZ + _zOffset;
        worldPos.z = 0;
        worldPos.y += _worldYOffset;
        return worldPos;
    }
    private Vector3 GetIntersect(Vector3 s1, Vector3 d1, Vector3 s2, Vector3 d2)
    {
        float u = (s1.y * d2.x + d2.y * s2.x - s2.y * d2.x - d2.y * s1.x) / (d1.x * d2.y - d1.y * d2.x);
        return s1 + u * d1;
    }
}
