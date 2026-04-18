using UnityEngine;

namespace Simulator_Game
{
    public class AnimatorController : MonoBehaviour
    {
        Animator animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
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