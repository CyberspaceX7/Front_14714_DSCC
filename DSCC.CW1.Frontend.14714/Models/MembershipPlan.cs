using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DSCC.CW1.Frontend._14714.Models
{
    public class MembershipPlan
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PlanName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }
    }
}