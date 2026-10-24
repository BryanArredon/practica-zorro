# 📖 Manual Maestro: Desarrollo de Videojuegos 2D en Unity
## Proyecto: "El Zorro y su Mundo" (Sunny Land)

Este manual está diseñado para que cualquier persona, **sin conocimientos previos de programación ni desarrollo de videojuegos**, pueda construir este juego desde cero y aprenda los fundamentos necesarios para realizar futuras prácticas de forma independiente.

---

## 📑 Tabla de Contenidos
1. [Los 4 Pilares de Unity (Conceptos Clave)](#1-los-4-pilares-de-unity)
2. [Fase 1: Preparación del Proyecto y Pixel Art](#2-fase-1-preparación-del-proyecto-y-pixel-art)
3. [Fase 2: Construcción del Mundo con Tilemaps](#3-fase-2-construcción-del-mundo-con-tilemaps)
4. [Fase 3: El Zorro, Físicas y Colisiones](#4-fase-3-el-zorro-físicas-y-colisiones)
5. [Fase 4: Animaciones y Máquina de Estados](#5-fase-4-animaciones-y-máquina-de-estados)
6. [Fase 5: Programación en C# (Movimiento y Salto)](#6-fase-5-programación-en-c-movimiento-y-salto)
7. [Fase 6: Cámara Dinámica que sigue al Jugador](#7-fase-6-cámara-dinámica-que-sigue-al-jugador)
8. [Fase 7: Expansión (Coleccionables, Muerte y UI)](#8-fase-7-expansión-coleccionables-muerte-y-ui)
9. [Diccionario de Errores Comunes y Soluciones](#9-diccionario-de-errores-comunes-y-soluciones)

---

## 1. Los 4 Pilares de Unity

Para entender Unity, piensa en una **obra de teatro**:

```
┌─────────────────────────────────────────────────────────────┐
│ 1. SCENE (Escenario)                                        │
│    El espacio donde colocas las luces, suelos y personajes. │
│                                                             │
│    ┌──────────────────────────────────────────────────┐     │
│    │ 2. GAMEOBJECT (El Actor)                         │     │
│    │    Cualquier elemento: Zorro, Suelo, Cámara.     │     │
│    │                                                  │     │
│    │    ┌────────────────────────────────────────┐    │     │
│    │    │ 3. COMPONENT (El Rol / Habilidad)      │    │     │
│    │    │    - ¿Tiene imagen? -> SpriteRenderer  │    │     │
│    │    │    - ¿Tiene peso?   -> Rigidbody2D     │    │     │
│    │    │    - ¿Choca?        -> Collider2D      │    │     │
│    │    │    - ¿Se mueve?     -> Script C#       │    │     │
│    │    └────────────────────────────────────────┘    │     │
│    └──────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

* **Project (Pestaña inferior):** Tu almacén de archivos (imágenes, sonidos, códigos).
* **Hierarchy (Pestaña izquierda):** La lista de objetos que están actualmente vivos en la escena.
* **Scene / Game (Centro):**
  * *Scene:* Tu mesa de trabajo (aquí arrastras y editas).
  * *Game:* Lo que ve la cámara (se activa al pulsar **Play ▶**).
* **Inspector (Pestaña derecha):** La ficha técnica del objeto que tengas seleccionado.

---

## 2. Fase 1: Preparación del Proyecto y Pixel Art

Las imágenes de juegos retro (Pixel Art) requieren configuración especial para verse nítidas y cortarse correctamente.

### 2.1 Configuración de Nitidez (Anti-borrosidad)
Por defecto, Unity difumina las imágenes. Para que se vean píxel a píxel:
1. Haz clic sobre la imagen en la pestaña **Project**.
2. En el **Inspector**, ajusta:
   * **Texture Type:** `Sprite (2D and UI)`
   * **Sprite Mode:** `Multiple` (si la imagen contiene varios recortes).
   * **Filter Mode:** `Point (no filter)` *(¡Vital para quitar la borrosidad!)*
   * **Compression:** `None`
3. Haz clic en el botón **Apply** abajo a la derecha.

### 2.2 El Corte de Imágenes (Sprite Editor)
> ⚠️ **Regla Crítica del Zorro:** Los frames del zorro miden **33 píxeles de ancho por 32 de alto** (no 32x32). Si cortas a 32, el sprite se desfasará.

1. Abre el **Sprite Editor** desde el Inspector de la imagen.
2. Clic en **Slice** (menú superior izquierdo):
   * **Para el Tileset (`tileset.png`):**
     * Type: `Grid By Cell Size`
     * Pixel Size: `X: 16`, `Y: 16`
   * **Para el Zorro Reposo (`Foxy/idle/spritesheet.png`):**
     * Type: `Grid By Cell Size`
     * Pixel Size: `X: 33`, `Y: 32` (deben salir 4 frames).
   * **Para el Zorro Corriendo (`Foxy/run/spritesheet.png`):**
     * Type: `Grid By Cell Size`
     * Pixel Size: `X: 33`, `Y: 32` (deben salir 6 frames).
3. Haz clic en **Slice** y luego en **Apply** (arriba a la derecha).

---

## 3. Fase 2: Construcción del Mundo con Tilemaps

Un nivel 2D profesional se divide en **dos capas como hojas de acetato transparentes**:

```
Grid (Contenedor general)
├── Suelo         (Tiene Tilemap Collider 2D -> SÓLIDO)
└── Decoracion    (NO tiene Collider -> ATRAVESABLE)
```

### 3.1 Crear las Capas
1. En **Hierarchy**, clic derecho ➔ **2D Object** ➔ **Tilemap** ➔ **Rectangular**.
2. Renombra el Tilemap como **`Suelo`**.
3. En el **Inspector** de `Suelo`, clic en **Add Component** ➔ **Tilemap Collider 2D**.
4. Clic derecho sobre `Grid` ➔ **2D Object** ➔ **Tilemap** ➔ **Rectangular**.
5. Renombra este segundo Tilemap como **`Decoracion`** *(a este NO le agregues Collider)*.

### 3.2 Usar la Tile Palette
1. Ve a **Window** ➔ **2D** ➔ **Tile Palette**.
2. Pulsa **Create New Palette** (Nombre: `MundoZorro`), guárdala en `Assets/Tiles`.
3. Arrastra la imagen `tileset.png` dentro de la ventana de la paleta.
4. **Para pintar:**
   * Cuando pintes plataformas y suelo donde se camina: selecciona **Active Tilemap: Suelo**.
   * Cuando pintes flores, hierba alta, piedras decorativas o troncos: selecciona **Active Tilemap: Decoracion**.

---

## 4. Fase 3: El Zorro, Físicas y Colisiones

### 4.1 Colocar al Zorro
1. Despliega la flecha `>` de `Foxy/idle/spritesheet`.
2. Arrastra el fotograma `0` a la ventana **Scene**.
3. En **Hierarchy**, renómbralo como **`Zorro`**.

### 4.2 Componentes del Zorro
En el **Inspector** del Zorro, añade los siguientes componentes con **Add Component**:

1. **`Rigidbody 2D` (Gravedad y Músculo):**
   * Despliega la sección **Constraints**.
   * Marca: **`Freeze Rotation Z`** *(evita que el zorro ruede y caiga de cabeza)*.
   * Collision Detection: `Continuous` (para colisiones precisas).

2. **`Capsule Collider 2D` (Cuerpo Físico):**
   * Pulsa el botón **Edit Collider**.
   * Ajusta los puntos verdes para que la base del collider coincida **exactamente con la parte inferior de las patas del zorro**. Si el collider sobresale hacia abajo, el zorro parecerá flotar sobre el pasto.

---

## 5. Fase 4: Animaciones y Máquina de Estados

### 5.1 Crear los Clips
1. Selecciona al **Zorro** en Hierarchy.
2. Abre **Window** ➔ **Animation** ➔ **Animation**.
3. Pulsa **Create** y guarda el archivo como `Assets/Animations/Fox_Idle.anim`.
4. Arrastra los **4 frames de reposo** a la línea de tiempo.
5. En el desplegable del nombre del clip, pulsa **Create New Clip...** y guárdalo como `Assets/Animations/Fox_Run.anim`.
6. Arrastra los **6 frames de correr** a la línea de tiempo.

### 5.2 Conectar en el Animator (El Cerebro)
1. Abre **Window** ➔ **Animation** ➔ **Animator**.
2. En la pestaña **Parameters** (izquierda), pulsa **`+`** ➔ **Bool** ➔ Nómbralo **`isRunning`**.
3. Asegúrate de que `Fox_Idle` sea la caja **naranja** (si no lo es, clic derecho sobre él ➔ *Set as Layer Default State*).
4. **Transición 1 (Idle ➔ Run):**
   * Clic derecho en `Fox_Idle` ➔ **Make Transition** ➔ haz clic en `Fox_Run`.
   * Clic en la flecha: En el Inspector, **desmarca** `Has Exit Time`, pon `Transition Duration: 0`, y en Conditions añade: `isRunning` = **`true`**.
5. **Transición 2 (Run ➔ Idle):**
   * Clic derecho en `Fox_Run` ➔ **Make Transition** ➔ haz clic en `Fox_Idle`.
   * Clic en la flecha: En el Inspector, **desmarca** `Has Exit Time`, pon `Transition Duration: 0`, y en Conditions añade: `isRunning` = **`false`**.

---

## 6. Fase 5: Programación en C# (Movimiento y Salto)

Crea un script llamado `PlayerController.cs` dentro de `Assets/Scripts/`:

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // Compatible con Unity 6

public class PlayerController : MonoBehaviour
{
    [Header("Parámetros de Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float movimientoHorizontal;
    private bool quiereSaltar;

    void Start()
    {
        // Se ejecuta una sola vez al nacer el objeto
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Se ejecuta cada fotograma (revisa teclado)
        movimientoHorizontal = 0f;

        // Soporte de Teclado Unity 6 (New Input System)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                movimientoHorizontal = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                movimientoHorizontal = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                quiereSaltar = true;
        }
        else // Respaldo clásico
        {
            movimientoHorizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
                quiereSaltar = true;
        }

        // Volteo horizontal del Sprite (Flip)
        if (movimientoHorizontal > 0)
            spriteRenderer.flipX = false; // Derecha
        else if (movimientoHorizontal < 0)
            spriteRenderer.flipX = true;  // Izquierda

        // Activar o desactivar animación de correr
        bool seEstaMoviendo = movimientoHorizontal != 0;
        if (animator != null)
            animator.SetBool("isRunning", seEstaMoviendo);
    }

    void FixedUpdate()
    {
        // Se ejecuta en ciclos fijos de física
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);

        if (quiereSaltar)
        {
            // Raycast invisible hacia abajo para saber si toca suelo
            bool estaEnSuelo = Physics2D.Raycast(transform.position, Vector2.down, 0.8f);
            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            }
            quiereSaltar = false;
        }
    }
}
```

> **Para asignarlo:** Arrastra el archivo `PlayerController` al objeto `Zorro` en la Hierarchy.

---

## 7. Fase 6: Cámara Dinámica que sigue al Jugador

Crea un script llamado `CameraFollow.cs` en `Assets/Scripts/`:

```csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float suavizado = 5f;
    public Vector3 desplazamiento = new Vector3(0, 1f, -10f);

    void LateUpdate()
    {
        if (objetivo == null) return;
        Vector3 posicionDeseada = objetivo.position + desplazamiento;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
```

1. Selecciona **Main Camera** en la Hierarchy.
2. Añade el script con **Add Component** ➔ **Camera Follow**.
3. Arrastra al objeto **`Zorro`** desde la Hierarchy a la casilla vacía **Objetivo**.

---

## 8. Fase 7: Expansión (Coleccionables, Muerte y UI)

### 8.1 Cerezas Coleccionables
1. Arrastra una imagen de cereza a la escena.
2. En el Inspector, añade **Circle Collider 2D** y marca la casilla **`Is Trigger`** *(para que sea un sensor intangible)*.
3. Crea un script `Cereza.cs`:
```csharp
using UnityEngine;

public class Cereza : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Destruye la cereza al ser tocada por el Zorro
            Destroy(gameObject);
        }
    }
}
```
*(Asegúrate de ponerle la etiqueta **Tag: Player** a tu Zorro en su Inspector).*

### 8.2 Zona de Muerte (Caída al Vacío)
Si el zorro se cae de las plataformas, no debe caer eternamente:
1. Crea un GameObject vacío: Clic derecho en Hierarchy ➔ **Create Empty** (Llámalo `ZonaMuerte`).
2. Añade un **Box Collider 2D**, actívale **`Is Trigger`** y estíralo como una línea larga debajo de todo el nivel.
3. Crea un script `ZonaMuerte.cs`:
```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reinicia el nivel actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
```

---

## 9. Diccionario de Errores Comunes y Soluciones

| Problema | Causa | Solución |
| :--- | :--- | :--- |
| **El Zorro flota sobre el suelo** | El Collider del Zorro sobresale por debajo de sus patas. | Pulsa **Edit Collider** en el Zorro y sube el borde inferior al ras de sus pies. |
| **El Zorro se queda corriendo siempre** | Falta la condición en el Animator o `Has Exit Time` está activo. | Desmarca `Has Exit Time` en ambas flechas y revisa que `isRunning` sea `true` (al ir a correr) y `false` (al volver). |
| **El Zorro se estrella con el pasto alto** | El pasto fue pintado en el Tilemap `Suelo`. | Pinta las hierbas y flores en el Tilemap `Decoracion` (sin collider). |
| **Las teclas no responden en Unity 6** | El proyecto tiene activado solo el nuevo paquete de Input. | En `Edit > Project Settings > Player > Other Settings`, pon **Active Input Handling: Both**. |
| **El Zorro rueda y se cae de cabeza** | La física no tiene bloqueada la rotación en el eje Z. | En `Rigidbody 2D` ➔ Constraints ➔ Marca **`Freeze Rotation Z`**. |
