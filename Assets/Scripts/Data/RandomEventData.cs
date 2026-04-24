using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator_Game
{
    public enum EventEffectType { Health, Mood, Hunger, Money, XP, Time }

    [Serializable]
    public class EventEffect
    {
        public EventEffectType type;
        public float amount; // positive = gain, negative = loss
    }

    [CreateAssetMenu(fileName = "NewRandomEvent", menuName = "Game/RandomEvent")]
    public class RandomEventData : ScriptableObject, IRandomEvent
    {
        [Header("Display")]
        public string eventTitle;
        [TextArea] public string eventDescription;

        [Header("Trigger")]
        [Range(0f, 1f)] public float triggerProbability = 0.1f;

        [Header("Effects")]
        public List<EventEffect> effects;

        public string EventName          => eventTitle;
        public string EventDescription   => eventDescription;
        public float  TriggerProbability => triggerProbability;

        public bool CanTrigger() => UnityEngine.Random.value <= triggerProbability;

        public void Trigger()
        {
            var stats = PlayerStatsManager.Instance;
            if (stats == null) return;

            var time = GameTimeManager.Instance;
            foreach (var effect in effects)
            {
                switch (effect.type)
                {
                    case EventEffectType.Health: stats.ModifyHealth(effect.amount); break;
                    case EventEffectType.Mood:   stats.ModifyMood(effect.amount);   break;
                    case EventEffectType.Hunger: stats.ModifyHunger(effect.amount); break;
                    case EventEffectType.Money:  stats.AddFunds(effect.amount);     break;
                    case EventEffectType.XP:     stats.AddXP(effect.amount);        break;
                    case EventEffectType.Time:   time.SkipMinutes(effect.amount);   break;
                }
            }
        }
    }
}
