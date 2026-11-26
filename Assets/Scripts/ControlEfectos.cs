using UnityEngine;
using UnityEngine.Rendering;

// Permite encender y apagar el postprocesado y la niebla,
// para comparar capturas y medir cuanto cuestan en rendimiento.
public class ControlEfectos : MonoBehaviour
{
    public Volume volumen;                          // el Global Volume con Bloom, color, etc.

    public bool EfectosActivos { get; private set; } = true;
    public bool NieblaActiva { get; private set; }

    void Start()
    {
        NieblaActiva = RenderSettings.fog;
        if (volumen != null) volumen.enabled = EfectosActivos;
    }

    void Update()
    {
        // P: postprocesado ON/OFF
        if (Input.GetKeyDown(KeyCode.P))
        {
            EfectosActivos = !EfectosActivos;
            if (volumen != null) volumen.enabled = EfectosActivos;
        }

        // N: niebla ON/OFF
        if (Input.GetKeyDown(KeyCode.N))
        {
            NieblaActiva = !NieblaActiva;
            RenderSettings.fog = NieblaActiva;
        }
    }
}