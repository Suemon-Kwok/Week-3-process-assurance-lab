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


## Defect Log

| Defect ID | Description | Severity | Status | Found In | Fixed In |
|---|---|---|---|---|---|
| DEF-001 | Several `[TestMethod]` tests in `AppointmentBookingServiceTests.cs` (e.g. `AppointmentRequest_WhenRequestedDateIsToday_ThrowsException`, `BookAppointment_WhenDoctorAtMaxDailyAppointments_ReturnsFailure`) were located outside the class and namespace closing braces, causing a compile error and blocking the whole test suite from building or running. | High | Fixed | Code review of `AppointmentBookingServiceTests.cs` before running tests | Moved the affected test methods back inside the `AppointmentBookingServiceTests` class body, unchanged in content, so the file compiles correctly |


## Process Assurance vs Product Assurance

| Area | Process Assurance | Product Assurance |
|---|---|---|
| Main focus | How the work is performed | Quality of the software product |
| Example in this project | Requirements review, coding standards, Git commits, test process | Validation logic, working booking feature, passing tests |
| Evidence | Review checklist, commits, test plan, CI results | Test results, defect reports, working prototype |
| Goal | Prevent quality problems | Detect and confirm product quality |

Process assurance and product assurance are both needed because they catch different kinds of risk. Product assurance (testing the booking and cancellation logic, running MSTest suites) tells us whether the *current* build works, but it only detects defects after they've already been written. Process assurance — requirement reviews, consistent commit history, a documented test plan — reduces the *rate* at which defects are introduced in the first place, and gives the clinic evidence that quality was built in deliberately rather than discovered by luck. A team with perfect test results but no process discipline is one bad sprint away from a regression; a team with good process but no product testing has no proof anything actually works. Together they give both prevention and detection.