using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    public Shader shader;

    public float totalWidth = 1;
    public float totalHeight = 1;
    public int subdivisionsX = 4;
    public int subdivisionsY = 4;

    private void Start()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if(meshRenderer == null)
        {
            Debug.Log("No msh found");
        }
        meshRenderer.sharedMaterial = new Material(shader);

        var meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = CreateGridMesh();
    }

    private Mesh CreateGridMesh()
    {
        var mesh = new Mesh
        {
            vertices = GenerateVertices(),
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt16
        };

        mesh.SetIndices(GenerateIndices(), MeshTopology.Lines, 0);

        return mesh;
    }

    private Vector3[] GenerateVertices()
    {
        var result = new Vector3[(subdivisionsX + 1) * (subdivisionsY + 1)];

        float stepX = totalWidth / subdivisionsX;
        float stepY = totalHeight / subdivisionsY;
        float negHalfWidth = totalWidth / -2.0F;
        float negHalfHeight = totalHeight / -2.0F;

        for (var j = 0; j < subdivisionsY + 1; j++)
            for (var i = 0; i < subdivisionsX + 1; i++)
            {
                result[(subdivisionsX + 1) * j + i] = new Vector3(negHalfWidth + i * stepX, 0F, negHalfHeight + j * stepY);
            }

        return result;
    }

    private int[] GenerateIndices()
    {
        var result = new int[(subdivisionsX * (subdivisionsY + 1) + subdivisionsY * (subdivisionsX + 1)) * 2];

        // HorizontalEdges
        int indexPointer = 0;

        for (var j = 0; j < subdivisionsY + 1; j++)
            for (var i = 0; i < subdivisionsX; i++)
            {
                result[indexPointer] = (subdivisionsX + 1) * j + i;
                result[indexPointer + 1] = (subdivisionsX + 1) * j + i + 1;
                indexPointer += 2;
            }

        // VerticalEdges
        for (var i = 0; i < subdivisionsX + 1; i++)
            for (var j = 0; j < subdivisionsY; j++)
            {
                result[indexPointer] = (subdivisionsX + 1) * j + i;
                result[indexPointer + 1] = (subdivisionsX + 1) * (j + 1) + i;
                indexPointer += 2;
            }

        return result;
    }
}
