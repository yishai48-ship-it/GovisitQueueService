# Govisit Queue Management Service

Web API (.NET 8) לניהול תורים, CQRS עם MediatR מול MongoDB.

## מבנה

```
Controllers/    -> AppointmentsController, HealthController
CQRS/Commands/  -> Create / Update / Delete  (Command + Handler)
CQRS/Queries/   -> GetAll / GetById + AppointmentMapper
Models/         -> Appointment, AppointmentStatuses
DTOs/           -> DTOs + ValidationPatterns (regex משותפים)
Data/           -> MongoDbSettings, MongoDbContext, DataSeeder
Program.cs      -> DI, Swagger, MongoDB מוטמע, seed ב-Development
```

## הרצה

לא נדרשת התקנת MongoDB — `Mongo2Go` מרים `mongod` אמיתי בעליית האפליקציה:

```bash
dotnet restore
dotnet run
```

Swagger: `http://localhost:5080/swagger`

## Endpoints

| Method | Route                          | תיאור               |
|--------|--------------------------------|---------------------|
| GET    | /api/health                    | בדיקת חיות (200 OK) |
| POST   | /api/appointments/create       | קביעת תור           |
| POST   | /api/appointments/all          | כל התורים           |
| POST   | /api/appointments/{id}/details | תור בודד            |
| POST   | /api/appointments/{id}/update  | עדכון תור           |
| POST   | /api/appointments/{id}/delete  | מחיקת תור           |

## ולידציה

`DTOs/ValidationPatterns.cs` מרכז את כל ה-regex:

| שדה           | כלל                                                |
|----------------|-----------------------------------------------------|
| CustomerName   | אותיות עברית/לטינית, רווח, גרש, מקף. עד 5 מילים    |
| CustomerPhone  | מספר ישראלי: `05X` או קידומת 2/3/4/8/9, מקף אופציונלי |
| ServiceType    | אותיות וספרות, עד 10 מילים                          |
| Status         | `Scheduled` \| `Completed` \| `Cancelled`           |
| id (route)     | ObjectId — 24 תווי hex                              |

ה-regex מעוגנים ב-`\A...\z` ולא `^...$`, כי ב-.NET הסימן `$` מתאים גם לפני `\n`
בסוף מחרוזת — כלומר `"Scheduled\n"` היה עובר ולידציה.

ולידציית ה-id נאכפת פעמיים: `{id:length(24)}` ב-route (מסנן לפני ה-controller)
ו-`[RegularExpression]` על הפרמטר. `[ApiController]` מחזיר 400 אוטומטית.

## נתוני בדיקה (Development בלבד)

`appsettings.Development.json` מכיל 3 תורים תחת `SeedData:Appointments`.
`DataSeeder` מזריק אותם רק אם האוסף ריק, ונקרא מ-`Program.cs` בתוך
`if (app.Environment.IsDevelopment())` — כך שב-Production זה לא רץ כלל.

## מעבר ל-MongoDB חיצוני

ב-`Program.cs` יש להסיר את `MongoDbRunner` ולקשור ישירות:

```csharp
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
```

הכתובת תילקח מ-`appsettings.json`.

## גרסאות

`Mongo2Go 4.x` מחייב `MongoDB.Driver 3.x` — דרייבר 2.28 עבר ל-strong-named
assemblies, ולכן שילוב `Mongo2Go 3.x` עם דרייבר 2.28 נשבר.

## GitHub

```bash
git init
git add .
git commit -m "Govisit queue management service - CQRS + MongoDB"
git branch -M main
git remote add origin <URL>
git push -u origin main
```
