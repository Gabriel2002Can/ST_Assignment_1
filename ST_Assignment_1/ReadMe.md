# WorkoutJournal — API Specification (base for AI code generation)

> *Purpose:* This document is a complete, machine-friendly specification in English to guide an AI or developer to implement a .NET Web API (ASP.NET Core) for a *WorkoutJournal* app focused on *calisthenics*. It includes functional requirements, data models, OpenAPI-ready endpoints, database schema, and UX behavior (calendar, daily exercises, tracking completed sets/reps/rest).

---

## 1. Project overview

WorkoutJournal is a backend API for a personal workout tracker focused on calisthenics. Key features:

* A calendar view (day-by-day) that lists the exercises scheduled for each day.
* Daily workout sessions that contain ordered exercises, with target sets, target reps (or durations), and recommended rest between sets.
* Ability to mark individual sets as completed and record actual reps, duration, and rest used.
* History: view what exercises and results happened on past days.
* CRUD for exercises, workout templates (reusable daily templates), and scheduled calendar entries.
* Configurable options to change number of sets, reps, rest, and frequency.
* Persist all data in a relational database.

## 2. High-level API goals

* Provide a complete RESTful API with Swagger documentation.
* Support full CRUD for Exercise, Template, WorkoutSession, and CalendarEntry resources.
* Store detailed per-set completion records so the frontend can show progress across days.
* Keep authentication optional for MVP (support token-based auth extension later).

## 3. Core data models (conceptual)

### User

* Id (GUID)
* Username (string)
* Email (string)
* PasswordHash (string) — for future auth
* CreatedAt (DateTime)

### Exercise (master list)

* Id (GUID)
* Name (string) — e.g. "Push-up", "Pull-up"
* Category (enum) — Push / Pull / Legs / Core / Conditioning
* Description (string)
* DefaultSets (int)
* DefaultReps (string) — reps or duration (e.g. "10", "30s")
* DefaultRestSeconds (int)
* Difficulty (enum) — Beginner / Intermediate / Advanced
* CreatedAt (DateTime)

### ExerciseTemplateItem

(Used by templates or scheduled day; ordered list item)

* Id (GUID)
* ExerciseId (FK)
* Order (int)
* TargetSets (int)
* TargetReps (string)
* TargetRestSeconds (int)
* Notes (string)

### Template (reusable routine)

* Id (GUID)
* Name (string)
* Description (string)
* Items (list of ExerciseTemplateItem)
* Frequency (enum) — Daily / EveryOtherDay / Weekly / Custom (CRON-like)

### CalendarEntry

* Id (GUID)
* UserId (GUID)
* Date (Date) — day of the entry
* TemplateId (optional)
* CustomItems (optional list of ExerciseTemplateItem) — if user overrides template for that date
* Status (enum) — Scheduled / Skipped / Completed

### WorkoutSession

* Id (GUID)
* UserId (GUID)
* CalendarEntryId (optional GUID)
* StartTime (DateTime)
* EndTime (DateTime?)
* SessionItems (list of SessionExercise)
* Notes (string)

### SessionExercise (per exercise in a session)

* Id (GUID)
* SessionId (GUID)
* ExerciseId (GUID)
* Order (int)
* TargetSets (int)
* TargetReps (string)
* TargetRestSeconds (int)
* CompletedSets (int)
* Sets (list of SetRecord)

### SetRecord

* SetNumber (int)
* PlannedRepsOrDuration (string)
* ActualRepsOrDuration (string)
* RestAfterSetSeconds (int)
* Timestamp (DateTime)

## 4. Example database schema (tables & essential columns)

* Users (Id, Username, Email, PasswordHash, CreatedAt)
* Exercises (Id, Name, Category, Description, DefaultSets, DefaultReps, DefaultRestSeconds, Difficulty, CreatedAt)
* Templates (Id, Name, Description, Frequency)
* TemplateItems (Id, TemplateId, ExerciseId, Order, TargetSets, TargetReps, TargetRestSeconds, Notes)
* CalendarEntries (Id, UserId, Date, TemplateId, Status)
* WorkoutSessions (Id, UserId, CalendarEntryId, StartTime, EndTime, Notes)
* SessionExercises (Id, SessionId, ExerciseId, Order, TargetSets, TargetReps, TargetRestSeconds, CompletedSets)
* SetRecords (Id, SessionExerciseId, SetNumber, PlannedRepsOrDuration, ActualRepsOrDuration, RestAfterSetSeconds, Timestamp)

Use EF Core migrations (code-first) for implementation.

## 5. API endpoints (OpenAPI-ready)

Paths are examples; include query params, request/response shapes.

### Exercises

* GET /api/exercises — list master exercises
* GET /api/exercises/{id} — get exercise
* POST /api/exercises — create
* PUT /api/exercises/{id} — update
* DELETE /api/exercises/{id} — delete

### Templates

* GET /api/templates
* GET /api/templates/{id}
* POST /api/templates
* PUT /api/templates/{id}
* DELETE /api/templates/{id}

### Calendar

* GET /api/calendar?start=yyyy-mm-dd&end=yyyy-mm-dd — list entries for date range
* POST /api/calendar — create a calendar entry (date + template or custom items)
* PUT /api/calendar/{id} — update (reschedule / change status)
* DELETE /api/calendar/{id}

### Sessions

* POST /api/sessions/start — start a session for a calendar entry (returns session id and session items prefilled)
* PATCH /api/sessions/{id}/exercise/{exerciseId}/set — record a completed set (body contains actual reps/duration and rest)
* PUT /api/sessions/{id}/end — end session (sets EndTime)
* GET /api/sessions/{id} — full session detail including SetRecords
* GET /api/sessions?userId=&start=&end= — session history

### Reports

* GET /api/reports/progress?exerciseId=&start=&end= — progression over time (volume, best reps, frequency)

## 6. Business & UX rules

* When creating a calendar entry from a Template, the API must copy template items into the entry (so users can override without changing the template).
* Starting a session should snapshot the planned targets to the WorkoutSession and SessionExercises.
* Each time the user records a set completion, create/append a SetRecord. Update CompletedSets accordingly.
* Provide endpoints to "adjust" planned target sets/reps for a single future calendar entry or template.
* Provide endpoints to change frequency of a template (e.g., daily → every other day → weekly).

## 7. Calisthenics exercises & suggested defaults

This app focuses on bodyweight calisthenics. The AI should seed an initial master Exercises table with a curated list of common calisthenics movements and *evidence-based default targets* (sets/reps/rest). The following sample list should be included (each record should set sensible DefaultSets, DefaultReps, and DefaultRestSeconds):

* *Push-up* — Default: 3 sets x 8–15 reps; Rest 60–90s
* *Incline Push-up* — 3 x 8–20; Rest 60s
* *Diamond Push-up* — 3 x 6–12; Rest 90s
* *Pike Push-up* — 3 x 6–12; Rest 90–120s
* *Pull-up* — 3–5 sets x 3–10 reps (use assisted/negatives for beginners); Rest 90–180s
* *Chin-up* — 3–5 x 3–10; Rest 90–180s
* *Bodyweight Row (Australian Pull-up)* — 3 x 8–15; Rest 60–90s
* *Dips (parallel bar)* — 3 x 6–15; Rest 90–120s
* *Squat (bodyweight)* — 3 x 12–20; Rest 60s
* *Jump Squat* — 3 x 8–15; Rest 60–90s
* *Pistol Squat (assisted)* — 3–5 x 3–8 per leg; Rest 90–180s
* *Lunges (walking/static)* — 3 x 8–15 per leg; Rest 60–90s
* *Plank* — 3 x 30–90s; Rest 60s
* *Hanging Leg Raise* — 3 x 8–15; Rest 60–90s
* *Mountain Climbers* — 3 x 30s; Rest 30–60s
* *Burpee* — 3 x 6–15; Rest 60–120s

> The defaults above are configurable by the user per template or calendar day.

## 8. Evidence & constraints (for the AI developer)

* Use established resistance training guidance for sets/reps/rest when choosing defaults and ranges. Typical ranges:

  * Strength / Power emphasis: fewer reps, more sets, longer rest (e.g., 3–6 sets × 3–6 reps, rest 2–5 min)
  * Hypertrophy / general strength: moderate sets/reps (3–5 sets × 6–12 reps), rest 60–120 s
  * Endurance: higher reps (>15) and shorter rest (<90 s)

* For calisthenics skills (e.g., muscle-up, pistol squat), allow lower rep ranges and longer rest to emphasize skill work and maximal effort.

## 9. Configuration & flexibility

Allow users to:

* Change DefaultSets, DefaultReps, DefaultRestSeconds on any Exercise.
* Override exercise targets in a TemplateItem or specific CalendarEntry.
* Set a Template frequency: daily, every N days, or weekly with specific weekdays.
* Enable "auto-schedule" — generate calendar entries from templates for a given date range.

## 10. Example JSON payloads

*Create Exercise*

json
POST /api/exercises
{
  "name": "Push-up",
  "category": "Push",
  "defaultSets": 3,
  "defaultReps": "8-12",
  "defaultRestSeconds": 75,
  "difficulty": "Beginner",
  "description": "Standard push-up. Keep body aligned."
}


*Start Session*

json
POST /api/sessions/start
{
  "userId": "...",
  "calendarEntryId": "..."
}


*Record completed set*

json
PATCH /api/sessions/{sessionId}/exercise/{sessionExerciseId}/set
{
  "setNumber": 1,
  "actualRepsOrDuration": "10",
  "restAfterSetSeconds": 80
}


## 11. Implementation notes (back-end)

* ASP.NET Core Web API (minimal or controller-based) with EF Core (Postgres or SQLite for MVP).
* Use migrations and seed the Exercises table with the curated calisthenics list.
* Swagger/OpenAPI doc auto-generated via AddSwaggerGen().
* Implement transactional behavior when starting a session and when recording sets.
* Add basic unit tests for session lifecycle and persistence of set records.

## 12. Acceptance criteria (for grader / AI tests)

* The API exposes CRUD endpoints for exercises and templates.
* Starting a session copies planned targets into a new session and allows recording set-by-set results.
* Calendar endpoints list scheduled workouts and allow creating entries from templates.
* All session data (set records) is persisted and queryable via session endpoints and reports.
* Swagger pages reflect all endpoints and models.

## 13. Deployment & database suggestions

* For a course submission, SQLite (file-based) or an online Postgres (Render/Supabase) are acceptable. Use environment variables for DB connection strings.
* Ensure CORS is enabled so a static frontend (GitHub Pages / Vercel) can call the API.

---

Last notes for the AI developer: seed with the calisthenics exercises above, make defaults editable, make calendar generation straightforward (template → calendar entries), and ensure every completed set is stored with a timestamp and actual reps/duration + rest used. This file is intentionally explicit so an AI coder can map models → EF Core classes → controllers → DTOs automatically.