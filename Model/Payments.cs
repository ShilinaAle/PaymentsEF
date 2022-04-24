using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentsEF.Model
{
    public partial class Payments
    {
        [Key]
        public int OrderId { get; set; }
        public int ArrivalId { get; set; }
        public decimal? Amount { get; set; }

        public virtual Arrivals Arrival { get; set; }
        public virtual Orders Order { get; set; }
    }
}
