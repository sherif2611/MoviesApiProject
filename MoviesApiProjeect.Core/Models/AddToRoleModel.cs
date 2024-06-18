using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesApiProjeect.Core.Models
{
    public class AddToRoleModel
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}