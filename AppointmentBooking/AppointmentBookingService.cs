using System;

namespace ENSE707_AppointmentBooking
{
    public class AppointmentBookingService
    {
        public BookingResult BookAppointment(AppointmentRequest request)
        {
            if (request == null)
                return new BookingResult(false, "Appointment request is missing. Please provide patient, doctor, and date details.");

            // Rule 3: explicit check for a valid patient ID at the point
            // of booking, not just relying on Patient's constructor -
            // makes the business rule visible here, and gives a specific,
            // actionable message rather than a generic failure.
            if (string.IsNullOrWhiteSpace(request.Patient.Id))
                return new BookingResult(false, "Appointment cannot be booked because the patient ID is invalid. Please provide a valid patient ID.");

            if (!request.Doctor.HasAvailableSlot())
            {
                return new BookingResult(
                    false,
                    $"Appointment cannot be booked because {request.Doctor.FullName} has no available slots. Please choose a different doctor or contact reception.");
            }

            // Rule 2: reject if the doctor is already fully booked on
            // the requested date, even if they still have total slots
            // remaining overall.
            if (!request.Doctor.HasCapacityOnDate(request.RequestedDate))
            {
                return new BookingResult(
                    false,
                    $"Appointment cannot be booked because {request.Doctor.FullName} already has the maximum number of appointments on {request.RequestedDate:dd MMM yyyy}. Please choose a different date.");
            }

            request.Doctor.ReserveSlot(request.RequestedDate);

            // NEW (Step 7): a successful booking now creates a real
            // Appointment object representing the confirmed booking.
            // Guid.NewGuid() generates a globally unique string so every
            // Appointment gets a distinct Id without the service needing
            // to track a counter or check a database.
            var appointment = new Appointment(
                Guid.NewGuid().ToString(),
                request.Doctor,
                request.Patient,
                request.RequestedDate);

            // Rule 4: every message - success or failure - states WHAT
            // happened, WHY (on failure), and WHAT the patient can do
            // next ("choose a different date/doctor") rather than just
            // stating an outcome.
            // The Appointment is now passed as the 3rd argument so callers
            // (like the future cancellation flow) can act on it directly
            // instead of having to reconstruct it from the original request.
            return new BookingResult(
                true,
                $"Appointment booked successfully for {request.Patient.DisplayName} with {request.Doctor.FullName} on {request.RequestedDate:dd MMM yyyy}.",
                appointment);
        }

        // NEW (Step 7): cancels an existing appointment and releases its
        // doctor slot. Returns a BookingResult (rather than void) so this
        // method reports outcomes the same consistent way BookAppointment
        // does, instead of the caller having to catch exceptions to know
        // whether cancellation actually succeeded.
        public BookingResult CancelAppointment(Appointment appointment)
        {
            // Guard against a null appointment being passed in - this is
            // the service-level check backing REQ-CAN-03 ("cannot cancel
            // an appointment that does not exist").
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            // Delegate the cancellation rule itself to Appointment.Cancel(),
            // which already guards against double-cancellation by throwing
            // InvalidOperationException if IsCancelled is already true.
            appointment.Cancel();

            // Only reached if Cancel() succeeded (didn't throw) - release
            // the doctor's slot for the date this appointment was booked on.
            appointment.Doctor.ReleaseSlot(appointment.AppointmentDate);

            return new BookingResult(
                true,
                $"Appointment for {appointment.Patient.DisplayName} with {appointment.Doctor.FullName} on {appointment.AppointmentDate:dd MMM yyyy} has been cancelled.",
                appointment);
        }
    }
}
