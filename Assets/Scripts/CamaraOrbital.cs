using UnityEngine;

// Camara orbital: gira alrededor de un punto con el mouse y se acerca/aleja con la rueda.
public class CamaraOrbital : MonoBehaviour
{
    public Transform objetivo;               // punto alrededor del cual orbita
    public float distancia = 60f;
    public float distanciaMinima = 8f;
    public float distanciaMaxima = 140f;
    public float sensibilidad = 3f;
    public float velocidadZoom = 100f;

    private float anguloHorizontal = 0f;     // alrededor del eje Y
    private float anguloVertical = 20f;      // elevacion sobre el suelo

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Mantener clic izquierdo o derecho para orbitar
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            anguloHorizontal += Input.GetAxis("Mouse X") * sensibilidad;
            anguloVertical -= Input.GetAxis("Mouse Y") * sensibilidad;
            anguloVertical = Mathf.Clamp(anguloVertical, 5f, 85f);
        }

        // Rueda del mouse: zoom
        distancia -= Input.GetAxis("Mouse ScrollWheel") * velocidadZoom;
        distancia = Mathf.Clamp(distancia, distanciaMinima, distanciaMaxima);

        // La camara se coloca a "distancia" metros del objetivo, segun los dos angulos
        Quaternion rotacion = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
        Vector3 desplazamiento = rotacion * new Vector3(0f, 0f, -distancia);

        transform.position = objetivo.position + desplazamiento;
        transform.LookAt(objetivo.position);
    }
}