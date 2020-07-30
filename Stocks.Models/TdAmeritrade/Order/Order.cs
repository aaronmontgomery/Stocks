namespace Stocks.Models.TdAmeritrade.Order
{
    public class Market
    {
        public string OrderType { get; set; } //'MARKET' or 'LIMIT' or 'STOP' or 'STOP_LIMIT' or 'TRAILING_STOP' or 'MARKET_ON_CLOSE' or 'EXERCISE' or 'TRAILING_STOP_LIMIT' or 'NET_DEBIT' or 'NET_CREDIT' or 'NET_ZERO'
        public string Session { get; set; } //'NORMAL' or 'AM' or 'PM' or 'SEAMLESS'
        public string Duration { get; set; } //'DAY' or 'GOOD_TILL_CANCEL' or 'FILL_OR_KILL'
        public string OrderStrategyType { get; set; } //'NONE' or 'COVERED' or 'VERTICAL' or 'BACK_RATIO' or 'CALENDAR' or 'DIAGONAL' or 'STRADDLE' or 'STRANGLE' or 'COLLAR_SYNTHETIC' or 'BUTTERFLY' or 'CONDOR' or 'IRON_CONDOR' or 'VERTICAL_ROLL' or 'COLLAR_WITH_STOCK' or 'DOUBLE_DIAGONAL' or 'UNBALANCED_BUTTERFLY' or 'UNBALANCED_CONDOR' or 'UNBALANCED_IRON_CONDOR' or 'UNBALANCED_VERTICAL_ROLL' or 'CUSTOM'
        public OrderLeg[] OrderLegCollection { get; set; }
    }

    public class Limit : Market
    {
        public decimal Price { get; set; }
    }

    public class Stop : Limit
    {
        public string ComplexOrderStrategyType { get; set; } //'NONE' or 'COVERED' or 'VERTICAL' or 'BACK_RATIO' or 'CALENDAR' or 'DIAGONAL' or 'STRADDLE' or 'STRANGLE' or 'COLLAR_SYNTHETIC' or 'BUTTERFLY' or 'CONDOR' or 'IRON_CONDOR' or 'VERTICAL_ROLL' or 'COLLAR_WITH_STOCK' or 'DOUBLE_DIAGONAL' or 'UNBALANCED_BUTTERFLY' or 'UNBALANCED_CONDOR' or 'UNBALANCED_IRON_CONDOR' or 'UNBALANCED_VERTICAL_ROLL' or 'CUSTOM'
        public string StopPriceLinkBasis { get; set; } //'MANUAL' or 'BASE' or 'TRIGGER' or 'LAST' or 'BID' or 'ASK' or 'ASK_BID' or 'MARK' or 'AVERAGE'
        public string StopPriceLinkType { get; set; } //'VALUE' or 'PERCENT' or 'TICK'
        public double StopPriceOffset { get; set; }
        public decimal StopPrice { get; set; }
        public string StopType { get; set; } //'STANDARD' or 'BID' or 'ASK' or 'LAST' or 'MARK'
    }

    public class Order : Limit
    {
        public ulong OrderId { get; set; }
        public ulong AccountId { get; set; }
        public string CancelTime { get; set; }
        public string CloseTime { get; set; }
        public double Quantity { get; set; }
        public double FilledQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public string RequestedDestination { get; set; } //'INET' or 'ECN_ARCA' or 'CBOE' or 'AMEX' or 'PHLX' or 'ISE' or 'BOX' or 'NYSE' or 'NASDAQ' or 'BATS' or 'C2' or 'AUTO'
        public string DestinationLinkName { get; set; }
        public string ReleaseTime { get; set; }
        public string PriceLinkBasis { get; set; } //'MANUAL' or 'BASE' or 'TRIGGER' or 'LAST' or 'BID' or 'ASK' or 'ASK_BID' or 'MARK' or 'AVERAGE'
        public string PriceLinkType { get; set; } //'VALUE' or 'PERCENT' or 'TICK'
        public string TaxLotMethod { get; set; } //'FIFO' or 'LIFO' or 'HIGH_COST' or 'LOW_COST' or 'AVERAGE_COST' or 'SPECIFIC_LOT'
        public bool Cancelable { get; set; }
        public bool Editable { get; set; }
        public string Status { get; set; }
        public string EnteredTime { get; set; }
    }
}
