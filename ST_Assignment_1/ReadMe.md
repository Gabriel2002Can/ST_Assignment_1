# Workout Journal API

Authoritative Product Requirements & Technical Specification for the current implementation. This README supersedes any separate PRD file.

## 1. Objective
Expose a stateless RESTful API for managing workout Exercises and multi-exercise Templates, and for designating a single current Template for the application.

## 2. Stack
- .NET 9 (ASP.NET Core Web API)
- C# (nullable enabled)
- Entity Framework Core (Npgsql provider)
- System.Text.Json (default options)
- Swagger / OpenAPI (Swashbuckle with XML comments)
- CORS: fully permissive (AllowAnyOrigin/Method/Header)

## 3. High-Level Requirements
1. CRUD for Exercises
2. CRUD for Templates with ordered exercise items
3. Ability to set & fetch a single current Template
4. Eager load Items + nested Exercise when returning Templates
5. Support numeric or string enum values for TemplateFrequency (frontend sends numeric)
6. Do not require navigation properties in request payloads (Items use ExerciseId only)
7. Provide descriptive Swagger documentation at root (`/`)
8. Consistent HTTP status semantics (200/201/204/400/404)

## 4. Data Models
### 4.1 Exercise
| Property | Type | Notes |
|----------|------|-------|
| Id | int | PK |
| Name | string | Required (implicitly) |
| Description | string? | Optional |
| Category | int | Arbitrary grouping code |
| DefaultSets | int | Default set count |
| DefaultReps | string? | E.g. "8-12" |
| DefaultRestSeconds | int | Rest seconds |
| Difficulty | int | Arbitrary scale |

### 4.2 Template
| Property | Type | Notes |
|----------|------|-------|
| Id | int | PK |
| Name | string | Required |
| Description | string | Optional text (nullable not enforced) |
| Frequency | TemplateFrequency | Enum (Daily=0, EveryOtherDay=1, Weekly=2, Custom=3) |
| Items | ICollection<ExerciseTemplateItem> | Empty allowed |

### 4.3 ExerciseTemplateItem
| Property | Type | Notes |
|----------|------|-------|
| Id | int | PK |
| TemplateId | int | FK Template |
| ExerciseId | int | FK Exercise |
| Exercise | Exercise? | [JsonIgnore][BindNever] – not required in payload |
| Order | int | Display/execution order |
| TargetSets | int | Planned sets |
| TargetReps | string | Planned reps text |
| TargetRestSeconds | int | Planned rest seconds |
| Notes | string | Optional |

### 4.4 AppSettings (single-row configuration)
| Property | Type | Notes |
|----------|------|-------|
| Id | int | PK |
| CurrentTemplateId | int | FK to Template (may reference missing -> current fetch 404) |

### 4.5 Enum TemplateFrequency
- Daily = 0
- EveryOtherDay = 1
- Weekly = 2
- Custom = 3

## 5. Endpoints
Base path prefix: `/api`
All payloads JSON.

### 5.1 Exercises
| Method | Route | Description | Responses |
|--------|-------|-------------|-----------|
| GET | /api/exercises | List all exercises | 200 |
| GET | /api/exercises/{id} | Get exercise by id | 200, 404 |
| POST | /api/exercises | Create new exercise (Id ignored) | 201, 400 |
| PUT | /api/exercises/{id} | Full update of exercise | 204, 404 |
| DELETE | /api/exercises/{id} | Delete exercise | 204, 404 |

### 5.2 Templates
| Method | Route | Description | Responses |
|--------|-------|-------------|-----------|
| GET | /api/templates | List all templates (Items + Exercise eager loaded) | 200 |
| GET | /api/templates/{id} | Get template by id (Items + Exercise) | 200, 404 |
| POST | /api/templates | Create template (Items optional; each item uses ExerciseId) | 201, 400 |
| PUT | /api/templates/{id} | Full replace of template + Items collection | 204, 404 |
| DELETE | /api/templates/{id} | Delete template | 204, 404 |

### 5.3 Current Template
| Method | Route | Description | Responses |
|--------|-------|-------------|-----------|
| GET | /api/templates/current | Fetch current template (Items + Exercise) | 200, 404 |
| PUT | /api/templates/current/{id} | Set current template id | 204, 404 |

## 6. Behavioral Rules
- Template GET endpoints always return Items ordered as stored (no enforced reordering in API).
- PUT for Template performs full replacement: existing Items cleared and replaced with provided Items.
- Creating or updating Template Items requires only: ExerciseId, Order, TargetSets, TargetReps, TargetRestSeconds, Notes.
- Navigation properties in request bodies are ignored / not mandatory.
- Deleting an Exercise that is referenced by Template Items is not currently prevented (no constraint logic defined in spec).

## 7. Validation & Errors
- Default ASP.NET model binding & validation mechanics (ProblemDetails on 400).
- Invalid enum values produce 400 with conversion error info.
- Missing resources produce 404.
- No custom exception middleware; unhandled exceptions surface as 500.

## 8. Swagger / Documentation
- Served at `/` (root) with single document `v1` titled "Workout Journal API V1".
- XML comments populate operation summaries and parameter docs.

## 9. CORS
- Default policy: AllowAnyOrigin, AllowAnyHeader, AllowAnyMethod (supports static cross-origin frontend such as GitHub Pages).

## 10. Security
- None (anonymous access). HTTPS redirection enabled.

## 11. Persistence
- EF Core DbContext: `WorkoutJournalDbContext` uses PostgreSQL provider (`UseNpgsql`).
- Connection string from configuration key `DefaultConnection` (env var `CONNECTION_STRING` currently not wired into options though read into variable — future improvement).

## 12. Serialization / Conventions
- System.Text.Json defaults
- Enum values accepted as numeric or string (frontend sends numeric for TemplateFrequency)
- `ExerciseTemplateItem.Exercise` excluded via `[JsonIgnore]` and not model-bound `[BindNever]`.

## 13. Non-Functional Requirements
- Fast development iteration (no auth, permissive CORS)
- Predictable deterministic responses
- Deployable to platforms like Render with minimal config

## 14. Future Enhancements (Not Implemented Yet)
- Data Annotations (e.g., `[Required]`, `[Range]`)
- Pagination / filtering
- Authentication & authorization
- PATCH partial updates
- Concurrency handling (ETag / rowversion)
- Soft delete or referential integrity constraints for exercises in use

## 15. Acceptance Checklist
- [ ] CRUD operations succeed with expected status codes
- [ ] Template creation with Items using only ExerciseId succeeds
- [ ] Setting current template then fetching returns expanded items & exercises
- [ ] Swagger UI displays action summaries at root
- [ ] Frontend (static) can interact cross-origin without CORS issues

## 16. Example Payloads
### Create Exercise
```json
{
  "name": "Bench Press",
  "description": "Barbell flat bench press",
  "category": 1,
  "defaultSets": 3,
  "defaultReps": "8-12",
  "defaultRestSeconds": 120,
  "difficulty": 3
}
```

### Create Template
```json
{
  "name": "Push Day",
  "description": "Chest / Shoulders / Triceps",
  "frequency": 0,
  "items": [
    {
      "order": 1,
      "exerciseId": 5,
      "targetSets": 3,
      "targetReps": "8-12",
      "targetRestSeconds": 120,
      "notes": "Focus on bar speed"
    },
    {
      "order": 2,
      "exerciseId": 9,
      "targetSets": 3,
      "targetReps": "10",
      "targetRestSeconds": 90,
      "notes": "Controlled eccentric"
    }
  ]
}
```

## 17. Rationale Summary
Design favors simplicity for a lightweight admin tool: full replacement updates, open CORS, eager loading for convenience, minimal validation to reduce friction during iteration.

---
This README defines the authoritative specification for the current API implementation.