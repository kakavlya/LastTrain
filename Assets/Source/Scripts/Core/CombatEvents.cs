using System;

public static class CombatEvents
{
    public static event Action EnemyHit;
    public static void RaiseHit() => EnemyHit?.Invoke();
}
