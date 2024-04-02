﻿using System;
using System.Collections.Generic;

namespace brskweb.Model;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
