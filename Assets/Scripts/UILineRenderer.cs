using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// from "Creating a UI Line Renderer in Unity" by Game Dev Guide : https://www.youtube.com/watch?v=--LB7URk60A&ab_channel=GameDevGuide

public class UILineRenderer : Graphic
{
    public Vector2Int gridSize;
    public UIGridRenderer grid;

    public List<Vector2> points;

    float width, height, unitWidth, unitHeight;

    public float thickness = 1f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

        if (points.Count < 2)
        {
            return;
        }

        float angle = 0;

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];

            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 45f;
            }

            DrawVerticesForPoint(point, vh, angle);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }

    }

    public float GetAngle(Vector2 me, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI));
    }

    void DrawVerticesForPoint(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
        
        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }
    //protected override void Start()
    //{
    //    gridSize = grid.gridSize;
    //}

    private void Update()
    {
        if (grid != null)
        {
            if (gridSize != grid.gridSize)
            {
                //GetComponent<RectTransform>().rect.width = grid.gridSize.x;
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }
    }
}
