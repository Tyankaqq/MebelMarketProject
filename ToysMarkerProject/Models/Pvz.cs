using System;
using System.Collections.Generic;

namespace ToysMarkerProject.Models;

public partial class Pvz
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
