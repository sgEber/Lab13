
namespace Lab13.Models
{
    public class Invoices
    {
        public int InvoicesID { get; set; }
        public Customers Customer { get; set; }
        public int CustomersID { get; set; }
        public DateTime Date { get; set; }
        public string InvoicesNumber { get; set; }
        public float Total { get; set; }
    }
}
