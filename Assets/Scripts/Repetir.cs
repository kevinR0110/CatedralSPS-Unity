using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Crea varias copias de un prefab. Cada copia se desplaza y/o gira un paso fijo
// respecto a la anterior. Se usa desde el menu del componente: Generar / Limpiar.
public class Repetir : MonoBehaviour
{
    public GameObject prefab;                                  // prefab a repetir (asset de Assets/Prefabs)
    public int cantidad = 7;                                   // cuantas copias
    public Vector3 separacion = new Vector3(0f, 0f, 3.6f);     // desplazamiento entre copias
    public Vector3 rotacionPaso = Vector3.zero;                // giro en grados entre copias

    [ContextMenu("Generar")]
    public void Generar()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Repetir: falta asignar el prefab.");
            return;
        }

        Limpiar();

        for (int i = 0; i < cantidad; i++)
        {
            GameObject copia;
#if UNITY_EDITOR
            copia = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
#else
            copia = Instantiate(prefab, transform);
#endif
            copia.transform.localPosition = prefab.transform.localPosition + separacion * i;
            copia.transform.localRotation = Quaternion.Euler(rotacionPaso * i) * prefab.transform.localRotation;
        }
    }

    [ContextMenu("Limpiar")]
    public void Limpiar()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}