## Continuous Improvement

### What Worked Well
Using AI to investigate issues rather than just generate code was the most useful part of this
week. When Test Explorer showed all 25 tests stuck on "Not Run" with no visible error, AI-assisted
troubleshooting worked through the problem systematically — checking the build output, running
`dotnet test` directly from the terminal to bypass the Visual Studio UI, and locating the actual
solution folder when the working directory turned out to be wrong. This is a different use of AI
than Week 2, where the workflow was closer to "copy the base code, then ask AI to improve it."
This week's AI use was diagnostic rather than generative — using it to narrow down *why* something
was failing, not just to produce new code.

### What Did Not Work Well
Downloading the previous lab (Week 2) from GitHub onto a different laptop to continue working
introduced a cluster of problems that weren't present in the original environment: a nested,
nearly-duplicate folder structure from unzipping (`ENSE707-Lab-2-main\ENSE707-Lab-2-main\...`),
loose backup copies of `.cs` files sitting outside the actual project folder, and — most
disruptively — a stale Visual Studio Test Explorer state that showed all tests as "Not Run" even
though the project built successfully with zero errors.

### Root Cause of One Issue
The root cause was a missing clean build after moving the project to a new machine. Test Explorer
was working from cached `.vs` state left over from the transfer, so it never actually attempted to
launch a fresh test host — it just sat at "Not Run" with no error message, which made the problem
look far worse than it was.

### Improvement Action
Once AI-assisted investigation identified the stale `.vs` cache as the likely cause, the fix was
straightforward: close Visual Studio, delete the `.vs` folder, run `dotnet clean`, and reopen the
solution so Test Explorer rebuilt its state from scratch instead of reusing the old cache.

### How We Will Check the Improvement
Reopened the solution after the clean build and ran all tests through Test Explorer (not just the
terminal) to confirm the fix worked end-to-end in the actual tool the lab expects to be used —
Test Explorer reported **25 Tests, 25 Passed, 0 Failed** in 214ms, matching the terminal's
`dotnet test` result exactly.

### Quality Culture Reflection
This week's process matched what good quality culture is supposed to look like in practice: write
the code, run it to check, hit a real problem (both the DEF-001 compile error and the stale Test
Explorer state), investigate rather than guess or ignore it, and only move forward once there was
actual evidence — a passing test run — that the fix worked. Early testing caught the DEF-001
compile error before it could hide inside a "looks done" submission; without running the full test
suite, the misplaced test methods would never have surfaced. Moving between machines also showed
why regular commits matter beyond just backup: they're what let a stale environment be diagnosed
and rebuilt from a known-good state rather than guessed at. The main thing to carry into the next
lab phase is to always do a clean build/test run *immediately* after moving or re-cloning a
project, before trusting whatever the IDE currently shows.

## Agile and DevOps Quality Practices for This Project

| Practice | How It Could Be Used in This Project |
|---|---|
| Sprint planning | Break the clinic's feature requests into small increments — e.g. Week 2's booking validation as one sprint, Week 3's cancellation feature as the next — rather than building everything at once |
| Daily stand-up | A short check-in to flag blockers early, such as the stale Test Explorer state this week, so troubleshooting starts the same day instead of being discovered right before a deadline |
| Definition of Done | A feature (e.g. `CancelAppointment`) is only "done" once it's coded, the doctor slot release logic is reviewed, all cancellation tests pass, and the Test Plan's traceability matrix is filled in — not just once the code compiles |
| Continuous Integration | A CI pipeline would have caught the DEF-001 compile error (test methods outside the class braces) automatically on every push, instead of it only surfacing when tests were run manually |
| Regression testing | Re-running the full 25-test suite after adding the cancellation feature confirmed the existing Week 2 booking tests still passed alongside the new cancellation tests — no regressions introduced |
| Retrospective | This Continuous Improvement section itself is a retrospective — reviewing what caused the Test Explorer issue and what to check first next time a project moves machines |

Even at this small scale, these practices map directly onto real problems from this lab: CI would
have caught DEF-001 before it needed manual debugging, and regression testing is what gave
confidence that fixing the cancellation feature didn't quietly break booking.

## Step 14 — AI QA Process Suggestions

### Prompt Used
"Review the CancelAppointment method and Appointment.Cancel() for reliability,
maintainability, and testability issues."

### Useful Suggestion
Flagged that `Appointment.Cancel()` throwing `InvalidOperationException` on a
double-cancel is a good defensive pattern, but pointed out that
`AppointmentBookingService.CancelAppointment()` calls `appointment.Doctor.ReleaseSlot(...)`
*after* `Cancel()` succeeds — meaning if `ReleaseSlot()` ever threw partway through
(e.g. a future change adds validation to it), the appointment would be left marked
cancelled while the doctor's slot was never released, silently corrupting state.
This was accepted as a legitimate maintainability risk worth noting, even though the
current `ReleaseSlot()` implementation can't actually throw — it's a latent risk for
future changes, not a bug today.

### Suggestion Modified
Copilot initially suggested adding a `Doctor.ReleaseSlot(DateTime date)` overload
by generating a brand-new implementation from scratch. It was modified rather than
accepted directly: the existing `GetAppointmentCountForDate` helper was reused instead
of duplicating the dictionary lookup logic, and `Math.Max(0, currentCount - 1)` was
added to guard against the per-day count ever going negative if `ReleaseSlot` were
mistakenly called twice — a safeguard Copilot's version didn't include.

### Suggestion Rejected
Copilot suggested making `CancelAppointment` silently return a failed `BookingResult`
(success = false) instead of throwing when `appointment` is null, to "match the
BookAppointment pattern." This was rejected: REQ-CAN-03 and the existing test
(`CancelAppointment_NullAppointment_ThrowsException`) specifically expect an
`ArgumentNullException`, and a null appointment reference is a programming error
(caller bug) rather than a valid business-rule rejection like "no available slots" —
conflating the two would hide bugs behind a soft failure message instead of surfacing
them immediately.

### Why Human Judgement Was Required
None of these three decisions could be made from code correctness alone — they
required knowing the actual requirements (REQ-CAN-01–03), the difference between a
caller bug and a business rule failure, and which risks are worth defending against
now versus documenting as a known limitation. AI could generate plausible-looking
code for any of the three options; only checking each suggestion against the
Test Plan's traceability matrix and the existing passing tests confirmed which
one was actually correct for this project.