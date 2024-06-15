namespace Lab13.Models
{
    public class Details
    {
        public int DetailsID { get; set; }
        public Invoices Invoice { get; set; }
        public int InovicesID { get; set; }
        public Products Product { get; set; }
        public int ProductsID { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public float Subtotal { get; set; }
    }
}
