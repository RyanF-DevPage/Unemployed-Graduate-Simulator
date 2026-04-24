using UnityEngine;

namespace Simulator_Game
{
    public static class WindowsManager
    {
        public static void OpenWindow(GameObject window)
        {
            window.SetActive(true);
        }

        public static void CloseWindow(GameObject window)
        {
            window.SetActive(false);
        }

    }
}