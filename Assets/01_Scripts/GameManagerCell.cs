using UnityEngine;
using UnityEngine.UI;

public class GameManagerCell : MonoBehaviour
{
    public static GameManagerCell Instance;

    public Text rondaText;
    public Text puntuacionText;
    public Text sobrevivientesText;

    private int ronda = 0;
    private int puntuacion = 0;
    private int sobrevivientes = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetTotalCelulas(int cantidad)
    {
        sobrevivientes = cantidad;
        UpdateUI();
    }

    public void CelulaEliminada()
    {
        puntuacion += 10;
        sobrevivientes--;
        UpdateUI();
    }

    public void NuevaRonda()
    {
        ronda++;
        UpdateUI();
    }

    void UpdateUI()
    {
        rondaText.text = "Ronda: " + ronda;
        puntuacionText.text = "Puntuación: " + puntuacion;
        sobrevivientesText.text = "Sobrevivientes: " + sobrevivientes;
    }

    public void ReiniciarRondas()
    {
        ronda = 0;
        UpdateUI();
    }

}
