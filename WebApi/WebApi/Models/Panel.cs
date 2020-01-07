using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Square
    {
        [Required]
        public int State { get; set; }
    }

    public class Panel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IList<Square> Squares { get; set; }
    }
}