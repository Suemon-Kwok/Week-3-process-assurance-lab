## Quality Governance Rules

| Governance Area | Rule | Evidence |
|---|---|---|
| Requirements | Each new feature must have at least one requirement ID (e.g. REQ-CAN-01–03) | `TestPlan.md` requirements list |
| Testing | Each requirement must have at least one test case | Traceability matrix in `TestPlan.md` |
| Code quality | Code must pass all unit tests before commit | `dotnet test` output — 25/25 passing |
| GitHub | Each student must commit meaningful work regularly | Git commit history |
| AI use | Copilot suggestions must be reviewed and tested, not accepted blindly | Code comments explaining accepted/modified/rejected suggestions (e.g. `BookingResult.cs`, `Appointment.cs`) |
| Defects | Defects must be recorded with status and severity | Defect log (below) |
| Release | A feature can only be released if exit criteria in the Test Plan are met | `TestSummaryReport.md` |

These rules matter because they turn "we tested it" into something checkable. A rule with no attached evidence is just a good intention — for example, "code must pass all unit tests before commit" only means something because there's an actual `dotnet test` run (25 passed, 0 failed) that a reviewer could go and look at, not just a claim that testing happened. Tying each governance area to a specific, inspectable artefact (a test plan, a commit log, a defect table) is what lets someone outside the project — the clinic, a reviewer, a future teammate — verify that quality was actually built in, rather than taking the team's word for it.