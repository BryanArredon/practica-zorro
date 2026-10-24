using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombre de la Escena del Juego")]
    public string escenaJuego = "SampleScene";

    [Header("Textos de la Portada")]
    public string tituloPrincipal = "EL ZORRO Y SU MUNDO";
    public string subtitulo = "AVENTURA EN SUNNY LAND";

    [Header("Imagen de Portada Personalizada (Opcional)")]
    public Sprite imagenFondoPortada;

    private GameObject panelMenuPrincipal;
    private GameObject panelComoJugar;
    private GameObject panelDificultad;
    private RectTransform rectTitulo;
    private Image fondoVelo;
    private Text textoDificultadActual;

    void Start()
    {
        // Asegurarnos de que exista un EventSystem para que los botones respondan al ratón
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        ConstruirCanvasProfesional();
    }

    void Update()
    {
        // Animación suave de respiración/flotación para el cartel del título
        if (rectTitulo != null)
        {
            float escala = 1f + Mathf.Sin(Time.time * 2.5f) * 0.03f;
            rectTitulo.localScale = new Vector3(escala, escala, 1f);
        }

        // Mover suavemente la cámara en la portada para dar efecto cinemático sobre la escena
        if (Camera.main != null)
        {
            float desplazamientoX = Mathf.Sin(Time.time * 0.4f) * 1.2f;
            Camera.main.transform.position = new Vector3(desplazamientoX, Camera.main.transform.position.y, -10f);
        }
    }

    // Métodos públicos por si también quieres asignarlos manualmente desde un botón de Unity UI
    public void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirComoJugar()
    {
        panelMenuPrincipal.SetActive(false);
        panelComoJugar.SetActive(true);
        panelDificultad.SetActive(false);
    }

    public void AbrirDificultad()
    {
        panelMenuPrincipal.SetActive(false);
        panelComoJugar.SetActive(false);
        panelDificultad.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelMenuPrincipal.SetActive(true);
        panelComoJugar.SetActive(false);
        panelDificultad.SetActive(false);
    }

    public void SeleccionarDificultad(int nivel)
    {
        if (nivel == 0)
        {
            PlayerPrefs.SetFloat("VelocidadZorro", 4f);
            PlayerPrefs.SetFloat("SaltoZorro", 9.5f);
            if (textoDificultadActual != null) textoDificultadActual.text = "MODO SELECCIONADO: EXPLORADOR (FÁCIL)";
        }
        else if (nivel == 1)
        {
            PlayerPrefs.SetFloat("VelocidadZorro", 5.5f);
            PlayerPrefs.SetFloat("SaltoZorro", 8f);
            if (textoDificultadActual != null) textoDificultadActual.text = "MODO SELECCIONADO: CLÁSICO (NORMAL)";
        }
        else if (nivel == 2)
        {
            PlayerPrefs.SetFloat("VelocidadZorro", 8.5f);
            PlayerPrefs.SetFloat("SaltoZorro", 10f);
            if (textoDificultadActual != null) textoDificultadActual.text = "MODO SELECCIONADO: ZORRO TURBO (DIFÍCIL)";
        }
        PlayerPrefs.Save();
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // =========================================================================
    // CONSTRUCTOR AUTOMÁTICO DE UI EN CANVAS (Con sombras, bordes y animaciones)
    // =========================================================================
    void ConstruirCanvasProfesional()
    {
        // 1. Crear el Canvas escalable a 1920x1080
        GameObject canvasObj = new GameObject("CanvasPortada");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();

        // 2. Si el usuario asignó una imagen de portada (o la guardó en Assets/environment/portada.png), mostrarla a pantalla completa
        if (imagenFondoPortada != null)
        {
            GameObject imgPortadaObj = CrearElementoUI("ImagenPortadaIA", canvasObj.transform, Vector2.zero, new Vector2(1920, 1080));
            Image imgPortada = imgPortadaObj.AddComponent<Image>();
            imgPortada.sprite = imagenFondoPortada;
            imgPortada.preserveAspect = false;
        }

        // Velo oscuro degradado suave para que la escena o imagen de fondo luzca pero el menú resalte
        GameObject veloObj = CrearElementoUI("VeloOscuro", canvasObj.transform, Vector2.zero, new Vector2(1920, 1080));
        fondoVelo = veloObj.AddComponent<Image>();
        fondoVelo.color = (imagenFondoPortada != null)
            ? new Color(0.03f, 0.04f, 0.10f, 0.30f)
            : new Color(0.05f, 0.07f, 0.14f, 0.52f);

        // 3. Banner del Título Superior estilo Madera / Dorado
        GameObject contenedorTitulo = CrearElementoUI("BannerTitulo", canvasObj.transform, new Vector2(0, 310), new Vector2(1150, 210));
        rectTitulo = contenedorTitulo.GetComponent<RectTransform>();

        CrearPanelConBorde(contenedorTitulo.transform, Vector2.zero, new Vector2(1150, 200),
            new Color(0.98f, 0.72f, 0.15f, 1f), // Borde dorado brillante
            new Color(0.22f, 0.12f, 0.06f, 0.96f)); // Fondo madera oscura

        CrearTextoUI("TextoTitulo", contenedorTitulo.transform, tituloPrincipal, 74, new Vector2(0, 22), new Vector2(1100, 110), new Color(1f, 0.88f, 0.25f));
        CrearTextoUI("TextoSubtitulo", contenedorTitulo.transform, subtitulo, 32, new Vector2(0, -52), new Vector2(1000, 60), new Color(0.95f, 0.95f, 0.95f));

        // 4. PANEL 1: MENÚ PRINCIPAL
        panelMenuPrincipal = CrearElementoUI("PanelMenuPrincipal", canvasObj.transform, new Vector2(0, -100), new Vector2(700, 560));
        CrearPanelConBorde(panelMenuPrincipal.transform, Vector2.zero, new Vector2(680, 540),
            new Color(1f, 0.78f, 0.2f, 0.9f),
            new Color(0.10f, 0.14f, 0.22f, 0.92f));

        CrearBotonPro(panelMenuPrincipal.transform, "▶   JUGAR AHORA", new Vector2(0, 155), new Vector2(540, 95),
            new Color(0.18f, 0.75f, 0.32f), new Color(0.25f, 0.88f, 0.40f), Jugar);

        CrearBotonPro(panelMenuPrincipal.transform, "🍎   GUÍA DE FRUTAS Y CONTROLES", new Vector2(0, 35), new Vector2(540, 90),
            new Color(0.18f, 0.52f, 0.88f), new Color(0.28f, 0.65f, 0.98f), AbrirComoJugar);

        CrearBotonPro(panelMenuPrincipal.transform, "⚡   DIFICULTAD / VELOCIDAD", new Vector2(0, -80), new Vector2(540, 90),
            new Color(0.92f, 0.48f, 0.12f), new Color(1f, 0.60f, 0.22f), AbrirDificultad);

        CrearBotonPro(panelMenuPrincipal.transform, "✖   SALIR DEL JUEGO", new Vector2(0, -195), new Vector2(540, 80),
            new Color(0.82f, 0.22f, 0.24f), new Color(0.95f, 0.32f, 0.35f), Salir);

        // 5. PANEL 2: CÓMO JUGAR Y REGLAS DE FRUTAS
        panelComoJugar = CrearElementoUI("PanelComoJugar", canvasObj.transform, new Vector2(0, -100), new Vector2(1050, 560));
        CrearPanelConBorde(panelComoJugar.transform, Vector2.zero, new Vector2(1050, 560),
            new Color(0.3f, 0.75f, 1f, 1f),
            new Color(0.08f, 0.12f, 0.20f, 0.95f));

        string explicacion =
            "CONTROLES DEL PERSONAJE:\n" +
            "• Caminar / Correr:  Teclas [ A ] y [ D ]  o  [ Flechas Izquierda / Derecha ]\n" +
            "• Saltar Plataformas:  [ Barra Espaciadora ]     • Volver al Menú:  [ ESC ]\n\n" +
            "SISTEMA DE FRUTAS:\n" +
            "• FRUTAS DULCES (+10 Puntos): Manzanas, fresas, cerezas y plátanos.\n" +
            "• FRUTAS AGRIAS (-5 Puntos): Limones y limas ácidas (¡Evítalas!).";

        CrearTextoUI("TextoInstrucciones", panelComoJugar.transform, explicacion, 34, new Vector2(0, 55), new Vector2(940, 360), Color.white);

        CrearBotonPro(panelComoJugar.transform, "⬅   VOLVER AL MENÚ PRINCIPAL", new Vector2(0, -195), new Vector2(520, 85),
            new Color(0.18f, 0.75f, 0.32f), new Color(0.25f, 0.88f, 0.40f), VolverAlMenu);
        panelComoJugar.SetActive(false);

        // 6. PANEL 3: SELECTOR DE DIFICULTAD
        panelDificultad = CrearElementoUI("PanelDificultad", canvasObj.transform, new Vector2(0, -100), new Vector2(960, 560));
        CrearPanelConBorde(panelDificultad.transform, Vector2.zero, new Vector2(960, 560),
            new Color(1f, 0.65f, 0.15f, 1f),
            new Color(0.12f, 0.10f, 0.18f, 0.95f));

        textoDificultadActual = CrearTextoUI("TextoDificultad", panelDificultad.transform,
            "MODO SELECCIONADO: CLÁSICO (NORMAL)", 36, new Vector2(0, 195), new Vector2(880, 70), new Color(1f, 0.85f, 0.25f));

        CrearBotonPro(panelDificultad.transform, "🐢  MODO EXPLORADOR (Salto Alto / Suave)", new Vector2(0, 90), new Vector2(720, 82),
            new Color(0.20f, 0.62f, 0.75f), new Color(0.30f, 0.75f, 0.88f), () => SeleccionarDificultad(0));

        CrearBotonPro(panelDificultad.transform, "🦊  MODO CLÁSICO (Velocidad Estándar)", new Vector2(0, -10), new Vector2(720, 82),
            new Color(0.18f, 0.52f, 0.88f), new Color(0.28f, 0.65f, 0.98f), () => SeleccionarDificultad(1));

        CrearBotonPro(panelDificultad.transform, "⚡  MODO ZORRO TURBO (Alta Velocidad)", new Vector2(0, -110), new Vector2(720, 82),
            new Color(0.88f, 0.38f, 0.12f), new Color(0.98f, 0.50f, 0.20f), () => SeleccionarDificultad(2));

        CrearBotonPro(panelDificultad.transform, "⬅   GUARDAR Y VOLVER", new Vector2(0, -210), new Vector2(480, 78),
            new Color(0.18f, 0.75f, 0.32f), new Color(0.25f, 0.88f, 0.40f), VolverAlMenu);
        panelDificultad.SetActive(false);
    }

    GameObject CrearElementoUI(string nombre, Transform padre, Vector2 pos, Vector2 tamano)
    {
        GameObject obj = new GameObject(nombre);
        obj.transform.SetParent(padre, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = tamano;
        return obj;
    }

    void CrearPanelConBorde(Transform padre, Vector2 pos, Vector2 tamano, Color colorBorde, Color colorFondo)
    {
        GameObject sombra = CrearElementoUI("Sombra", padre, pos + new Vector2(8, -10), tamano);
        Image imgSombra = sombra.AddComponent<Image>();
        imgSombra.color = new Color(0f, 0f, 0f, 0.55f);

        GameObject borde = CrearElementoUI("Borde", padre, pos, tamano);
        Image imgBorde = borde.AddComponent<Image>();
        imgBorde.color = colorBorde;

        GameObject interior = CrearElementoUI("FondoInterior", padre, pos, tamano - new Vector2(14, 14));
        Image imgInterior = interior.AddComponent<Image>();
        imgInterior.color = colorFondo;
    }

    Text CrearTextoUI(string nombre, Transform padre, string contenido, int tamanoFuente, Vector2 pos, Vector2 tamano, Color colorTexto)
    {
        GameObject obj = CrearElementoUI(nombre, padre, pos, tamano);
        Text txt = obj.AddComponent<Text>();
        Font fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (fuente == null) fuente = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.font = fuente;
        txt.text = contenido;
        txt.fontSize = tamanoFuente;
        txt.fontStyle = FontStyle.Bold;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = colorTexto;

        Shadow sombra = obj.AddComponent<Shadow>();
        sombra.effectColor = new Color(0f, 0f, 0f, 0.85f);
        sombra.effectDistance = new Vector2(3, -3);

        Outline contorno = obj.AddComponent<Outline>();
        contorno.effectColor = new Color(0f, 0f, 0f, 0.75f);
        contorno.effectDistance = new Vector2(2, -2);

        return txt;
    }

    void CrearBotonPro(Transform padre, string textoBoton, Vector2 pos, Vector2 tamano, Color colorNormal, Color colorHover, UnityEngine.Events.UnityAction accion)
    {
        GameObject contenedor = CrearElementoUI("Boton_" + textoBoton, padre, pos, tamano);

        // Sombra 3D debajo del botón
        GameObject sombraBtn = CrearElementoUI("BordeInferior3D", contenedor.transform, new Vector2(0, -6), tamano);
        Image imgSombra = sombraBtn.AddComponent<Image>();
        imgSombra.color = new Color(colorNormal.r * 0.45f, colorNormal.g * 0.45f, colorNormal.b * 0.45f, 1f);

        // Cuerpo del botón
        GameObject cuerpoBtn = CrearElementoUI("CuerpoBoton", contenedor.transform, Vector2.zero, tamano);
        Image imgBtn = cuerpoBtn.AddComponent<Image>();
        imgBtn.color = Color.white;

        Button btn = cuerpoBtn.AddComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = colorNormal;
        cb.highlightedColor = colorHover;
        cb.pressedColor = new Color(1f, 0.85f, 0.2f);
        cb.selectedColor = colorHover;
        cb.colorMultiplier = 1f;
        btn.colors = cb;

        btn.onClick.AddListener(accion);

        CrearTextoUI("Texto", cuerpoBtn.transform, textoBoton, 32, Vector2.zero, tamano, Color.white);
    }
}
