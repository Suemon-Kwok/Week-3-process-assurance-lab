using System;

namespace ENSE707_AppointmentBooking
{
    public class Appointment
    {
        // A unique identifier for this specific appointment record.
        // Read-only after construction, same pattern as Doctor.Id and
        // Patient.Id - an appointment's identity shouldn't change once created.
        public string Id { get; }

        // The Doctor this appointment is booked with. Storing the whole
        // object (not just an ID) means Cancel logic can later call
        // Doctor.ReleaseSlot(...) directly without a lookup.
        public Doctor Doctor { get; }

        // The Patient this appointment is for.
        public Patient Patient { get; }

        // The date this appointment is booked for. Kept here (rather than
        // only on the original AppointmentRequest) because a request object
        // represents "what was asked for", while Appointment represents
        // "what was actually confirmed" - the two should be tracked
        // separately even though they'll usually match.
        public DateTime AppointmentDate { get; }

        // Tracks whether this appointment has been cancelled.
        // Private setter so only this class can change the state
        // (encapsulation) - no external code can flip this flag directly
        // without going through the Cancel() method and its guard below.
        public bool IsCancelled { get; private set; }

        public Appointment(string id, Doctor doctor, Patient patient, DateTime appointmentDate)
        {
            // Validation: an appointment must have a real ID. This mirrors
            // the same defensive pattern already used in Doctor and Patient.
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Appointment ID is required.");

            // Null-coalescing throw: if doctor is null, throw immediately
            // with a message naming the exact broken parameter, rather
            // than allowing an Appointment to exist with no doctor attached.
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

            // Same protection for Patient - an appointment must always
            // have a real patient attached.
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));

            // No extra validation needed on the date here - AppointmentRequest
            // already enforces "at least one day in advance" before an
            // Appointment is ever created from it.
            AppointmentDate = appointmentDate;

            // Every new appointment starts as active (not cancelled).
            IsCancelled = false;
        }

        // Cancellation rule: an appointment can only be cancelled once.
        // Calling Cancel() a second time throws, so the caller
        // (AppointmentBookingService.CancelAppointment) can never
        // accidentally release the same doctor slot twice for one appointment.
        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Appointment has already been cancelled.");

            IsCancelled = true;
        }
    }
}
