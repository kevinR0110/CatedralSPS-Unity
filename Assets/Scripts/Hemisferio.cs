using UnityEngine;
using System.Collections.Generic;

// Genera una media esfera (domo) por codigo.
// Con caraInterior = true se ve desde adentro (para el techo interior de la cupula).
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Hemisferio : MonoBehaviour
{
    public float radio = 5f;
    public int sectores = 32;          // divisiones alrededor
    public int anillos = 12;           // divisiones de abajo hacia arriba
    public bool caraInterior = false;  // true: se ve desde adentro

    private Mesh malla;

    void OnEnable() { Generar(); }

    void OnValidate()
    {
        if (isActiveAndEnabled) Generar();   // se actualiza al cambiar valores en el Inspector
    }

    [ContextMenu("Generar")]
    public void Generar()
    {
        if (malla == null) malla = new Mesh();
        malla.Clear();
        malla.name = "Hemisferio";

        int columnas = sectores + 1;
        Vector3[] vertices = new Vector3[(anillos + 1) * columnas];
        Vector3[] normales = new Vector3[vertices.Length];
        Vector2[] uvs = new Vector2[vertices.Length];

        int v = 0;
        for (int i = 0; i <= anillos; i++)
        {
            float anguloVertical = (Mathf.PI / 2f) * i / anillos;   // 0 = borde de abajo, 90 grados = punta
            float y = Mathf.Sin(anguloVertical);
            float r = Mathf.Cos(anguloVertical);

            for (int j = 0; j <= sectores; j++)
            {
                float anguloSector = 2f * Mathf.PI * j / sectores;
                Vector3 punto = new Vector3(r * Mathf.Cos(anguloSector), y, r * Mathf.Sin(anguloSector));

                vertices[v] = punto * radio;
                normales[v] = caraInterior ? -punto : punto;        // la normal apunta hacia adentro o hacia afuera
                uvs[v] = new Vector2((float)j / sectores, (float)i / anillos);
                v++;
            }
        }

        List<int> triangulos = new List<int>();
        for (int i = 0; i < anillos; i++)
        {
            for (int j = 0; j < sectores; j++)
            {
                int a = i * columnas + j;    // abajo-izquierda
                int b = a + columnas;        // arriba-izquierda

                if (!caraInterior)
                {
                    triangulos.Add(a); triangulos.Add(b); triangulos.Add(a + 1);
                    triangulos.Add(a + 1); triangulos.Add(b); triangulos.Add(b + 1);
                }
                else
                {
                    // orden invertido para que la cara visible sea la de adentro
                    triangulos.Add(a); triangulos.Add(a + 1); triangulos.Add(b);
                    triangulos.Add(a + 1); triangulos.Add(b + 1); triangulos.Add(b);
                }
            }
        }

        malla.vertices = vertices;
        malla.normals = normales;
        malla.uv = uvs;
        malla.triangles = triangulos.ToArray();
        malla.RecalculateTangents();
        GetComponent<MeshFilter>().sharedMesh = malla;
    }
}