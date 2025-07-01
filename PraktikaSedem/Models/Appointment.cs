namespace PraktikaSedem.Models;

using System;
using System.ComponentModel.DataAnnotations;
public class Appointment
{
    public int AppointmentId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Status { get; set; }

    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }
}
