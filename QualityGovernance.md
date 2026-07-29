## Defect Log

| Defect ID | Description | Severity | Status | Found In | Fixed In |
|---|---|---|---|---|---|
| DEF-001 | Several `[TestMethod]` tests in `AppointmentBookingServiceTests.cs` (e.g. `AppointmentRequest_WhenRequestedDateIsToday_ThrowsException`, `BookAppointment_WhenDoctorAtMaxDailyAppointments_ReturnsFailure`) were located outside the class and namespace closing braces, causing a compile error and blocking the whole test suite from building or running. | High | Fixed | Code review of `AppointmentBookingServiceTests.cs` before running tests | Moved the affected test methods back inside the `AppointmentBookingServiceTests` class body, unchanged in content, so the file compiles correctly |