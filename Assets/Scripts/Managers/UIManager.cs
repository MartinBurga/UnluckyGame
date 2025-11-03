using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    public TMP_Text playerBodyText;
    public TMP_Text playerMindText;
    public TMP_Text playerSoulText;

    [Header("Enemy UI")]
    public TMP_Text enemyBodyText;
    public TMP_Text enemyMindText;
    public TMP_Text enemySoulText;

    [Header("Round UI")]
    public TMP_Text roundResultText;
    public TMP_Text playerCardText;
    public TMP_Text enemyCardText;

    [Header("End Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    public void UpdateStats(PlayerStats player, PlayerStats enemy)
    {
        if (playerBodyText != null) playerBodyText.text = $"Cuerpo: {player.cuerpo}";
        if (playerMindText != null) playerMindText.text = $"Mente: {player.mente}";
        if (playerSoulText != null) playerSoulText.text = $"Alma: {player.alma}";

        if (enemyBodyText != null) enemyBodyText.text = $"Cuerpo: {enemy.cuerpo}";
        if (enemyMindText != null) enemyMindText.text = $"Mente: {enemy.mente}";
        if (enemySoulText != null) enemySoulText.text = $"Alma: {enemy.alma}";
    }

    public void ShowCards(int playerValue, int enemyValue)
    {
        if (playerCardText != null) playerCardText.text = $"Tu carta: {playerValue}";
        if (enemyCardText != null) enemyCardText.text = $"Carta enemiga: {enemyValue}";
    }

    public void ShowRoundResult(string txt)
    {
        if (roundResultText != null) roundResultText.text = txt;
    }

    public void ShowEnd(bool playerWon)
    {
        if (victoryPanel != null) victoryPanel.SetActive(playerWon);
        if (defeatPanel != null) defeatPanel.SetActive(!playerWon);
    }
}
