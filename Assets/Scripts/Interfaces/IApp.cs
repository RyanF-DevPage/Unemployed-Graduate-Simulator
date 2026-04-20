namespace Simulator_Game
{
    /// <summary>
    /// Represents an application on the virtual desktop that can be opened and closed.
    /// Implemented by any app icon that maps to a window (Browser, Tasks, Notes, Wallet, Hunger Eats, etc.).
    /// </summary>
    public interface IApp
    {
        string AppName { get; }
        bool IsOpen { get; }

        void Open();
        void Close();
    }
}
