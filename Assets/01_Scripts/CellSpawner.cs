using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellSpawner : MonoBehaviour
{
    public static CellSpawner Instance;
    [Range(0f, 1f)]
    public float probabilidadExploracion = 0.4f; 


    private List<CellStats> historial = new List<CellStats>();
    private bool primeraRonda = true;

    public Text txtDebugInfo; 

    public Cell cellPrefab;
    public Transform[] firePoints = new Transform[4];
    public int CantidadCelulas = 5;
    public float TimeBtwSpawn = 10;
    private float Timer;

    public Color[] possibleColors = new Color[] { };

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnCell();
    }

    private void Update()
    {
        if (Timer >= TimeBtwSpawn)
        {
            SpawnCell();

            if (GameManagerCell.Instance != null)
                GameManagerCell.Instance.NuevaRonda(); 

            Timer = 0;
        }

        else
        {
            Timer += Time.deltaTime;
        }
    }

    public void SpawnCell()
    {
        if (firePoints.Length < 4)
        {
            Debug.LogWarning("¡Debes asignar 4 firePoints!");
            return;
        }

        float minX = Mathf.Min(firePoints[0].position.x, firePoints[1].position.x, firePoints[2].position.x, firePoints[3].position.x);
        float maxX = Mathf.Max(firePoints[0].position.x, firePoints[1].position.x, firePoints[2].position.x, firePoints[3].position.x);
        float minY = Mathf.Min(firePoints[0].position.y, firePoints[1].position.y, firePoints[2].position.y, firePoints[3].position.y);
        float maxY = Mathf.Max(firePoints[0].position.y, firePoints[1].position.y, firePoints[2].position.y, firePoints[3].position.y);

        
        float sizeMin = 0.07f;
        float sizeMax = 0.12f;

        if (historial.Count > 0)
        {
            var mejor = historial.OrderByDescending(h => h.Score).First();
            float progreso = Mathf.Clamp01(mejor.Score);

            sizeMin = Mathf.Lerp(0.07f, 0.02f, progreso);
            sizeMax = Mathf.Lerp(0.12f, 0.05f, progreso);
            CantidadCelulas = Mathf.RoundToInt(Mathf.Lerp(5f, 10f, progreso));
        }

        for (int i = 0; i < CantidadCelulas; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            (Color colorElegido, float tamanoElegido) =
     primeraRonda || Random.value < probabilidadExploracion
     ? ObtenerColorYTamanoAleatorio()
     : ObtenerColorYTamanoAprendido(sizeMin, sizeMax);


            Cell newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
            newCell.transform.localScale = new Vector3(tamanoElegido, tamanoElegido, 1f);

            SpriteRenderer sr = newCell.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = colorElegido;

            newCell.Setup(TimeBtwSpawn, colorElegido, tamanoElegido);
        }

        primeraRonda = false;
        GameManagerCell.Instance.SetTotalCelulas(CantidadCelulas);
        ActualizarDebugInfo();
    }

    private (Color, float) ObtenerColorYTamanoAleatorio()
    {
        Color color = possibleColors[Random.Range(0, possibleColors.Length)];
        float size = Random.Range(0.07f, 0.12f);
        return (color, size);
    }

    private (Color, float) ObtenerColorYTamanoAprendido(float sizeMin, float sizeMax)
    {
        if (historial.Count == 0 || possibleColors.Length == 0)
        {
            return ObtenerColorYTamanoAleatorio();
        }

        var mejor = historial.OrderByDescending(h => h.Score).First();
        float size = Random.Range(sizeMin, sizeMax);
        return (mejor.color, size);
    }

    public void RegistrarResultado(Color color, float size, float ratioSupervivencia)
    {
        CellStats existente = historial.FirstOrDefault(h =>
            h.color == color && Mathf.Approximately(h.size, size));

        if (existente == null)
        {
            existente = new CellStats
            {
                color = color,
                size = size
            };
            historial.Add(existente);
        }

        existente.totalRatio += ratioSupervivencia;
        existente.observaciones++;
    }

    public void ReiniciarAprendizaje()
    {
        historial.Clear();
        primeraRonda = true;
        Debug.Log(" Aprendizaje reiniciado por cambio de fondo.");
    }

    private void ActualizarDebugInfo()
    {
        if (txtDebugInfo == null)
            return;

        if (historial.Count == 0)
        {
            txtDebugInfo.text = "Aprendizaje: sin datos aún.";
            return;
        }

        var mejor = historial.OrderByDescending(h => h.Score).First();
        txtDebugInfo.text =
            $"Mas exitoso:\nColor: {ColorUtility.ToHtmlStringRGB(mejor.color)}\n" +
            $"Tamaño: {mejor.size:F3}\n" +
            $"Score: {mejor.Score:F2}\n" +
            $"Observaciones: {mejor.observaciones}";
    }
}
