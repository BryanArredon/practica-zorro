using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform objetivo; // Aquí arrastraremos al Zorro

    [Header("Ajustes de Cámara")]
    public float suavizado = 5f; // Qué tan suave sigue la cámara
    public Vector3 desplazamiento = new Vector3(0, 1f, -10f); // Para que la cámara quede un poco arriba y no tape el 2D

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Calculamos la posición donde debería estar la cámara
        Vector3 posicionDeseada = objetivo.position + desplazamiento;

        // Movemos la cámara suavemente hacia esa posición
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
