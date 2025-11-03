using UnityEngine;

[CreateAssetMenu(menuName = "Unlucky/Card")]
public class Card : ScriptableObject
{
    [Range(1, 10)] public int value = 1;
    public Sprite sprite;
}
