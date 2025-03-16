namespace ProvaPub.Presentation.DataObjects
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<OrderDto> Orders { get; set; }
    }
}
