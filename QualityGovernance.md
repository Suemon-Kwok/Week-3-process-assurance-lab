## Process Assurance vs Product Assurance

| Area | Process Assurance | Product Assurance |
|---|---|---|
| Main focus | How the work is performed | Quality of the software product |
| Example in this project | Requirements review, coding standards, Git commits, test process | Validation logic, working booking feature, passing tests |
| Evidence | Review checklist, commits, test plan, CI results | Test results, defect reports, working prototype |
| Goal | Prevent quality problems | Detect and confirm product quality |

Process assurance and product assurance are both needed because they catch different kinds of risk. Product assurance (testing the booking and cancellation logic, running MSTest suites) tells us whether the *current* build works, but it only detects defects after they've already been written. Process assurance — requirement reviews, consistent commit history, a documented test plan — reduces the *rate* at which defects are introduced in the first place, and gives the clinic evidence that quality was built in deliberately rather than discovered by luck. A team with perfect test results but no process discipline is one bad sprint away from a regression; a team with good process but no product testing has no proof anything actually works. Together they give both prevention and detection.