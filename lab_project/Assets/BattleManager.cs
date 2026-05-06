using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    private Fighter[] enemies;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemies = new Fighter[3];
        enemies[0] = enemy1.GetComponent<Fighter>();
        enemies[1] = enemy2.GetComponent<Fighter>();
        enemies[2] = enemy3.GetComponent<Fighter>();
    }

    public Fighter[] GetEnemies()
    {
        return enemies;
    }
}