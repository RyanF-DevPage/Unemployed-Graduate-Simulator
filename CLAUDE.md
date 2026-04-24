# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Unemployed Graduate** is a 2D life/career simulation game built in Unity 6 (6000.3.11f1) using URP. The player manages a graduate character's daily life stats (Health, Hunger, Mood) while applying for jobs, working, earning income/XP, and leveling up to access better opportunities.

## Proeject scope
Always think this project as a future expandable prototype. The goal is to ship it as a commercial game, so every design choice make should consider its scalability and maintainability.

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
All managers live on the `Managers` GameObject, use the singleton pattern, and persist across scenes via `DontDestroyOnLoad`:

- `GameTimeManager` — drives in-game time (configurable via `GameTime` ScriptableObject). Broadcasts `OnTimeChanged` every in-game minute tick. Systems that react to time implement `ITimeDependent`.
- `PlayerStatsManager` — owns Health/Mood/Hunger depletion (via `ITimeDependent`), XP/leveling (`ILevelable`), wallet (`IWallet`), and JSON save/load (`ISaveable`). Fires `OnGameOver` when any stat hits 0.
- `ApplicationStateManager` — single source of truth for all job application states. Implements `IJobApplication`. Stores `Dictionary<JobData, ApplicationStatus>` and exposes the four pipeline transitions: `TryApply`, `TryAdvanceToInterview`, `TryDirectOffer`, `TryPassInterview`. Eligibility rates are tunable `[SerializeField]` floats (plug skill bonuses in here later).
- `WindowsManager` — controls opening/closing UI windows (activates/deactivates GameObjects).
- `GameManager` — handles scene entry (NewGame/Load). Win/loss/cutscene callbacks are stubs.
- `SceneChanger` — thin scene-loading singleton.

### ScriptableObject Configuration
- `PlayerData` SO (`Assets/Data/`) — Health, Mood, Hunger (0–100), level, XP, wallet balance. `PlayerStatsManager` reads/writes this at runtime and serialises it to JSON on save.
- `GameTime` SO (`Assets/Scripts/Configurations/`) — time scale and start values. Currently: Day 1, Hour 23, Minute 0, 5 min/real-second.
- `JobData` SO (`Assets/Data/Jobs/`) — implements `IJob`. One asset per job listing (title, company, type, salary, required level, requirements, responsibilities, benefits). Created via `Game/Job` asset menu.
- `JobDatabase` SO (`Assets/Data/Jobs/`) — holds a `List<JobData>` and exposes `GetEligibleJobs(playerLevel)`.

### Interface-Driven Design
All major gameplay systems are defined as interfaces in `Assets/Scripts/Interfaces/`:

| Interface | Purpose |
|---|---|
| `IApp` | Any virtual desktop app — Open/Close |
| `IJob` | Job listing data: title, company, salary, required level, eligibility check |
| `IJobApplication` | Implemented by `ApplicationStateManager` — `GetStatus`, `TryApply`, `CanAdvanceTo`, `OnApplicationStatusChanged` event |
| `ILevelable` | Level/XP tracking with `OnLevelUp` event — implemented by `PlayerStatsManager` |
| `IWallet` | Currency management (`AddFunds`, `TrySpend`) — implemented by `PlayerStatsManager` |
| `IWorkable` | Active work sessions — StartWorking/StopWorking, OnWorkCompleted with earnings |
| `IXPSource` | Any activity that grants XP |
| `IStatAffector` | Modifies player stats (food, events, entertainment) |
| `ITimeDependent` | Subscribes to `GameTimeManager.OnTimeChanged` ticks |
| `IRandomEvent` | Chance-based events with trigger probability and conditions |
| `ISaveable` | Save/Load persistence contract |
| `ISkill` | Skill tree node — tier, SP cost, prerequisites, unlock state |
| `IInterviewQuestion` | Interview question with answer choices and success weight |

### Job Search Flow
The browser job search is fully wired end-to-end:
1. `SearchButton` (on `Browser/SearchEngine/SearchButton`) — on click, calls `JobBulletinPanelUI.Refresh(playerLevel)`.
2. `JobBulletinPanelUI` (on `JobBulletinPanel`) — queries `JobDatabase.GetEligibleJobs`, instantiates `Posting.prefab` entries into `Jobs` container.
3. `PostingEntryUI` (on `Posting.prefab`) — populates Title/Type/Salary/Company fields; button click calls back into `JobBulletinPanelUI`.
4. `JobPostingPanelUI` (on `JobPostingPanel`) — calls `JobPostingPageUI.Populate(job)` and manages the Apply button + confirmation popup.
5. `JobPostingPageUI` (on `JobPostingPanel/.../Content`) — fills the scroll view text fields.
6. Confirming "Apply" calls `ApplicationStateManager.Instance.TryApply(job)` → NotApplied → Pending.

### UI Binding Pattern
UI components subscribe to events from managers or ScriptableObjects directly:
- `ClockUI` → `GameTimeManager.OnTimeChanged`
- `StatsBarUI` → `PlayerStatsManager.OnHealthChanged` / `OnMoodChanged` / `OnHungerChanged`
- `JobPostingPanelUI` → polls `ApplicationStateManager.Instance.GetStatus()` on open

### Prefab Templates
`Assets/Prefabs/`:
- `Posting.prefab` — bulletin board entry; has `PostingEntryUI` with TMP_Text fields wired (Title, Type, Salary, One-liner).
- `AppTemplate.prefab` — base for desktop apps implementing `IApp`.
- `StatsTemplate.prefab` — stat bar display UI.

## Implementation Status

**Working systems:** Time management, stat display (Health/Mood/Hunger bars), ClockUI, scene transitions, main menu, task panel toggle animation, XP/leveling logic, wallet, stat depletion, JSON save/load for player data, job bulletin panel, job posting detail panel, Apply button with confirmation popup, application state machine (NotApplied → Pending), `SearchButton` wired to bulletin.

**Skeleton/placeholder (not yet implemented):**
- `GameManager` — `LoadOpeningCutScene()`, `OnPlayerWin()`, `OnPlayerLose()` are empty stubs; no Game Over or Win screen exists yet.
- Application wait timer — `TryAdvanceToInterview` / `TryDirectOffer` exist but nothing calls them on a timer yet.
- Interview system — `IInterviewQuestion` defined, no UI or evaluation logic.
- Work sessions — `IWorkable` defined, no `StartWorking` implementation or time-jump.
- Save system — `ISaveable` defined and implemented on `PlayerStatsManager`; not yet wired to a save/load trigger in the game loop.
- Random events — `IRandomEvent` defined, no concrete events or trigger system.
- XP/leveling HUD — `PlayerStatsManager` tracks XP and level but there is no HUD widget displaying them.
- Skill tree — `ISkill` defined, no UI or skill logic implemented.
- Hunger Eats, Wallet, Notes apps — icons exist on the desktop but no app panels built.
- More job listings — only one `JobData` asset ("Warehouse Cleaner") exists in `Assets/Data/Jobs/`.

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

### Three game pillars mandate: 
(1) players always know their application status — no ghosting, 
(2) random events always affect stats, 
(3) player freedom to explore jobs with outcome shaped by luck + skills + stats.

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

Triggered probabilistically via `IRandomEvent.CanTrigger()` + `Trigger()`. Affect player stats (`IStatAffector`). 



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