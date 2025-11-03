using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats Stats = new PlayerStats();

    [Header("Deck (1..10)")]
    public List<Card> deck = new List<Card>();

    [Header("Runtime")]
    public Card CurrentCard { get; private set; }
    public int TempCardBoost { get; set; }
    public ChipType CurrentBetType { get; set; } = ChipType.Body;

    private CardSystem _cardSystem;
    private AbilitySystem _abilitySystem;

    // Guardamos el mazo original para reinicios
    private List<Card> _originalDeck;

    private void Awake()
    {
        _cardSystem = new CardSystem(deck);
        _abilitySystem = GetComponent<AbilitySystem>();
        _originalDeck = new List<Card>(deck);
    }

    public void SetBetType(int betIndex)
    {
        CurrentBetType = (ChipType)betIndex;
    }

    // Selección de carta, ahora elimina la carta del mazo
    public bool SelectCardByValue(int value)
    {
        var c = _cardSystem.GetByValue(value);
        if (c == null) return false;

        // Si la carta ya fue usada, no dejar seleccionarla
        if (!deck.Contains(c))
        {
            Debug.Log($"Carta {value} ya fue usada.");
            return false;
        }

        CurrentCard = c;
        TempCardBoost = 0;

        // 🔹 Eliminar la carta del mazo para que no se repita
        deck.Remove(c);

        return true;
    }

    public void ClearCard()
    {
        CurrentCard = null;
        TempCardBoost = 0;
    }

    public int GetEffectiveCardValue()
    {
        return (CurrentCard?.value ?? 0) + TempCardBoost;
    }

    public bool TryUseBoostAbility(int amount, int cost, int costTypeIndex)
    {
        if (_abilitySystem == null) return false;
        return _abilitySystem.BoostCard(amount, cost, (ChipType)costTypeIndex);
    }

    public void ApplyPassiveLifesteal(int cuerpoLostByEnemy)
    {
        if (_abilitySystem == null) return;
        _abilitySystem.PassiveLifesteal(cuerpoLostByEnemy);
    }

    // 🔁 Reinicia el mazo original (para reiniciar partida)
    public void ResetDeck()
    {
        deck = new List<Card>(_originalDeck);
    }
}
