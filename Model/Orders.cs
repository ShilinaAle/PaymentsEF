using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentsEF.Model
{
    public partial class Orders
    {
        public int Idorder { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Payment { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
