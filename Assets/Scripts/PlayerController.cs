using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5.5f;

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
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Cargar velocidad/salto si fue elegido en la escena del Menú Principal
        if (PlayerPrefs.HasKey("VelocidadZorro"))
        {
            velocidad = PlayerPrefs.GetFloat("VelocidadZorro");
            fuerzaSalto = PlayerPrefs.GetFloat("SaltoZorro");
        }
    }

    void Update()
    {
        // Presionar ESC para volver a la Escena del Menú Principal
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MenuPrincipal");
            return;
        }

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

        if (movimientoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimientoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }

        bool seEstaMoviendo = movimientoHorizontal != 0;
        if (animator != null)
        {
            animator.SetBool("isRunning", seEstaMoviendo);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);

        if (quiereSaltar)
        {
            bool estaEnSuelo = Physics2D.Raycast(transform.position, Vector2.down, 0.8f);

            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            }

            quiereSaltar = false;
        }
    }

    public void CambiarPuntos(int cantidad)
    {
        puntos += cantidad;
        Debug.Log("Puntos actuales: " + puntos);
    }

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
