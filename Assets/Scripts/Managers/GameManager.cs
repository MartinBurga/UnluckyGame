using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum TurnState { Selecting, Revealing, Resolving, End }

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerController player;
    public PlayerController enemy;
    public AIController enemyAI;
    public UIManager ui;

    [Header("Ronda")]
    public int betAmount = 2;

    [Header("Botones de Cartas (llenado automático por CardSpawner)")]
    public GameObject[] cardButtons;

    public TurnState state;

    void Start()
    {
        state = TurnState.Selecting;

        if (ui != null)
        {
            ui.UpdateStats(player.Stats, enemy.Stats);
            ui.ShowRoundResult("Elige una carta para comenzar.");
            ui.ShowCards(0, 0);
        }
    }

    public void OnPickCardButton(int value)
    {
        if (state != TurnState.Selecting) return;

        // El jugador intenta seleccionar esa carta
        if (!player.SelectCardByValue(value)) return;

        // 🔹 Desaparecer el botón con fade
        int index = value - 1;
        if (cardButtons != null && index >= 0 && index < cardButtons.Length && cardButtons[index] != null)
        {
            FadeOutButton(cardButtons[index], 0.25f);
        }

        // IA elige carta
        enemyAI.ChooseCard();

        state = TurnState.Revealing;
        RevealCards();
    }

    public void OnUseBoost()
    {
        if (player.TryUseBoostAbility(amount: 2, cost: 1, costTypeIndex: (int)ChipType.Mind))
        {
            ui.ShowRoundResult("Usaste Impulso Mental (+2).");
            ui.UpdateStats(player.Stats, enemy.Stats);
        }
        else
        {
            ui.ShowRoundResult("No tienes fichas suficientes para impulsar.");
        }
    }

    private void RevealCards()
    {
        int pVal = player.GetEffectiveCardValue();
        int eVal = enemy.GetEffectiveCardValue();

        ui.ShowCards(pVal, eVal);
        state = TurnState.Resolving;
        ResolveRound(pVal, eVal);
    }

    private void ResolveRound(int pVal, int eVal)
{
    string msg;

    // Determinar resultado
    if (pVal > eVal)
    {
        // 🟢 Gana jugador
        ChipSystem.StealChips(player.Stats, enemy.Stats, player.CurrentBetType, betAmount);
        int lostEnemy = (player.CurrentBetType == ChipType.Body) ? betAmount : 0;
        player.ApplyPassiveLifesteal(lostEnemy);
        msg = $"¡Ganaste la ronda! Robas {betAmount} de {player.CurrentBetType}.";
    }
    else if (eVal > pVal)
    {
        // 🔴 Gana IA
        ChipSystem.StealChips(enemy.Stats, player.Stats, enemy.CurrentBetType, betAmount);
        msg = $"Perdiste la ronda. El enemigo roba {betAmount} de {enemy.CurrentBetType}.";
    }
    else
    {
        // ⚪ Empate
        msg = "Empate. No se roban fichas.";
    }

    // Actualizar interfaz
    ui.UpdateStats(player.Stats, enemy.Stats);
    ui.ShowRoundResult(msg);

    // 🔹 Si la partida termina, mostrar fin
    if (player.Stats.IsDefeated || enemy.Stats.IsDefeated)
    {
        state = TurnState.End;
        ui.ShowEnd(playerWon: !player.Stats.IsDefeated);
        return;
    }

    // 🔹 Siempre limpiar las cartas usadas
    player.ClearCard();
    enemy.ClearCard();

    // 🔹 Asegurar que la carta del jugador se oculte si sigue visible
    HideUsedCards();

    // 🔹 Regresar al estado de selección
    state = TurnState.Selecting;
}


    public void RestartGame()
    {
        // No necesitamos tocar CardSpawner aquí, recargar la escena reinicia todo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ---------- EFECTO VISUAL: FADE DE BOTONES ----------

    private void FadeOutButton(GameObject buttonObj, float duration = 0.25f)
    {
        if (buttonObj == null) return;

        CanvasGroup cg = buttonObj.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = buttonObj.AddComponent<CanvasGroup>();

        StartCoroutine(FadeOutRoutine(cg, buttonObj, duration));
    }

    private IEnumerator FadeOutRoutine(CanvasGroup cg, GameObject obj, float duration)
    {
        float t = 0f;
        float startAlpha = cg.alpha;
        Vector3 startScale = obj.transform.localScale;
        Vector3 endScale = startScale * 0.8f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            yield return null;
        }

        cg.alpha = 0f;
        obj.SetActive(false);
    }

    private void HideUsedCards()
{
    // Evitar errores si no hay botones configurados
    if (cardButtons == null || cardButtons.Length == 0) return;

    for (int i = 0; i < cardButtons.Length; i++)
    {
        GameObject btn = cardButtons[i];
        if (btn == null) continue;

        // Si el botón ya está desactivado, omitir
        if (!btn.activeSelf) continue;

        // Verificar si la carta de este índice ya no existe en el mazo del jugador
        if (!player.deck.Exists(c => c.value == i + 1))
        {
            FadeOutButton(btn, 0.15f);
        }
    }
}

}
