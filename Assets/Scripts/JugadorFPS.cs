using UnityEngine;

// Camara en primera persona: caminar con WASD, mirar con el mouse, correr con Shift.
[RequireComponent(typeof(CharacterController))]
public class JugadorFPS : MonoBehaviour
{
    public Transform camara;                 // la camara hija del jugador
    public float velocidadCaminar = 3.5f;    // metros por segundo
    public float velocidadCorrer = 7f;
    public float sensibilidadMouse = 2f;
    public float gravedad = -20f;

    private CharacterController controlador;
    private float rotacionVertical = 0f;     // angulo de arriba/abajo de la camara
    private float velocidadVertical = 0f;    // para la caida por gravedad

    void Start()
    {
        controlador = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Solo mira alrededor cuando el cursor esta bloqueado
        if (Cursor.lockState == CursorLockMode.Locked) Mirar();
        Mover();
    }

    void Mirar()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Giro izquierda/derecha: gira todo el cuerpo
        transform.Rotate(Vector3.up * mouseX);

        // Giro arriba/abajo: solo la camara, limitado para no dar la vuelta
        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);
        camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");   // A y D
        float vertical = Input.GetAxis("Vertical");       // W y S

        float velocidad = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidadCaminar;

        // Direccion relativa a hacia donde mira el jugador
        Vector3 direccion = transform.right * horizontal + transform.forward * vertical;
        if (direccion.magnitude > 1f) direccion.Normalize();

        // Gravedad
        if (controlador.isGrounded && velocidadVertical < 0f) velocidadVertical = -2f;
        velocidadVertical += gravedad * Time.deltaTime;

        Vector3 movimiento = direccion * velocidad + Vector3.up * velocidadVertical;
        controlador.Move(movimiento * Time.deltaTime);
    }
}