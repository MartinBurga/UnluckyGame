using System.Collections.Generic;
using UnityEngine;

public class CardSystem
{
    private readonly List<Card> _deck;

    public CardSystem(List<Card> deck)
    {
        _deck = deck;
    }

    public Card GetByValue(int v)
    {
        if (_deck == null) return null;
        return _deck.Find(c => c.value == v);
    }

    public Card GetRandom()
    {
        if (_deck == null || _deck.Count == 0) return null;
        return _deck[Random.Range(0, _deck.Count)];
    }
}
