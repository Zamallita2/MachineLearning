using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public Cell cellPrefab; // El prefab de la célula 
    public Transform[] firePoints = new Transform[4]; // Los 4 puntos del rectángulo
    public int CantidadCelulas = 5;
    public float TimeBtwSpawn = 10;
    private float Timer;

    public Color[] possibleColors = new Color[] {
    };

    private void Start()
    {
        SpawnCell();
    }

    private void Update()
    {
        if (Timer >= TimeBtwSpawn)
        {
            SpawnCell();
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

        for (int i = 0; i < CantidadCelulas; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Cell newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);

            float randomScale = Random.Range(0.01f, 0.1f);
            newCell.transform.localScale = new Vector3(randomScale, randomScale, 1f); // 2D así que Z=1

            SpriteRenderer sr = newCell.GetComponent<SpriteRenderer>();
            if (sr != null && possibleColors.Length > 0)
            {
                sr.color = possibleColors[Random.Range(0, possibleColors.Length)];
            }

            newCell.Setup(TimeBtwSpawn);
        }

    }
}
