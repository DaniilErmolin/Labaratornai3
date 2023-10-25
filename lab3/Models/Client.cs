using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Fio { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Sex { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Series { get; set; } = null!;

    public long Number { get; set; }

    public long Discount { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
