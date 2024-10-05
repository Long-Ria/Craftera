using System;
using System.Collections.Generic;

namespace Craftera_MVC1.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? Status { get; set; }
        public decimal? TotalMoney { get; set; }
        public int? PaymentId { get; set; }

        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
    }
}
