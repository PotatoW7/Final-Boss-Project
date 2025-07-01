namespace PraktikaSedem.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Doctor
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Specialization { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}