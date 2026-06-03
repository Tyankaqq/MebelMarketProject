using System;
using System.Collections.Generic;

namespace ToysMarkerProject.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int UnitId { get; set; }

    public short Cost { get; set; }

    public int ProviderId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public byte Discount { get; set; }

    public byte Count { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;

    public string? ImageSource
    {
        get
        {
            if (Photo == null) return null;
            return "/Resources/Image/" + Photo;
        }
    }

}
