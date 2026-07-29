using System;
using System.Collections.Generic;

namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; }
        public string FullName { get; }
        public int AvailableSlots { get; private set; }

        // New: caps how many appointments this doctor can take on any
        // single calendar day, independent of total AvailableSlots.
        public int MaxDailyAppointments { get; }

        // Tracks how many appointments are already booked per date.
        // Keyed by date-only (no time component) so "23 July 9am" and
        // "23 July 3pm" count toward the same day's limit.
        private readonly Dictionary<DateTime, int> _dailyAppointmentCounts = new();

        // maxDailyAppointments defaults to 5 so existing calls to the
        // constructor (from your earlier tests) keep compiling without
        // needing a 4th argument everywhere.
        public Doctor(string id, string fullName, int availableSlots, int maxDailyAppointments = 5)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required.");

            if (availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative.");

            if (maxDailyAppointments <= 0)
                throw new ArgumentException("Maximum daily appointments must be greater than zero.");

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
            MaxDailyAppointments = maxDailyAppointments;
        }

        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        // Looks up how many appointments are already booked on a given
        // date. Returns 0 if the date has no entries yet, rather than
        // throwing - a date with no bookings is a normal, valid state.
        public int GetAppointmentCountForDate(DateTime date)
        {
            return _dailyAppointmentCounts.TryGetValue(date.Date, out var count) ? count : 0;
        }

        // Lets the service check capacity BEFORE attempting to reserve,
        // so it can return a clean failure message instead of relying
        // on catching an exception.
        public bool HasCapacityOnDate(DateTime date)
        {
            return GetAppointmentCountForDate(date) < MaxDailyAppointments;
        }

        // ReserveSlot now takes the requested date, since "is there
        // capacity" is no longer a single global number - it depends
        // on which day is being booked.
        public void ReserveSlot(DateTime date)
        {
            if (!HasAvailableSlot())
                throw new InvalidOperationException("No appointment slots are available.");

            if (!HasCapacityOnDate(date))
                throw new InvalidOperationException("Doctor has reached the maximum number of appointments for this date.");

            AvailableSlots--;

            var dateKey = date.Date;
            _dailyAppointmentCounts[dateKey] = GetAppointmentCountForDate(dateKey) + 1;
        }
    }
}