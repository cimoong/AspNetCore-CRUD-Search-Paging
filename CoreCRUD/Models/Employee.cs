using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUD.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage ="This field cannot empty.")]
        [Column(TypeName="nvarchar(250)")]
        public string FullName { get; set; }

        [DisplayName("Emp. Code")]
        [Column(TypeName = "varchar(10)")]
        public string Empcode { get; set; }

        [DisplayName("Position")]
        [Column(TypeName = "varchar(100)")]
        public string Position { get; set; }

        [DisplayName("Office Location")]
        [Column(TypeName = "varchar(100)")]
        public string OfficeLocation { get; set; }
    }
}
