using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Rihal_Challenge.Data
{
    [Table("countries", Schema = "public")]
    public class CountriesClass
    {
        [Key]

        [Column(name: "id")]
        public int Id { get; set; }

        [Column(name: "name")]
        public string Name { get; set; }

        [Column(name: "created_date")]
        public Nullable<DateTime> CreatedDate { get; set; }

        [Column(name: "modified_date")]
        public Nullable<DateTime> ModifiedDate { get; set; }
        [NotMapped]
        public int Count { get; set; }

    }
}
