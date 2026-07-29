# Test Plan

## Feature Under Test
Appointment Cancellation — reception staff can cancel an existing appointment, which releases the doctor's slot back into availability.

## Test Objective
To verify that a valid appointment can be cancelled, that cancelling it correctly releases the doctor's slot, and that invalid cancellation attempts (null or already-cancelled appointments) are rejected safely with a clear exception rather than corrupting state.

## Requirements to be Tested
- REQ-CAN-01: The system shall allow an existing appointment to be cancelled.
- REQ-CAN-02: When an appointment is cancelled, the doctor's available slot count shall increase by one.
- REQ-CAN-03: The system shall not allow cancellation of an appointment that does not exist.

## Test Items
- `Appointment.Cancel()`
- `BookingService.CancelAppointment(Appointment appointment)`
- `Doctor.ReleaseSlot()`

## Test Approach
Unit tests using MSTest, using in-memory `Doctor`, `Patient`, and `Appointment` objects (no external dependencies). Each test targets one requirement directly, and test method names follow the pattern `MethodUnderTest_Scenario_ExpectedResult` for traceability.

## Test Data
- One valid `Doctor` with a known starting `AvailableSlots` count (e.g. 3)
- One valid `Patient`
- One valid `Appointment` built from the above, with a valid `Id` and future `AppointmentDate`
- `null` used as an invalid appointment input

## Responsibilities
- Developer/tester (you): write and run the cancellation tests, record results
- Reviewer (pair/peer, if applicable): review test coverage against the requirement list

## Schedule
| Task | When |
|---|---|
| Implement `Appointment` class and cancellation logic | Before test writing |
| Write cancellation test cases | Same lab session |
| Run full regression suite | Immediately after |
| Record results in Test Summary Report | After all tests pass |

## Pass and Fail Criteria
- **Pass**: All 5 cancellation-related tests pass, plus all pre-existing booking tests still pass (no regressions).
- **Fail**: Any test fails, or an existing passing test is broken by the new feature (a defect should be logged for each failure).

## Traceability Matrix

| Requirement | Test Case |
|---|---|
| REQ-CAN-01 | `CancelAppointment_ExistingAppointment_MarksAppointmentAsCancelled` |
| REQ-CAN-02 | `CancelAppointment_ExistingAppointment_ReleasesDoctorSlot` |
| REQ-CAN-03 | `CancelAppointment_NullAppointment_ThrowsException` |
| (supporting) | `CancelAppointment_AlreadyCancelledAppointment_ThrowsException` |
| (supporting) | `BookAppointment_Success_ReturnsAppointmentWithCorrectDetails` |

## Entry and Exit Criteria
**Entry:** Cancellation requirements (REQ-CAN-01/02/03) agreed; `Appointment` and `CancelAppointment` implemented; existing booking tests already passing.
**Exit:** All 5 planned test cases executed and passing; no open High severity defects; traceability matrix above fully satisfied (every requirement has a passing test); limitations below are recorded, not silently ignored.

## Risks
- Slot release logic could double-increment if called twice on the same appointment — mitigated by the `IsCancelled` guard in `Appointment.Cancel()`.
- If `Doctor`/`Patient` constructors differ from what's assumed here, test setup code will need adjusting to match your actual Week 2 classes.
- **Residual risk carried from the Test Strategy:** this plan only covers *sequential* cancellation scenarios. It does not test two simultaneous cancellation requests against the same appointment, or a booking and a cancellation racing on the same doctor slot. This plan's "pass" result should not be read as proof the slot count is safe under concurrent access — that gap is intentionally reported, not hidden, in the Test Summary Report.