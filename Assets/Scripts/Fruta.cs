using UnityEngine;

public class Fruta : MonoBehaviour
{
    public int puntos = 10;

    private void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        PlayerController zorro = otroObjeto.GetComponent<PlayerController>();

        if (zorro != null)
        {
            zorro.CambiarPuntos(puntos);
            Destroy(gameObject);
        }
    }
}
