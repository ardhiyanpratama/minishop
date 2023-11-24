using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Helper
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string? CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
