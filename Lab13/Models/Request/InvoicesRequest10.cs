namespace Lab13.Models.Request
{
    public class InvoicesRequest10
    {
        public int InvoicesID { get; set; }
        public List<DetailsRequest> InvoiceDetail { get; set; }
    }
}
