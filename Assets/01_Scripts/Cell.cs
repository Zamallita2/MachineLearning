using UnityEngine;

public class Cell : MonoBehaviour
{
    private Color color;
    private float size;
    private bool fueEliminada = false;

    private float tiempoVida = 0f;
    private float tiempoLimite = 10f;

    public void Setup(float timeToDes, Color color, float size)
    {
        this.color = color;
        this.size = size;
        this.tiempoLimite = timeToDes;
        this.tiempoVida = 0f;
        this.fueEliminada = false;
    }

    void Update()
    {
        tiempoVida += Time.deltaTime;

        if (tiempoVida >= tiempoLimite)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        fueEliminada = true;

        if (GameManagerCell.Instance != null)
            GameManagerCell.Instance.CelulaEliminada();

        ReportarResultado();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!fueEliminada && tiempoVida > 0f)
        {
            ReportarResultado();
        }
    }

    void ReportarResultado()
    {
        float ratio = Mathf.Clamp01(tiempoVida / tiempoLimite);

        if (CellSpawner.Instance != null)
            CellSpawner.Instance.RegistrarResultado(color, size, ratio);
    }
}
