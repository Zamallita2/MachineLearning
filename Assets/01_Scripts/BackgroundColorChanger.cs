using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    public SpriteRenderer square; 
    public Color[] colores = new Color[] { };

    private int indice = 0;

    public void CambiarColor()
    {
        if (square != null && colores.Length > 0)
        {
            square.color = colores[indice];
            indice = (indice + 1) % colores.Length;
        }

        if (GameManagerCell.Instance != null)
        {
            GameManagerCell.Instance.ReiniciarRondas();
        }

        if (CellSpawner.Instance != null)
        {
            CellSpawner.Instance.ReiniciarAprendizaje();
        }
    }

}
