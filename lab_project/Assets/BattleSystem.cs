using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSystem : MonoBehaviour
{
    public Fighter player;

    public Text battleLog;
    public Text playerHPText;
    public Text enemyHPText;
    public Button attackButton;
    public Button specialButton;
    public Button defendButton;

    private Fighter currentEnemy;
    private int enemyIndex = 0;
    private bool playerTurn = false;

    void Start()
    {
        attackButton.onClick.AddListener(() => PlayerAction("attack"));
        specialButton.onClick.AddListener(() => PlayerAction("special"));
        defendButton.onClick.AddListener(() => PlayerAction("defend"));

        player.currentHP = player.maxHP;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        Fighter[] enemies = BattleManager.Instance.GetEnemies();
        currentEnemy = enemies[enemyIndex];
        currentEnemy.currentHP = currentEnemy.maxHP;

        Log($"{currentEnemy.fighterName} appears!");
        UpdateHPText();
        SetButtons(false);
        yield return new WaitForSeconds(1.5f);

        if (player.speed >= currentEnemy.speed)
        {
            Log("You go first!");
            yield return new WaitForSeconds(1f);
            BeginPlayerTurn();
        }
        else
        {
            Log($"{currentEnemy.fighterName} is faster!");
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyTurn());
        }
    }

    void BeginPlayerTurn()
    {
        playerTurn = true;
        SetButtons(true);
        Log("Choose your action.");
    }

    void PlayerAction(string action)
    {
        if (!playerTurn) return;
        playerTurn = false;
        SetButtons(false);
        StartCoroutine(DoPlayerAction(action));
    }

    IEnumerator DoPlayerAction(string action)
    {
        if (action == "attack")
        {
            float rng = Random.Range(0.85f, 1.15f);
            bool crit = Random.value < 0.1f;
            int dmg = Mathf.Max(1, Mathf.RoundToInt((player.strength - currentEnemy.defense / 2) * rng));
            if (crit) dmg *= 2;
            currentEnemy.currentHP = Mathf.Max(0, currentEnemy.currentHP - dmg);
            Log($"You attack for {dmg} damage!{(crit ? " CRITICAL!" : "")}");
        }
        else if (action == "special")
        {
            float rng = Random.Range(0.80f, 1.20f);
            bool crit = Random.value < 0.15f;
            int dmg = Mathf.Max(1, Mathf.RoundToInt((player.intelligence - currentEnemy.defense / 4) * rng));
            if (crit) dmg *= 2;
            currentEnemy.currentHP = Mathf.Max(0, currentEnemy.currentHP - dmg);
            Log($"You use special attack for {dmg} damage!{(crit ? " CRITICAL!" : "")}");
        }
        else if (action == "defend")
        {
            int heal = Mathf.RoundToInt(player.maxHP * 0.15f);
            player.currentHP = Mathf.Min(player.maxHP, player.currentHP + heal);
            Log($"You defend and recover {heal} HP!");
        }

        UpdateHPText();
        yield return new WaitForSeconds(1.2f);

        if (!currentEnemy.isAlive)
        {
            yield return StartCoroutine(EnemyDefeated());
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        float hpPercent = (float)currentEnemy.currentHP / currentEnemy.maxHP;

        if (hpPercent < 0.3f && Random.value < 0.3f)
        {
            int heal = Mathf.RoundToInt(currentEnemy.maxHP * 0.15f);
            currentEnemy.currentHP = Mathf.Min(currentEnemy.maxHP, currentEnemy.currentHP + heal);
            Log($"{currentEnemy.fighterName} defends and recovers {heal} HP!");
        }
        else if (currentEnemy.intelligence > currentEnemy.strength)
        {
            float rng = Random.Range(0.80f, 1.20f);
            int dmg = Mathf.Max(1, Mathf.RoundToInt((currentEnemy.intelligence - player.defense / 4) * rng));
            player.currentHP = Mathf.Max(0, player.currentHP - dmg);
            Log($"{currentEnemy.fighterName} uses special attack for {dmg}!");
        }
        else
        {
            float rng = Random.Range(0.85f, 1.15f);
            int dmg = Mathf.Max(1, Mathf.RoundToInt((currentEnemy.strength - player.defense / 2) * rng));
            player.currentHP = Mathf.Max(0, player.currentHP - dmg);
            Log($"{currentEnemy.fighterName} attacks for {dmg}!");
        }

        UpdateHPText();
        yield return new WaitForSeconds(1.2f);

        if (!player.isAlive)
        {
            Log("You have been defeated...");
        }
        else
        {
            BeginPlayerTurn();
        }
    }

    IEnumerator EnemyDefeated()
    {
        Log($"{currentEnemy.fighterName} defeated!");
        yield return new WaitForSeconds(1.5f);

        enemyIndex++;
        Fighter[] enemies = BattleManager.Instance.GetEnemies();

        if (enemyIndex >= enemies.Length)
        {
            Log("All enemies defeated! You win!");
        }
        else
        {
            int heal = Mathf.RoundToInt(player.maxHP * 0.3f);
            player.currentHP = Mathf.Min(player.maxHP, player.currentHP + heal);
            currentEnemy = enemies[enemyIndex];
            currentEnemy.currentHP = currentEnemy.maxHP;
            UpdateHPText();
            Log($"You recover some HP. Next: {currentEnemy.fighterName}!");
            yield return new WaitForSeconds(2f);
            StartCoroutine(StartBattle());
        }
    }

    void Log(string msg)
    {
        battleLog.text = msg;
    }

    void UpdateHPText()
    {
        playerHPText.text = $"{player.fighterName}  HP: {player.currentHP}/{player.maxHP}";
        enemyHPText.text = $"{currentEnemy.fighterName}  HP: {currentEnemy.currentHP}/{currentEnemy.maxHP}";
    }

    void SetButtons(bool on)
    {
        attackButton.interactable = on;
        specialButton.interactable = on;
        defendButton.interactable = on;
    }
}