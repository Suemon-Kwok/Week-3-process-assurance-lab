namespace ENSE707_AppointmentBooking
{
    public class BookingResult
    {
        // Read-only after construction - a booking result represents a
        // single outcome that happened at one point in time. Nothing
        // should be able to change a result after it's been created and
        // returned to the caller.
        public bool Success { get; }

        // The human-readable explanation of what happened and why. This
        // exists specifically to fix the original design flaw noted in
        // the lab: the old BookAppointment() returned a plain bool, so
        // callers knew booking failed but had NO idea why (no slots?
        // invalid request? something else?). Bundling a Message alongside
        // Success means the caller always gets both the outcome AND the
        // reasoning in one object.
        public string Message { get; }

        // The constructor takes both pieces of information together so
        // it's impossible to create a BookingResult that's missing an
        // explanation - Success and Message are always set as a pair.
        public BookingResult(bool success, string message)
        {
            Success = success;
            Message = message;

            // Note: unlike Doctor/Patient, there's no validation here
            // rejecting an empty message. That's a deliberate simplification
            // for this lab - in a production system you might still want
            // to guard against a null/blank message so failures are never
            // silently unexplained.
        }
    }
}