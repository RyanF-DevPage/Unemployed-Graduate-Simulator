# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Unemployed Graduate** is a 2D life/career simulation game built in Unity 6 (6000.3.11f1) using URP. The player manages a graduate character's daily life stats (Health, Hunger, Mood) while applying for jobs, working, earning income/XP, and leveling up to access better opportunities.

## Unity Development

### Common Operations
- **Open in Unity:** Open the project folder in Unity Hub (Unity 6000.3.11f1)
- **Run tests:** Window → General → Test Runner → Run All (uses `com.unity.test-framework` 1.6.0)
- **Build:** File → Build Settings → Build
- **Main scenes:** `Assets/Scenes/MainMenu.unity`, `Assets/Scenes/GameScene.unity`

### Key Packages
- URP 17.3.0 (`com.unity.render-pipelines.universal`)
- Input System 1.19.0 (`com.unity.inputsystem`)
- UGUI 2.0.0 (`com.unity.ui`)
- 2D Animation 13.0.4

## Architecture

### Singleton Managers
Core managers use the singleton pattern and persist across scenes:
- `GameTimeManager` — drives in-game time (configurable via `GameTime` ScriptableObject). Broadcasts `OnTimeChanged` every in-game minute tick. Systems that react to time implement `ITimeDependent`.
- `WindowsManager` — controls opening/closing UI windows (apps on the virtual desktop)
- `SceneChanger` — scene loading with `DontDestroyOnLoad`

### ScriptableObject Configuration
Stats and time config live in `Assets/Scripts/Configurations/`:
- `PlayerStat` SO — Health, Hunger, Mood each as normalized 0–1 values with `UnityEvent OnStatValueChanged`. UI (e.g., `StatsBarUI`) subscribes to this event.
- `GameTime` SO — configures time scale and start values. Currently set to Day 1, Hour 23, Minute 0, 5 min/real-second.

### Interface-Driven Design
All major gameplay systems are defined as interfaces in `Assets/Scripts/Interfaces/`:

| Interface | Purpose |
|---|---|
| `IApp` | Any virtual desktop app (Browser, Notes, etc.) — Open/Close |
| `IJob` | Job listing data: title, company, salary, required level, eligibility |
| `IJobApplication` | Application lifecycle state machine (NotApplied → Pending → Interview → Accepted/Rejected) with status change events |
| `ILevelable` | Level/XP tracking with level-up events |
| `IWorkable` | Active work sessions — StartWorking/StopWorking, OnWorkCompleted with earnings |
| `IXPSource` | Any activity that grants XP |
| `IStatAffector` | Modifies PlayerStats (food, events, entertainment) |
| `ITimeDependent` | Subscribes to time ticks for passive effects |
| `IRandomEvent` | Chance-based events with trigger probability and conditions |
| `ISaveable` | Save/Load persistence contract for SaveManager |

### UI Binding Pattern
UI components subscribe to events from managers or ScriptableObjects directly:
- `ClockUI` → subscribes to `GameTimeManager.OnTimeChanged`
- `StatsBarUI` → subscribes to `PlayerStat.OnStatValueChanged`

### Prefab Templates
`Assets/Prefabs/` contains base templates for extension:
- `AppTemplate.prefab` — base for desktop apps implementing `IApp`
- `JobPostingPageTemplate.prefab` — job browser listing layout
- `StatsTemplate.prefab` — stat display UI

## Implementation Status

**Working systems:** Time management, stat display, ClockUI, scene transitions, main menu, task panel toggle animation, stat image fill.

**Skeleton/placeholder (not yet implemented):** `SearchButton`, `Clock` class, save system (`ISaveable`), random events (`IRandomEvent`), XP/leveling (`ILevelable`, `IXPSource`), job application state machine, active work sessions.

---

## Game Design Document (Prototype)

> Source: [Notion GDD](https://www.notion.so/4f984e5c0b534fbaafc59f670da9baa7) — *Unemployed Graduate Simulator*
> Prototype deadline: **2026-05-08**

### Core Loop

1. Search for jobs → Apply → Wait (>5 min real time per job tier) → Rejected or Interview
2. Behavioral interview (multiple-choice, evaluated against Career Skills tree) → Rejected or Job Offer
3. Accept offer → Start Working → time jumps forward → wallet increases
4. Eat food (Hunger Eats app) → restore Hunger, gain XP, spend money
5. Do entertainment activity → restore Mood, gain XP
6. If any stat hits 0 → **Game Over**
7. Earn enough XP → Level Up → gain Skill Points → spend in Skill Tree
8. Wallet reaches **$1,000,000** → **Win**

### Player Stats

| Stat | Depletes when | Fail condition |
|---|---|---|
| Health | Hunger or Mood at critical levels | Reaches 0 |
| Hunger | Passively over time | Reaches 0 |
| Mood | Working, rejections, time | Reaches 0 |

Stats are normalized 0–1 (`PlayerStat` SO). Time passing drives passive depletion via `ITimeDependent`.

### XP & Leveling

- XP sources: eating, working, applying for jobs, entertainment
- Level Up → +1 Skill Point (SP) (bonus SP from rare random events)
- Higher level unlocks higher-tier job listings and better interview pass rates

### Job Application Flow

```
NotApplied → [Apply] → Pending (wait timer)
  → Rejected (Mood hit) → back to search
  → Interview → Behavioral Q&A (Career Skills evaluated)
    → Rejected (Mood hit)
    → Accepted → Job Offer → Accept → IWorkable.StartWorking()
```

Time jumps forward by `WorkDurationMinutes` during work. Working also drains Mood.

### Virtual Desktop Apps (Scene 2)

| App / Folder | Purpose |
|---|---|
| Browser | Job search, apply, freelance gigs (via Side Hustle skill) |
| Tasks | Active job assignments, notifications (rejections, offers) |
| Notes | Journaling activity (Mood restore with skill) |
| Wallet | Balance display; Investments tab (unlocked via skill) |
| Hunger Eats | Order food to restore Hunger; Lottery Ticket (unlocked via skill) |
| Documents → Skills | Skill Tree screen |
| Documents → Resumes | Resume management |

All apps implement `IApp` (Open/Close). The desktop icon grid opens them via `WindowsManager`.

### Skill Tree System

> Full design: [Notion — Skill Tree System](https://www.notion.so/34523b2dc711818f8388e2ad1502d675)

**Economy:** 1 SP per level-up. No respec in prototype. Tier 2 requires 2 Tier-1 skills; Tier 3 requires 2 Tier-2 skills.

**Four trees:**

**🧠 Career Skills** — application success, job unlocks, interview performance
- T1: Resume Polish (+10% screening), Cover Letter Basics (+5% success), Job Search Efficiency (+2 listings)
- T2: Interview Confidence (−15% stress in interview), Networking (referral chance, +20%), Industry Focus (+15% salary in chosen industry)
- T3: Dream Job Tracker (wishlist + alerts), Salary Negotiation (+10–30% offer)

**💪 Wellbeing** — stat decay management, survival
- T1: Meal Prep (+15% Hunger restore), Sleep Hygiene (+10 Health at midnight), Stress Awareness (HUD warning at <25% Mood)
- T2: Exercise Habit (new activity, +20 Mood, 1 hr), Comfort Eating (+5 Mood on eat), Journaling (+10 Mood from Notes)
- T3: Iron Will (Mood fail threshold −10 buffer), Healthy Routine (all stats decay 20% slower)

**💼 Work Performance** — income speed, passive income
- T1: Fast Learner (first shift −20% time), Attention to Detail (−10% Mood drain while working), Side Hustle Basics (freelance gigs in Browser)
- T2: Overtime Tolerance (+50% pay, +10% Mood drain), Performance Bonus (10% chance bonus payout), Freelance Network (+15% freelance pay)
- T3: Passive Income (Investments tab in Wallet), Promotion Track (5 days same company → +30% salary event)

**🎲 Luck & Chaos** — random event manipulation, high-risk rewards
- T1: Silver Lining (negative events +15% chance for XP bonus), Early Bird (exclusive listing before 9am), Social Media Scroll (Viral Post event)
- T2: Gambler's Instinct (Lottery Ticket in Hunger Eats), Networking Serendipity (10% chance random job referral), Viral Brand (Viral Post → freelance gig chance)
- T3: Lucky Break (one-time manual massive positive event), Chaos Immune (worst negative events blocked)

### Skill Tree UI Requirements
- All four trees displayed side-by-side or as tabs in the Skills screen
- Locked skills: grayed out with padlock icon
- Hover tooltip: name, description, cost, prerequisite
- Purchased skills: gold outline highlight
- SP balance shown in top-right corner

### Random Events

Triggered probabilistically via `IRandomEvent.CanTrigger()` + `Trigger()`. Affect player stats (`IStatAffector`). Three game pillars mandate: (1) players always know their application status — no ghosting, (2) random events always affect stats, (3) player freedom to explore jobs with outcome shaped by luck + skills + stats.

### Win / Fail Conditions

- **Win:** Wallet ≥ $1,000,000 → show win screen with final stats and elapsed time
- **Fail:** Any stat (Health, Hunger, or Mood) reaches 0 → Game Over screen

### Key Design Decisions

- **Time control:** Player cannot pause time; events (work, eat) jump time forward. Pressure mechanic.
- **Job response delay:** >5 min real time per tier — creates tension and forces stat management while waiting.
- **Interview system:** Multiple-choice questions evaluated against Career Skills tree investment.
- **Input:** Mouse left-click only.

## KEY DESIGN THINKING
1. Think Before Coding
Don't assume. Don't hide confusion. Surface tradeoffs.

Before implementing:

State your assumptions explicitly. If uncertain, ask.
If multiple interpretations exist, present them - don't pick silently.
If a simpler approach exists, say so. Push back when warranted.
If something is unclear, stop. Name what's confusing. Ask.
2. Simplicity First
Minimum code that solves the problem. Nothing speculative.

No features beyond what was asked.
No abstractions for single-use code.
No "flexibility" or "configurability" that wasn't requested.
No error handling for impossible scenarios.
If you write 200 lines and it could be 50, rewrite it.
Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

3. Surgical Changes
Touch only what you must. Clean up only your own mess.

When editing existing code:

Don't "improve" adjacent code, comments, or formatting.
Don't refactor things that aren't broken.
Match existing style, even if you'd do it differently.
If you notice unrelated dead code, mention it - don't delete it.
When your changes create orphans:

Remove imports/variables/functions that YOUR changes made unused.
Don't remove pre-existing dead code unless asked.
The test: Every changed line should trace directly to the user's request.

4. Goal-Driven Execution
Define success criteria. Loop until verified.

Transform tasks into verifiable goals:

"Add validation" → "Write tests for invalid inputs, then make them pass"
"Fix the bug" → "Write a test that reproduces it, then make it pass"
"Refactor X" → "Ensure tests pass before and after"
For multi-step tasks, state a brief plan:

1. [Step] → verify: [check]
2. [Step] → verify: [check]
3. [Step] → verify: [check]
Strong success criteria let you loop independently. Weak criteria ("make it work") require constant clarification.