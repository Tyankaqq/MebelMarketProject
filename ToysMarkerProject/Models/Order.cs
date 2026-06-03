using System;
using System.Collections.Generic;

namespace ToysMarkerProject.Models;

public partial class Order
{
    public int Id { get; set; }

    public string Article { get; set; } = null!;

    public DateOnly DateOrder { get; set; }

    public DateOnly DateDelivery { get; set; }

    public int AddressId { get; set; }

    public int UserId { get; set; }

    public short Code { get; set; }

    public string Status { get; set; } = null!;

    public virtual Pvz Address { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
