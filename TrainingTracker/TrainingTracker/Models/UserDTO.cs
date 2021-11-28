using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int IsMasterAdmin { get; set; }
        public int IsDelegate { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeNumber { get; set; }
        public string Password { get; set; }
        public string EncriptedId { get; set; }
        public string AssignUserRole { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Designation { get; set; }
        public string Profile{ get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> SessionRole { get; set; }
    }
}