using UnityEngine;

public class Fighter : MonoBehaviour
{
    public string fighterName;
    public int maxHP;
    public int currentHP;
    public int strength;
    public int intelligence;
    public int speed;
    public int defense;

    public bool isAlive => currentHP > 0;

    void Awake()
    {
        currentHP = maxHP;
    }
}