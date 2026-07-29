using System;

namespace ENSE707_AppointmentBooking
{
    // This class was referenced by AppointmentBookingService and its tests
    // (result.Success, result.Message) but was missing from the project -
    // recreated here so the solution compiles again.
    public class BookingResult
    {
        // Whether the booking (or cancellation) attempt succeeded.
        // Read-only after construction - a result shouldn't be able to
        // change its own outcome after the fact.
        public bool Success { get; }

        // A human-readable explanation of what happened and, on failure,
        // why - matches the existing messages already written in
        // AppointmentBookingService (e.g. "no available slots").
        public string Message { get; }

        // NEW for Step 7: carries the Appointment that was created when
        // a booking succeeds, so callers don't have to separately
        // reconstruct it. Nullable (defaults to null) so every existing
        // call site like "new BookingResult(false, \"...\")" still
        // compiles unchanged - we only pass an Appointment where one
        // actually exists (a successful booking).
        public Appointment? Appointment { get; }

        // appointment defaults to null so this constructor is backward
        // compatible with every existing 2-argument call in the codebase
        // (all the failure-path returns), while still allowing a 3rd
        // argument to be passed where a booking actually succeeds.
        public BookingResult(bool success, string message, Appointment? appointment = null)
        {
            // No validation guard on 'message' being empty here, because
            // failure/success messages are always supplied as literal
            // strings by the service itself, not by external/untrusted
            // input - keeping this constructor simple matches how it's
            // already used throughout AppointmentBookingService.
            Success = success;
            Message = message;
            Appointment = appointment;
        }
    }
}
