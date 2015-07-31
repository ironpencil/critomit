public enum WeaponLocation
{
    None,
    Primary,
    Secondary,
    Utility
}

public enum EffectSource
{
    Universal,
    Player,
    Enemy
}

public enum GameState
{
    Title,
    Lobby,
    Arena,
    Campaign
}

public enum GameLevel
{
    Title,
    Lobby,
    Arena,
    Level1,
    Level2,
    Level3
}

public enum MutatorType
{
    None,
    Spin,
    EnemySpeed,
    EnemyDamage,
    EnemyExplode,
    EnemyHP,
    EnemyRegen,
    EnemyInvulnerable,
    EnemySplit,
    SpawnTime,
    WildBullets,
    TilesetSwap,
    EnemyMumble,
    HeavyBullets,
    RandomPlayerForce
}