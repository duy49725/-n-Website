using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWebNangCao.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
		public string UserName { get; set; }
        public double TotalPrice { get; set; }
	}
}
