using UnityEngine;

namespace Simulator_Game
{
    public class AnimatorController : MonoBehaviour
    {
        Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void ToggleAnimation(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
    }
}