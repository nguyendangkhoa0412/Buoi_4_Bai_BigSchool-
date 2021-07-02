using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buoi_4_Bai_BigSchool__.Models
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (DateTime)value > DateTime.Now;
        }
        
    }
}