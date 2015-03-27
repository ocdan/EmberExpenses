namespace Ember.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CostCode
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public int GeneralLedgerCode { get; set; }

        public ICollection<ExpenseLine> ExpenseLines { get; set; }
    }
}
