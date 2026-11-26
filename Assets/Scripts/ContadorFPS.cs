using UnityEngine;
using TMPro;

// Muestra los FPS y el tiempo por cuadro en pantalla, y el estado de los efectos.
// La tecla V activa o desactiva el VSync (util para medir el rendimiento real).
public class ContadorFPS : MonoBehaviour
{
    public TMP_Text texto;
    public ControlEfectos efectos;      // opcional: muestra el estado de efectos y niebla
    public float intervalo = 0.5f;      // cada cuantos segundos se actualiza el texto

    private float tiempoAcumulado = 0f;
    private int cuadros = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            QualitySettings.vSyncCount = (QualitySettings.vSyncCount == 0) ? 1 : 0;

        // Se promedian los cuadros de medio segundo para que el numero no parpadee
        tiempoAcumulado += Time.unscaledDeltaTime;
        cuadros++;

        if (tiempoAcumulado >= intervalo)
        {
            float fps = cuadros / tiempoAcumulado;
            float milisegundos = 1000f * tiempoAcumulado / cuadros;

            // Verde desde 60 FPS, amarillo desde 30, rojo por debajo
            string color = fps >= 60f ? "#66FF66" : (fps >= 30f ? "#FFD84D" : "#FF6666");

            string estado = "<color=" + color + ">" + fps.ToString("0") + " FPS</color>  ("
                          + milisegundos.ToString("0.0") + " ms)";

            if (efectos != null)
            {
                estado += "\nEfectos: " + (efectos.EfectosActivos ? "ON" : "OFF");
                estado += "   Niebla: " + (efectos.NieblaActiva ? "ON" : "OFF");
            }
            estado += "\nVSync: " + (QualitySettings.vSyncCount > 0 ? "ON" : "OFF");

            texto.text = estado;
            tiempoAcumulado = 0f;
            cuadros = 0;
        }
    }
}