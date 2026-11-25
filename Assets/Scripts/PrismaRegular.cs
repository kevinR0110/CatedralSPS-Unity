using UnityEngine;
using System.Collections.Generic;

// Genera por codigo un prisma regular: un poligono de N lados con una altura.
// lados = 6 da un hexagono. El centro del prisma es su pivote.
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PrismaRegular : MonoBehaviour
{
    public int lados = 6;        // numero de lados (minimo 3)
    public float radio = 2f;     // distancia del centro a cada esquina
    public float altura = 0.8f;  // alto total

    private Mesh malla;

    void OnEnable() { Generar(); }

    void OnValidate()
    {
        if (isActiveAndEnabled) Generar();   // se actualiza al cambiar valores en el Inspector
    }

    [ContextMenu("Generar")]
    public void Generar()
    {
        if (lados < 3) lados = 3;
        if (malla == null) malla = new Mesh();
        malla.Clear();
        malla.name = "PrismaRegular";

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normales = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangulos = new List<int>();

        float mitad = altura / 2f;

        // 1) Esquinas del poligono con seno y coseno
        Vector3[] esquinas = new Vector3[lados];
        for (int i = 0; i < lados; i++)
        {
            float angulo = 2f * Mathf.PI * i / lados;
            esquinas[i] = new Vector3(Mathf.Cos(angulo) * radio, 0f, Mathf.Sin(angulo) * radio);
        }

        // 2) Paredes: un rectangulo (2 triangulos) por cada lado
        for (int i = 0; i < lados; i++)
        {
            Vector3 a = esquinas[i];
            Vector3 b = esquinas[(i + 1) % lados];
            Vector3 normal = ((a + b) / 2f).normalized;   // apunta hacia afuera

            int inicio = vertices.Count;
            vertices.Add(a + Vector3.down * mitad);   // inicio + 0: abajo-a
            vertices.Add(b + Vector3.down * mitad);   // inicio + 1: abajo-b
            vertices.Add(a + Vector3.up * mitad);     // inicio + 2: arriba-a
            vertices.Add(b + Vector3.up * mitad);     // inicio + 3: arriba-b

            for (int k = 0; k < 4; k++) normales.Add(normal);
            uvs.Add(new Vector2(0, 0)); uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1)); uvs.Add(new Vector2(1, 1));

            triangulos.Add(inicio); triangulos.Add(inicio + 2); triangulos.Add(inicio + 3);
            triangulos.Add(inicio); triangulos.Add(inicio + 3); triangulos.Add(inicio + 1);
        }

        // 3) Tapa de arriba y tapa de abajo
        AgregarTapa(vertices, normales, uvs, triangulos, esquinas, mitad, true);
        AgregarTapa(vertices, normales, uvs, triangulos, esquinas, mitad, false);

        malla.SetVertices(vertices);
        malla.SetNormals(normales);
        malla.SetUVs(0, uvs);
        malla.SetTriangles(triangulos, 0);
        malla.RecalculateTangents();
        GetComponent<MeshFilter>().sharedMesh = malla;
    }

    // Tapa: un abanico de triangulos desde el centro (igual que GL_TRIANGLE_FAN)
    void AgregarTapa(List<Vector3> vertices, List<Vector3> normales, List<Vector2> uvs,
                     List<int> triangulos, Vector3[] esquinas, float mitad, bool arriba)
    {
        float y = arriba ? mitad : -mitad;
        Vector3 normal = arriba ? Vector3.up : Vector3.down;

        int centro = vertices.Count;
        vertices.Add(new Vector3(0f, y, 0f));
        normales.Add(normal);
        uvs.Add(new Vector2(0.5f, 0.5f));

        for (int i = 0; i < esquinas.Length; i++)
        {
            vertices.Add(new Vector3(esquinas[i].x, y, esquinas[i].z));
            normales.Add(normal);
            uvs.Add(new Vector2(esquinas[i].x / radio * 0.5f + 0.5f, esquinas[i].z / radio * 0.5f + 0.5f));
        }

        for (int i = 0; i < esquinas.Length; i++)
        {
            int actual = centro + 1 + i;
            int siguiente = centro + 1 + (i + 1) % esquinas.Length;

            if (arriba) { triangulos.Add(centro); triangulos.Add(siguiente); triangulos.Add(actual); }
            else { triangulos.Add(centro); triangulos.Add(actual); triangulos.Add(siguiente); }
        }
    }
}