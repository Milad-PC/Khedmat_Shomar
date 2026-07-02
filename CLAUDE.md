# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository overview

"Khedmat Shomar" (خدمت شمار) is a small Persian-language web app that calculates how much mandatory military service a soldier has left, given their start date (Persian/Shamsi calendar) and various deductions/additions (کسری پدر, کسری بسیج, تاهل, فرزند, غیبت, etc.). It then shows a live countdown to the discharge date.

The repo contains **two unrelated ASP.NET projects**:

- `Khedmat Shomar/Khedmat Shomar/` — the **old app**, ASP.NET MVC 5 on .NET Framework 4.8, classic `packages.config`/`Web.config` style. This is **frozen/reference-only**: the user does not care about it and it should not be modified. It's useful only as a spec of the existing calculation logic (see below) to reference when reimplementing things in the new app.
- `Khedmat Shomar/KhedmatShomar/` — the **new app** and the actual focus of development going forward. It's a freshly scaffolded ASP.NET Core (net10.0) project (currently just the default `dotnet new mvc` template) that the user intends to build out as a substantially better version of the old calculator — not a 1:1 port. It is not part of the `.sln` and has no shared code with the old project.

Default to working in `KhedmatShomar/` unless the user explicitly asks about the old app. Do not propose changes to `Khedmat Shomar/Khedmat Shomar/`.

## Commands

### New app — `Khedmat Shomar/KhedmatShomar/` (ASP.NET Core, net10.0)

This is the project to build. SDK-style, usable with the `dotnet` CLI:

```powershell
dotnet build "Khedmat Shomar/KhedmatShomar/KhedmatShomar.csproj"
dotnet run --project "Khedmat Shomar/KhedmatShomar/KhedmatShomar.csproj"
```

No tests exist yet.

### Old app — `Khedmat Shomar/Khedmat Shomar/` (.NET Framework, MVC 5) — reference only, do not modify

Classic ASP.NET (System.Web) project using NuGet `packages.config`, not SDK-style — the `dotnet` CLI cannot build it (would need MSBuild/Visual Studio/IIS Express). Not expected to be built or run during normal work; kept only so its logic can be read as a spec.

## Old app's calculation logic (reference for reimplementing in the new app)

The old app is a single-page calculator: enter a Shamsi (Persian calendar) service start date plus various deductions/additions (کسری پدر, کسری بسیج, تاهل, فرزند, غیبت, etc.), get back a countdown to discharge. When rebuilding this in the new app, the logic below is the spec to match (and improve on) — the old code itself should not be touched.

All of it lives in `Khedmat Shomar/Khedmat Shomar/Controllers/HomeController.cs`, driven by a single `Soldier` model (`Models/Soldier.cs`) and one Razor form (`Views/Home/Index.cshtml`):

- `Index()` — checks for a `KhedmatShomar` cookie storing a previously computed finish date; if present, redirects straight to `Counter`. Otherwise shows the input form.
- `ShowFinish(Soldier input)` — the calculation core. Takes the Shamsi (Persian) start date (`Sal`/`Mah`/`Roz`) via `System.Globalization.PersianCalendar`, adds the base 2-year service term, then applies a chain of independent adjustments in sequence: `RozKasri`/`MahKasri` (father-deduction), `RozBasij`/`MahBasij` (Basij deduction), marital status (`Mojarad == "2"` → -2 months) and per-child deduction (`Bache` → -3 months each), `RozFarar` (AWOL days, added back as +3 months per unit) and `RozEzaf` (misc extra days, same +3 months per unit treatment). Each adjustment is wrapped in its own try/catch that silently swallows parse failures — note this when porting: the old app just skips bad input silently rather than validating, and the new app doesn't have to repeat that.
- `Counter(DateTime FinishDate)` — sets the `KhedmatShomar` cookie (90-day expiry) if not already set, computes the remaining days, and renders the countdown view.
- `Helper.ToShamsi(DateTime)` (`Assisst/Helper.cs`) — extension method for formatting a `DateTime` as a Shamsi date string; used for display purposes.

Standard MVC5 plumbing lives in `App_Start/` (`RouteConfig`, `FilterConfig`, `BundleConfig`) and `Global.asax.cs` — nothing project-specific there beyond the default single `{controller}/{action}/{id}` route.

Note: `packages.config` and the `.csproj` still reference `EntityFramework`, but there is currently no `DbContext` or connection string in the project (removed in the "connection string"/"del visits" commits) — the app is presently stateless aside from the cookie. Don't assume a database is wired up.
