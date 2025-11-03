using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerController self;   // El PlayerController del enemigo
    public PlayerController player; // El PlayerController del jugador

    [Header("Comportamiento")]
    [Range(0f, 1f)] public float activeAbilityChance = 0.2f; // 20% probabilidad de usar habilidad
    public int lowChipThreshold = 5; // Si tiene menos de 5 fichas de algún tipo, usa cartas altas

    public void ChooseCard()
    {
        if (self == null || self.deck.Count == 0) return;

        var stats = self.Stats;
        bool lowOnAny = stats.cuerpo < lowChipThreshold ||
                        stats.mente < lowChipThreshold ||
                        stats.alma < lowChipThreshold;

        int chosenValue;

        // 🔹 Si está bajo en fichas, intenta usar cartas altas
        if (lowOnAny)
        {
            int[] highCards = { 8, 9, 10 };
            chosenValue = highCards[Random.Range(0, highCards.Length)];
        }
        else
        {
            chosenValue = Random.Range(1, 11);
        }

        // 🔹 Asegurarse de no elegir una carta ya usada (no existente en el mazo)
        int attempts = 0;
        while (!_SelfHasCard(chosenValue) && attempts < 10)
        {
            chosenValue = Random.Range(1, 11);
            attempts++;
        }

        // 🔹 Seleccionar la carta válida (si no hay, ignora el turno)
        bool selected = self.SelectCardByValue(chosenValue);
        if (!selected)
        {
            Debug.Log($"IA no pudo seleccionar carta {chosenValue}");
            return;
        }

        // 🔹 Probabilidad de usar una habilidad activa
        if (Random.value < activeAbilityChance)
        {
            self.TryUseBoostAbility(amount: 2, cost: 1, costTypeIndex: (int)ChipType.Mind);
        }

        // 🔹 Decide qué tipo de ficha apostar
        if (stats.cuerpo < stats.mente && stats.cuerpo < stats.alma)
            self.CurrentBetType = ChipType.Body;
        else if (stats.mente < stats.cuerpo && stats.mente < stats.alma)
            self.CurrentBetType = ChipType.Mind;
        else
            self.CurrentBetType = ChipType.Soul;
    }

    // 🔹 Comprueba si la carta aún existe en el mazo de la IA
    private bool _SelfHasCard(int value)
    {
        return self.deck.Exists(c => c.value == value);
    }
}
