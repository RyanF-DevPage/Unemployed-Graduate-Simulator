namespace Simulator_Game
{
    /// <summary>
    /// Implemented by any system that needs to persist state between sessions
    /// (e.g. GameTimeManager, player stats, wallet, job applications).
    /// The SaveManager should iterate all ISaveable instances on save/load.
    /// </summary>
    public interface ISaveable
    {
        void Save();
        void Load();
    }
}
