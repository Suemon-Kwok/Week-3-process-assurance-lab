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