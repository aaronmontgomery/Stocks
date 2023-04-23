using System;

namespace Stocks.Entities
{
    public class Setting
    {
        public string Key { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
        
        public DateTime Updated { get; set; }
        
        public Guid Id { get; set; }
    }
}
