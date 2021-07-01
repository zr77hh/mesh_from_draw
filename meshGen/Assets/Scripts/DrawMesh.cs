using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider))]
public class DrawMesh : MonoBehaviour
{
	//how many vertices for each loop
	[SerializeField] int loops = 5;
	[SerializeField] float Radius = 0.3f;
	
	Vector3[] points;
	Vector3[] vertices;
    public Mesh Mesh;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();
		Mesh = new Mesh();

	}


	public void GenerateMesh(Vector3[] fingerPos)
	{
        points = fingerPos;
		int verticesLength = loops*points.Length;
		vertices = new Vector3[verticesLength];

		int[] triangles = CalculateTriangles();

		
		int currentVertIndex = 0;

		for (int i = 0; i < points.Length; i++)
		{
			Vector3[] circle = CalculateCircle(i);
			foreach (var vertex in circle)
			{
				vertices[currentVertIndex++] = vertex;
			}
		}
		Mesh.Clear();
		Mesh.vertices = vertices;
		Mesh.triangles = triangles;
		Mesh.RecalculateNormals();
		meshFilter.mesh = Mesh;
		meshCollider.sharedMesh = Mesh;
	}


	private int[] CalculateTriangles()
	{
		
		int[] triangles = new int[points.Length*loops*2*3];

		int currentIndex = 0;
		for (int i = 1; i < points.Length; i++)
		{
			for (int loop = 0; loop < loops; loop++)
			{
				int vertIndex = (i*loops + loop);
				int prevVertIndex = vertIndex - loops;
				// First Triangle
				triangles[currentIndex++] = prevVertIndex;
				if(loop == loops - 1)
				{
                    triangles[currentIndex++] = vertIndex - (loops - 1);
				}
				else
				{
					triangles[currentIndex++] = vertIndex + 1;
				}
				triangles[currentIndex++] = vertIndex;
				

				//Second Triangle
				if(loop == loops - 1)
				{
					triangles[currentIndex++] =prevVertIndex - (loops - 1);
				}
				else
				{
					triangles[currentIndex++] = (prevVertIndex + 1);
				} 
				if(loop == loops - 1)
				{
					triangles[currentIndex++] = vertIndex - (loops - 1);
				}
				else
				{
					triangles[currentIndex++] = (vertIndex + 1);
				}
				triangles[currentIndex++] = prevVertIndex;
			}
		}

		return triangles;
	}

	private Vector3[] CalculateCircle(int index)
	{
		int dirCount = 0;
		Vector3 forward = Vector3.zero;

		// If not first index
		if (index > 0)
		{
			forward += (points[index] - points[index - 1]).normalized;
			dirCount++;
		}

		// If not last index
		if (index < points.Length-1)
		{
			forward += (points[index + 1] - points[index]).normalized;
			dirCount++;
		}

		// Forward is the average of the connecting edges directions
		forward = (forward/dirCount).normalized;
		Vector3 side = Vector3.Cross(forward, forward+new Vector3(1f, 3f, 7f)).normalized;
		Vector3 up = Vector3.Cross(forward, side).normalized;

		Vector3[] circle = new Vector3[loops];
		float angle = 0f;
		float angleStep = (2*Mathf.PI)/loops;

		float t = index / (points.Length-1f);
		

		for (int i = 0; i < loops; i++)
		{
			float x = Mathf.Cos(angle);
			float y = Mathf.Sin(angle);

			circle[i] = points[index] + side*x* Radius + up*y* Radius;

			angle += angleStep;
		}

		return circle;
	}
}
