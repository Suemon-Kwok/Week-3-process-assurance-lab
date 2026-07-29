using ENSE707_AppointmentBooking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentBookingServiceTests
    {
        [TestMethod]
        public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            BookingResult result = service.BookAppointment(request);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            var result = service.BookAppointment(request);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            service.BookAppointment(request);

            Assert.AreEqual(1, doctor.AvailableSlots);
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            service.BookAppointment(request);

            Assert.AreEqual(0, doctor.AvailableSlots);
        }

        [TestMethod]
        public void Doctor_WhenIdIsEmpty_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new Doctor("", "Dr Mark", 2));
        }

        [TestMethod]
        public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new Doctor("D001", "Dr Mark", -1));
        }

        [TestMethod]
        public void Patient_WhenIdIsEmpty_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new Patient("", "Diana William"));
        }

        [TestMethod]
        public void Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName()
        {
            var patient = new Patient("P001", "Diana William", "Aroha");

            Assert.AreEqual("Aroha", patient.DisplayName);
        }

        [TestMethod]
        public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
        {
            var patient = new Patient("P001", "Diana William");

            Assert.AreEqual("Diana William", patient.DisplayName);
        }

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");

            Assert.ThrowsExactly<ArgumentException>(() =>
                new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1)));
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William", "Aroha");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            var result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "Appointment booked successfully");
            StringAssert.Contains(result.Message, "Aroha");
        }

        [TestMethod]
        public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            var result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "no available slots");
        }

        // --- The methods below were found outside the class/namespace
        // braces in the uploaded file (a compile error) and have been
        // moved back inside here, unchanged, as part of fixing that defect. ---

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsToday_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");

            Assert.ThrowsExactly<ArgumentException>(() =>
                new AppointmentRequest(patient, doctor, DateTime.Today));
        }

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsTomorrow_Succeeds()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");

            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));

            Assert.AreEqual(DateTime.Today.AddDays(1), request.RequestedDate);
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorAtMaxDailyAppointments_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 5, maxDailyAppointments: 1);
            var patient1 = new Patient("P001", "Diana William");
            var patient2 = new Patient("P002", "John Smith");
            var date = DateTime.Today.AddDays(1);
            var service = new AppointmentBookingService();

            service.BookAppointment(new AppointmentRequest(patient1, doctor, date));
            var result = service.BookAppointment(new AppointmentRequest(patient2, doctor, date));

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_MessageIncludesDoctorName()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            var result = service.BookAppointment(request);

            StringAssert.Contains(result.Message, "Dr Mark");
        }

        [TestMethod]
        public void BookAppointment_WhenPatientIdInvalid_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            // Note: kept as-is from the original file. This test doesn't
            // actually exercise an invalid patient ID (Patient's own
            // constructor already blocks empty IDs, so one can't be
            // constructed in the first place) - it's really just a
            // sanity check that a normal valid booking succeeds. Consider
            // renaming or removing this test in your Test Summary Report
            // reflection, since its name overstates what it verifies.
            var result = service.BookAppointment(request);

            Assert.IsTrue(result.Success); // sanity check: valid ID books fine
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_SlotCountRemainsUnchanged()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();

            service.BookAppointment(request);

            Assert.AreEqual(0, doctor.AvailableSlots);
        }
    }
}
