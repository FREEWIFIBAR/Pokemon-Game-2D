using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Nuevo Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private int ID;

    [SerializeField] private string name;

    public string Name => name;

    [TextArea] [SerializeField] private string description;

    public string Description => description;

    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;

    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;

    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;

    [SerializeField] private int catchRate = 255;

    public int CatchRate => catchRate;

    //Stats
    [SerializeField] private int maxHP;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spAttack;
    [SerializeField] private int spDefense;
    [SerializeField] private int speed;
    [SerializeField] private int expBase;
    [SerializeField] private GrowthRate growthRate;

    public int MaxHP => maxHP;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public int ExpBase => expBase;

    public GrowthRate GrowthRate => growthRate;

    [SerializeField] private List<LearnableMove> learnableMoves;

    public List<LearnableMove> LearnableMoves => learnableMoves;

    public static int NUMBER_OF_LEARNABLE_MOVES { get; } = 4;

    public int GetNecessaryExpForLevel(int level)
    {
        switch (growthRate)
        {
            case GrowthRate.Fast:
                return Mathf.FloorToInt(4 * Mathf.Pow(level, 3) / 5);
                break;

            case GrowthRate.MediumFast:
                return Mathf.FloorToInt(Mathf.Pow(level, 3));
                break;

            case GrowthRate.MediumSlow:
                return Mathf.FloorToInt(6 * Mathf.Pow(level, 3) / 5 - 15 * Mathf.Pow(level, 2) + 100 * level - 140);
                break;

            case GrowthRate.Slow:
                return Mathf.FloorToInt(5 * Mathf.Pow(level, 3) / 4);
                break;

            case GrowthRate.Erratic:
                if (level < 50)
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (100 - level) / 50);
                }
                else if (level < 68)
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (150 - level) / 100);
                }
                else if (level < 98)
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * Mathf.FloorToInt((1911 - 10 * level) / 3) / 500);
                }
                else
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (160 - level) / 100);
                }

                break;

            case GrowthRate.Fluctuating:
                if (level < 15)
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.FloorToInt((level + 1) / 3) + 24) / 50);
                }
                else if (level < 36)
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (level + 14) / 50);
                }
                else
                {
                    return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.FloorToInt(level / 2) + 32) / 50);
                }

                break;
        }

        return -1;
    }
}

public enum GrowthRate
{
    Erratic,
    Fast,
    MediumFast,
    MediumSlow,
    Slow,
    Fluctuating
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fight,
    Poison,
    Ground,
    Fly,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

public enum Stat
{
    Ataque,
    Defensa,
    AtaqueSp,
    DefensaSp,
    Velocidad,
    Precision,
    Evasion
}

public class TypeMatrix
{
    private static float[][] matrix = 
    {
    //                   Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar   Ste   Fai
    /*Nor*/ new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f, 1.0f, 1.0f, 0.5f, 1.0f},
    /*Fir*/ new float[] {1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 2.0f, 1.0f},
    /*Wat*/ new float[] {1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f},
    /*Ele*/ new float[] {1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f},
    /*Gra*/ new float[] {1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f},
    /*Ice*/ new float[] {1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f},
    /*Fig*/ new float[] {2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.0f, 1.0f, 2.0f, 2.0f, 0.5f},
    /*Poi*/ new float[] {1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.0f, 2.0f},
    /*Gro*/ new float[] {1.0f, 2.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f},
    /*Fly*/ new float[] {1.0f, 1.0f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f},
    /*Psy*/ new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.5f, 1.0f},
    /*Bug*/ new float[] {1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f, 0.5f},
    /*Roc*/ new float[] {1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f},
    /*Gho*/ new float[] {0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f},
    /*Dra*/ new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.0f},
    /*Dar*/ new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f},
    /*Ste*/ new float[] {1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f},
    /*Fai*/ new float[] {1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 0.5f, 1.0f}
    };

    public static float GetMultEffectiveness(PokemonType attackType, PokemonType pokemonDefenderType)
    {
        if (attackType == PokemonType.None || pokemonDefenderType == PokemonType.None)
        {
            return 1.0f;
        }

        int row = (int)attackType;
        int col = (int)pokemonDefenderType;

        return matrix[row - 1][col - 1];
    }
}

[Serializable]
public class LearnableMove
{
    [SerializeField] private MoveBase move;
    [SerializeField] private int level;

    public MoveBase Move => move;
    public int Level => level;
}