using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Rihal_Challenge.Data
{
    [Table("students", Schema = "public")]
    public class StudentsClass
    {
        [Key]
    
        [Column(name:"id")]
        public int Id { get; set; }

        [Column(name: "class_id")]
        public int ClassId { get; set; }

        [Column(name: "country_id")]
        public int CountryId { get; set; }

        [Column(name: "name")]
        public string Name { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string ClassName { get; set; }

     
        [Column(name: "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column(name: "created_date")]
        public Nullable<DateTime> CreatedDate { get; set; }

        [Column(name: "modified_date")]
        public Nullable<DateTime> ModifiedDate { get; set; }



    }
}
