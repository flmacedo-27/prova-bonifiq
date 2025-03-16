namespace ProvaPub.Presentation.DataObjects
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
