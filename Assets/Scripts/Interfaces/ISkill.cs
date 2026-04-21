using System;

public interface ISkill
{
    string SkillName { get; }
    int Tier { get; }           // 1, 2, or 3
    int SPCost { get; }
    bool IsUnlocked { get; }
    ISkill[] Prerequisites { get; }

    bool CanUnlock(int availableSP);
    void Unlock();
}
