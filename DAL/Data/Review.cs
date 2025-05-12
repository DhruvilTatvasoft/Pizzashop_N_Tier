using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class Review
{
    public int Id { get; set; }

    public int FoodReview { get; set; }

    public int ServiceReview { get; set; }

    public int AmbienceReview { get; set; }

    public string? Description { get; set; }

    public virtual Customer IdNavigation { get; set; } = null!;
}
