using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 8f;

    [Header("Puntuación de Frutas")]
    public int puntos = 0;
    public int tamanoTexto = 64;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col;

    private float movimientoHorizontal;
    private bool quiereSaltar;

    void Start()
    {
        // Obtenemos los componentes del Zorro
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 1. Leer las teclas de movimiento horizontal (A/D o Flechas)
        movimientoHorizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movimientoHorizontal = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movimientoHorizontal = 1f;
            }

            // Detectar salto con la Barra Espaciadora en Unity 6
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                quiereSaltar = true;
            }
        }
        else
        {
            movimientoHorizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                quiereSaltar = true;
            }
        }

        // 2. Voltear al personaje según la dirección (Flip horizontal)
        if (movimientoHorizontal > 0)
        {
            spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else if (movimientoHorizontal < 0)
        {
            spriteRenderer.flipX = true;  // Mirar a la izquierda
        }

        // 3. Controlar la animación de correr/descansar
        bool seEstaMoviendo = movimientoHorizontal != 0;
        if (animator != null)
        {
            animator.SetBool("isRunning", seEstaMoviendo);
        }
    }

    void FixedUpdate()
    {
        // 4. Mover horizontalmente manteniendo la velocidad vertical
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);

        // 5. Aplicar salto si presionó Espacio y está tocando el suelo
        if (quiereSaltar)
        {
            // Verificamos con un rayo invisible si hay suelo debajo del zorro
            bool estaEnSuelo = Physics2D.Raycast(transform.position, Vector2.down, 0.8f);

            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            }

            quiereSaltar = false; // Reiniciar la orden de salto
        }
    }

    // 6. Función para sumar (dulces) o restar (agrias) puntos
    public void CambiarPuntos(int cantidad)
    {
        puntos += cantidad;
        Debug.Log("Puntos actuales: " + puntos);
    }

    // 7. Dibujar el marcador de puntos en la esquina superior izquierda automáticamente
    void OnGUI()
    {
        GUIStyle estilo = new GUIStyle(GUI.skin.box);
        estilo.fontSize = tamanoTexto;
        estilo.fontStyle = FontStyle.Bold;
        estilo.normal.textColor = Color.yellow;
        estilo.alignment = TextAnchor.MiddleCenter;

        float anchoCaja = tamanoTexto * 7.5f;
        float altoCaja = tamanoTexto * 1.8f;
        GUI.Box(new Rect(30, 30, anchoCaja, altoCaja), "Puntos: " + puntos, estilo);
    }
}
