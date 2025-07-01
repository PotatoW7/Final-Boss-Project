namespace PraktikaSedem.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}