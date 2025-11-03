using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AbilitySystem : MonoBehaviour
{
    private PlayerController _owner;

    private void Awake()
    {
        _owner = GetComponent<PlayerController>();
    }

    public bool BoostCard(int amount, int cost, ChipType costType)
    {
        if (_owner == null || _owner.CurrentCard == null) return false;
        if (!_owner.Stats.SpendChips(costType, cost)) return false;
        _owner.TempCardBoost += amount;
        return true;
    }

    public void PassiveLifesteal(int cuerpoLostByEnemy)
    {
        if (cuerpoLostByEnemy <= 0) return;
        int heal = Mathf.Max(1, cuerpoLostByEnemy / 2);
        _owner.Stats.AddChips(ChipType.Body, heal);
    }
}
