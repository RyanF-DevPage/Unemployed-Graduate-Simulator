using UnityEngine;

namespace Simulator_Game
{
    public class TestPlayerStat : MonoBehaviour
    {
        [SerializeField] private PlayerStat stat;

        public void StatChanger()
        {
            stat.CurrentValue -= 50f;
        }

    }
}
