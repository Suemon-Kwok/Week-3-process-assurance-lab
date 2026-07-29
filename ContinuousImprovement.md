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