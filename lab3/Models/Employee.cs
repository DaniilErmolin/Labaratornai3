using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Fio { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public int Age { get; set; }

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
