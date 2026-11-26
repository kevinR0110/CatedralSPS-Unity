using UnityEngine;

// Alterna entre la camara en primera persona y la orbital con la tecla C.
// Esc libera el cursor y la tecla H muestra u oculta el panel de ayuda.
public class ControlCamaras : MonoBehaviour
{
    public GameObject jugador;               // objeto Jugador (contiene la camara FPS)
    public Camera camaraOrbital;
    public GameObject panelAyuda;            // panel de controles

    private bool primeraPersona = true;

    void Start()
    {
        AplicarModo();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            primeraPersona = !primeraPersona;
            AplicarModo();
        }

        if (Input.GetKeyDown(KeyCode.H) && panelAyuda != null)
            panelAyuda.SetActive(!panelAyuda.activeSelf);

        // Esc libera el cursor; un clic lo vuelve a bloquear (solo en primera persona)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (primeraPersona && Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void AplicarModo()
    {
        // Solo una camara activa a la vez
        jugador.SetActive(primeraPersona);
        camaraOrbital.gameObject.SetActive(!primeraPersona);

        // Evita el aviso de dos AudioListener
        AudioListener oyente = camaraOrbital.GetComponent<AudioListener>();
        if (oyente != null) oyente.enabled = !primeraPersona;

        Cursor.lockState = primeraPersona ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !primeraPersona;
    }
}