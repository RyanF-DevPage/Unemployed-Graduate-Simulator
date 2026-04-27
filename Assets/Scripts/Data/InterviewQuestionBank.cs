using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Simulator_Game
{
    [CreateAssetMenu(fileName = "InterviewQuestionBank", menuName = "Game/InterviewQuestionBank")]
    public class InterviewQuestionBank : ScriptableObject
    {
        public List<InterviewQuestion> questions;

        public List<InterviewQuestion> PickRandomQuestions(int count)
        {
            var pool = new List<InterviewQuestion>(questions);
            var picked = new List<InterviewQuestion>();
            int n = Mathf.Min(count, pool.Count);
            for (int i = 0; i < n; i++)
            {
                int idx = URandom.Range(0, pool.Count);
                picked.Add(pool[idx]);
                pool.RemoveAt(idx);
            }
            return picked;
        }

        public List<InterviewAnswer> PickRandomAnswers(InterviewQuestion question, int count)
        {
            var pool = new List<InterviewAnswer>(question.answerPool);
            var picked = new List<InterviewAnswer>();
            int n = Mathf.Min(count, pool.Count);
            for (int i = 0; i < n; i++)
            {
                int idx = URandom.Range(0, pool.Count);
                picked.Add(pool[idx]);
                pool.RemoveAt(idx);
            }
            return picked;
        }
    }
}
