using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "GameTime", menuName = "Scriptable Objects/GameTime")]
    public class GameTime : ScriptableObject
    {
        [Header("Default Start Time")]
        public int defaultStartMonth = 0;
        public int defaultStartDay = 1;
        public int defaultStartYear = 2026;
        public int defaultStartHour = 9;
        public int defaultStartMinute = 0;

        [Header("Time Scale")]
        public float minutePerRealSecond = .5f; // Time scale for the game (1 = 1 minute per real second)

        public float DefaultStartInMinutes => defaultStartDay * 24 * 60 + defaultStartHour * 60 + defaultStartMinute; // Calculate the default start time in minutes

    }
}
