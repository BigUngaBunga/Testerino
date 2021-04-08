using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    public MeshFilter meshFilter;
    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                TriangulateSquare(squareGrid.squares[x, y]);

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        Generate2DCollider();
    }

    private void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            //One point active
            case 1:
                MeshFromPoints(square.centreBottom, square.bottomLeft, square.centreLeft);
                break;
            case 2:
                MeshFromPoints(square.centreRight, square.bottomRight, square.centreBottom);
                break;
            case 4:
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;
            //Two points active
            case 3:
                MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;
            //Three points active
            case 7:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;
            //All points active
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                break;

            default:
                break;
        }
    }

    private void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    private void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    public void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }

    private void Generate2DCollider()
    {
        
    }


    //DEBUG REMOVE LATER
    private void OnDrawGizmos()
    {
        //if (squareGrid != null)
        //{
        //    for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
        //        {
        //            Gizmos.color = (squareGrid.squares[x, y].topLeft.isActive) ? Color.blue : Color.black;
        //            Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * 0.6f);

        //            Gizmos.color = (squareGrid.squares[x, y].topRight.isActive) ? Color.blue : Color.black;
        //            Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * 0.6f);

        //            Gizmos.color = (squareGrid.squares[x, y].bottomRight.isActive) ? Color.blue : Color.black;
        //            Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * 0.6f);

        //            Gizmos.color = (squareGrid.squares[x, y].bottomLeft.isActive) ? Color.blue : Color.black;
        //            Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.position, Vector3.one * 0.6f);

        //            Gizmos.color = Color.red;
        //            Gizmos.DrawCube(squareGrid.squares[x, y].centreTop.position, Vector3.one * 0.2f);
        //            Gizmos.DrawCube(squareGrid.squares[x, y].centreRight.position, Vector3.one * 0.2f);
        //            Gizmos.DrawCube(squareGrid.squares[x, y].centreLeft.position, Vector3.one * 0.2f);
        //            Gizmos.DrawCube(squareGrid.squares[x, y].centreBottom.position, Vector3.one * 0.2f);
        //        }
        //    }
        //}
    }


    public class SquareGrid
    {
        public Square[,] squares;
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 position = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, -mapHeight / 2 + y * squareSize + squareSize / 2, 0);
                    controlNodes[x, y] = new ControlNode(position, map[x, y] == 0, squareSize);
                }
            }

            squares = new Square[nodeCountX -1, nodeCountY -1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x, y], controlNodes[x + 1, y]);
                }
            }
        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;

        public Square (ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.isActive)
                configuration += 8;
            if (topRight.isActive)
                configuration += 4;
            if (bottomRight.isActive)
                configuration += 2;
            if (bottomLeft.isActive)
                configuration += 1;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;
        
        public Node (Vector3 position)
        {
            this.position = position;
        }
    }

    public class ControlNode : Node
    {
        public bool isActive;
        public Node above, right;

        public ControlNode(Vector3 position, bool isActive, float squareSize) : base (position)
        {
            this.isActive = isActive;
            above = new Node(position + Vector3.up * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }
}
