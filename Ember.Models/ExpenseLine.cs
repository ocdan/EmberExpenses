namespace Ember.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExpenseLine
    {
        [Key]
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public CostCode CostCode { get; set; }

        public decimal Total { get; set; }

        public decimal? VatAmount { get; set; }

        public Currency Currency { get; set; }

        public ICollection<StatementLine> MatchedStatementLines { get; set; }
    }
}
