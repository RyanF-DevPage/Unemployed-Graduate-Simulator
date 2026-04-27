using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "NewInterviewQuestion", menuName = "Game/InterviewQuestion")]
    public class InterviewQuestion : ScriptableObject
    {
        [TextArea] public string questionText;
        public List<InterviewAnswer> answerPool;
    }

    [Serializable]
    public class InterviewAnswer
    {
        [TextArea] public string answerText;
        [Range(-0.3f, 0.3f)] public float scoreModifier;
    }
}
