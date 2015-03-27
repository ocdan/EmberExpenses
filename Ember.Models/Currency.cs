namespace Ember.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Currency
    {
        [Key]
        public string CurrencyCode { get; set; }

        public string CurrencyDescription { get; set; }

        public int CurrencyIsoCode { get; set; }

        public ICollection<StatementLine> StatementLines { get; set; }

        public ICollection<ExpenseLine> ExpenseLines { get; set; }
    }
}
