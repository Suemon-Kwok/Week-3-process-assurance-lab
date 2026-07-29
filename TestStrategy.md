# Test Strategy

## 1. Purpose
This Test Strategy defines how the important quality risks in the Medical Appointment Booking System will generally be evaluated. It sets the stable, high-level testing direction for the project — it does not list day-to-day test cases; that level of detail belongs in the Test Plan for each feature (e.g. `TestPlan.md` for the cancellation feature).

**Quality objective:** An appointment must never be booked against a doctor slot that is unavailable, and a cancelled appointment must always release its slot back to the doctor's availability — reception staff and patients must be able to trust the slot count as accurate at all times.

## 2. Scope of Testing
- Doctor availability and slot management (`Doctor.AvailableSlots`, `ReleaseSlot()`)
- Patient and appointment validation rules (`Appointment` constructor guards)
- Booking an appointment (`BookAppointment`)
- Cancelling an appointment and releasing a slot (`CancelAppointment`, `Appointment.Cancel()`)
- Business rules such as preventing double-booking, double-cancellation, or cancelling a non-existent appointment

## 3. Out of Scope
- Third-party integrations not yet built (e.g. SMS/email notification, payment processing)
- UI/UX testing (no front-end exists yet in this version)
- Load/concurrency testing at production scale (see note below — this is a known limitation, not an assumption that concurrency is safe)
- Security penetration testing

**Assumption/limitation to record honestly:** like the course-enrolment example in the lecture, booking and slot updates in this system are currently two separate steps (check availability, then update the count). No concurrency/race-condition testing has been performed to confirm two simultaneous booking or cancellation requests can't corrupt the slot count. This is flagged as a residual risk in the Test Summary Report rather than silently assumed to be fine.

## 4. Test Levels
- **Unit testing** — individual classes (`Appointment`, `Doctor`, `Patient`) in isolation
- **Integration testing** — the booking service working together with `Doctor`/`Patient`/`Appointment`
- **System testing** — the full booking-and-cancellation workflow end-to-end
- **Regression testing** — re-running the full suite after every change, so a fix or new feature can't silently break existing behaviour

## 5. Test Types
- Unit testing (MSTest)
- Integration testing
- System testing
- Regression testing
- Usability testing (manual walkthrough of the workflow from a receptionist's perspective)
- Validation testing (confirming the system meets the clinic's stated requirements, e.g. REQ-CAN-01/02/03)

## 6. Test Environment
- Local development machine running .NET 10 (Visual Studio)
- MSTest test runner
- No external database required — in-memory objects are used for Doctor/Patient/Appointment during testing

## 7. Tools
- Visual Studio
- MSTest test framework
- Git / GitHub for version control and CI evidence (commit history is itself a form of process-assurance evidence)
- GitHub Copilot — used to help draft test cases and review logic, but every suggestion is reviewed, understood, and re-tested before being committed; Copilot output is never treated as evidence on its own

## 8. Defect Management Approach
Defects are logged in `docs/QualityGovernance.md` with a Defect ID, description, severity (Low/Medium/High), status (Open/Fixed), where found, and where/how fixed. This turns "we tested it" from a bare claim into traceable evidence — anyone can check what was found, when, and how it was resolved. No defect is closed without a passing regression test confirming the fix.

## 9. Entry Criteria
- Code compiles with no errors
- Relevant requirement IDs exist for the feature under test
- Test environment is set up and existing tests pass

## 10. Exit Criteria
- All planned test cases for the feature have been executed
- No open High severity defects
- All requirement IDs have at least one passing test case (traceability satisfied — see the Traceability Matrix in `TestPlan.md`)
- Test Summary Report has been completed, including any residual risks that were *not* covered (e.g. concurrency), rather than only reporting a pass percentage

## 11. Risks and Mitigation
| Risk | Mitigation |
|---|---|
| Slot count becomes inconsistent after cancellation | Dedicated test cases for slot release; guard against double-release |
| Cancelling a non-existent or already-cancelled appointment causes a crash or silent failure | Explicit exception-based validation with matching test cases |
| Two simultaneous cancellations (or a booking and cancellation) race on the same slot count | Not currently mitigated — recorded as a known residual risk rather than assumed safe, per the strategy's scope limitation above |
| Copilot-generated code or tests are used without review | Governance rule requiring all AI suggestions to be reviewed and tested before commit |
| Small team, limited time, tests skipped under pressure | Governance rule requiring passing tests before every commit; exit criteria block release regardless of time pressure |

## Roles and Responsibilities
| Role | Responsibility |
|---|---|
| Developer (you) | Writes code, developer/unit tests, and reviews own logic before commit |
| Reviewer / peer (if applicable) | Independent check that tests actually cover the stated requirements, not just that they pass |
| Release decision | For this student project, you act as your own release authority — but the principle from governance still applies: a release recommendation must be backed by evidence in the Test Summary Report, not just a feeling that "it works" |

## Strategy Review and Maintenance
This strategy is revisited whenever the product's risk profile changes materially — for example, if concurrent booking/cancellation, persistence, or authentication features are added in a later phase, the "Out of Scope" and "Risks and Mitigation" sections above must be updated rather than left stale.