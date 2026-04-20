using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "GameTime", menuName = "Scriptable Objects/GameTime")]
    public class GameTime : ScriptableObject
    {
        [Header("Default Start Time")]
        public int defaultStartDay = 1;
        public int defaultStartHour = 9;
        public int defaultStartMinute = 0;

        [Header("Time Scale")]
        public float minutePerRealSecond = .5f;

        public float DefaultStartInMinutes => defaultStartDay * 24 * 60 + defaultStartHour * 60 + defaultStartMinute;

    }
}
