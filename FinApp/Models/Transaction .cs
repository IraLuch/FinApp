using FinApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public TypeOfOperation TypeOfOperation { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public string TypeOfOperationDisplay
       => EnumHelper.GetDescription(TypeOfOperation);
    }

    public enum TypeOfOperation 
    {
        [Description("Доход")]
        Income,

        [Description("Расход")]
        Expenditure
    }


}
