using UnityEngine;

namespace Simulator_Game
{
    [RequireComponent(typeof(Collider2D))]
    public class Food : MonoBehaviour, IStatAffector
    {
        [SerializeField] private float hungerGain = 30f;
        [SerializeField] private float moodGain   = 10f;

        private Collider2D _collider;

        private void Awake()  => _collider = GetComponent<Collider2D>();

        private void Start()     => InputManager.Instance.OnWorldClick += OnClick;
        private void OnDestroy()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnWorldClick -= OnClick;
        }

        private void OnClick(Vector2 worldPos)
        {
            if (_collider.OverlapPoint(worldPos))
                ApplyEffect(PlayerStatsManager.Instance);
        }

        public void ApplyEffect(PlayerStatsManager manager)
        {
            manager.ModifyHunger(hungerGain);
            manager.ModifyMood(moodGain);
            Destroy(gameObject);
        }
    }
}
