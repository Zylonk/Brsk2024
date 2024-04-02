using brskweb.Model;
using System.Security.Cryptography;

namespace brskweb.ModelDTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime Date { get; set; }
        public static Order ConvertToOrder(OrderDTO dto)
        {
            Order ord = new Order();
            ord.OrderId = dto.OrderId;
            ord.UserId = dto.UserId;
            ord.Status = dto.Status;
            ord.Date = dto.Date;
            return ord;
        }
    }
}
