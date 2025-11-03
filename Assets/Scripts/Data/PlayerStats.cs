using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [Header("Chips (Body/Mind/Soul)")]
    public int cuerpo = 10;
    public int mente = 10;
    public int alma = 10;

    public bool IsDefeated => cuerpo <= 0 || mente <= 0 || alma <= 0;

    public int GetChips(ChipType type)
    {
        switch (type)
        {
            case ChipType.Body: return cuerpo;
            case ChipType.Mind: return mente;
            case ChipType.Soul: return alma;
            default: return 0;
        }
    }

    public void AddChips(ChipType type, int amount)
    {
        switch (type)
        {
            case ChipType.Body: cuerpo += amount; break;
            case ChipType.Mind: mente += amount; break;
            case ChipType.Soul: alma += amount; break;
        }
    }

    public bool SpendChips(ChipType type, int amount)
    {
        if (GetChips(type) < amount) return false;
        AddChips(type, -amount);
        return true;
    }
}

public enum ChipType { Body, Mind, Soul }
