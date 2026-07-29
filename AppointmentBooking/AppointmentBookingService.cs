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

            // Rule 4: every message - success or failure - states WHAT
            // happened, WHY (on failure), and WHAT the patient can do
            // next ("choose a different date/doctor") rather than just
            // stating an outcome.
            return new BookingResult(
                true,
                $"Appointment booked successfully for {request.Patient.DisplayName} with {request.Doctor.FullName} on {request.RequestedDate:dd MMM yyyy}.");
        }
    }
}