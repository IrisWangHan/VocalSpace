using System;
using System.Collections.Generic;

namespace VocalSpace.newModels;

/// <summary>
/// 紀錄贊助詳細資訊
/// </summary>
public partial class Ecpay
{
    public long EcpayId { get; set; }

    /// <summary>
    /// 對應綠界MerchantMemberID
    /// </summary>
    public long DonationId { get; set; }

    /// <summary>
    /// 回傳的收款用戶註冊綠界時的ID(綠界辨識用)
    /// </summary>
    public string MerchantId { get; set; } = null!;

    /// <summary>
    /// 回傳的交易編號,收款者可查詢交易狀態
    /// </summary>
    public string TradeNo { get; set; } = null!;

    /// <summary>
    /// 回傳的交易狀態碼。0 未付款,1成功
    /// </summary>
    public int RtnCode { get; set; }

    /// <summary>
    /// 回傳的交易狀態訊息
    /// </summary>
    public string? RtnMsg { get; set; }

    /// <summary>
    /// 回傳的交易金額
    /// </summary>
    public int? TradeAmt { get; set; }

    /// <summary>
    /// 支付方式的類型
    /// </summary>
    public string? PaymentType { get; set; }

    /// <summary>
    /// 支付手續費。 0表示沒有手續費
    /// </summary>
    public string? PaymentTypeChargeFee { get; set; }

    /// <summary>
    /// 交易成立的時間
    /// </summary>
    public string? MerchantTradeDate { get; set; }

    public virtual Donation Donation { get; set; } = null!;
}
