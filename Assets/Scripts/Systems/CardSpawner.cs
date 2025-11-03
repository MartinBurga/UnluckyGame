using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour
{
    [Header("Prefabs y Referencias")]
    public GameObject[] cardPrefabs;   // Tus 10 prefabs distintos
    public Transform container;        // Panel donde se colocarán
    public GameManager gameManager;    // Referencia al GameManager

    [Header("Configuración")]
    [Range(1, 10)]
    public int cardsToSpawn = 5;       // Cuántas cartas quieres mostrar (por ahora 5)

    void Start()
    {
        // Esperamos un frame para asegurarnos de que GameManager ya exista
        StartCoroutine(SpawnWhenReady());
    }

    private System.Collections.IEnumerator SpawnWhenReady()
    {
        // Esperar hasta que tenga referencia al GameManager
        while (gameManager == null)
            yield return null;

        SpawnRandomCards();
    }

    private void SpawnRandomCards()
    {
        if (cardPrefabs == null || cardPrefabs.Length == 0 || container == null || gameManager == null)
        {
            Debug.LogError("CardSpawner: faltan referencias en el inspector.");
            return;
        }

        // Limpiar cartas previas (por si acaso)
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Limitar cantidad
        cardsToSpawn = Mathf.Min(cardsToSpawn, cardPrefabs.Length);

        // Elegir índices únicos aleatorios
        List<int> indicesDisponibles = new List<int>();
        for (int i = 0; i < cardPrefabs.Length; i++)
            indicesDisponibles.Add(i);

        List<int> seleccionadas = new List<int>();
        while (seleccionadas.Count < cardsToSpawn && indicesDisponibles.Count > 0)
        {
            int rnd = Random.Range(0, indicesDisponibles.Count);
            seleccionadas.Add(indicesDisponibles[rnd]);
            indicesDisponibles.RemoveAt(rnd);
        }

        // Preparar arreglo de botones en GameManager
        gameManager.cardButtons = new GameObject[seleccionadas.Count];

        for (int i = 0; i < seleccionadas.Count; i++)
        {
            int prefabIndex = seleccionadas[i];
            GameObject prefab = cardPrefabs[prefabIndex];
            if (prefab == null) continue;

            GameObject newCard = Instantiate(prefab, container);
            newCard.name = $"Carta_{prefabIndex + 1}";

            // Conectar botón al GameManager
            Button btn = newCard.GetComponent<Button>();
            if (btn != null)
            {
                int capturedValue = prefabIndex + 1; // valor lógico de la carta (1..10)
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    if (gameManager != null)
                        gameManager.OnPickCardButton(capturedValue);
                });
            }
            else
            {
                Debug.LogWarning($"{newCard.name} no tiene componente Button en el objeto raíz.");
            }

            // Guardar referencia para que GameManager pueda hacer fade/ocultar
            gameManager.cardButtons[i] = newCard;
        }

        Debug.Log($"CardSpawner: se generaron {seleccionadas.Count} cartas aleatorias.");
    }
}
