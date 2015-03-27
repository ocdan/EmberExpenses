namespace Ember.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StatementLine
    {
        [Key]
        public int Id { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime OccurredDate { get; set; }

        public string MerchantName { get; set; }

        public string MerchantCity { get; set; }

        public decimal OriginalAmount { get; set; }

        public Currency Currency { get; set; }

        public double ConversionRate { get; set; }

        public double BilledAmount { get; set; }

        // this should probably link back to an accounts table, but fine for now
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public bool IsForexFee { get; set; }
    }
}
