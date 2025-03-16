using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

/// <summary>
/// 紀錄贊助
/// </summary>
public partial class Donation
{
    public long DonationId { get; set; }

    public long SponsorId { get; set; }

    public long ReceiverId { get; set; }

    /// <summary>
    /// 串接的廠商  [ecpay]綠界
    /// </summary>
    public string PaymentGateway { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public virtual User DonationNavigation { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;
}
