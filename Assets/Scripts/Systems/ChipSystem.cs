using UnityEngine;

public static class ChipSystem
{
    public static void StealChips(PlayerStats winner, PlayerStats loser, ChipType type, int amount)
    {
        int steal = Mathf.Min(amount, loser.GetChips(type));
        loser.AddChips(type, -steal);
        winner.AddChips(type, steal);
    }
}
