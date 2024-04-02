using brskweb.Model;

namespace brskweb.ModelDTO
{
    public class OrderADTO
    {
        public int OrderId { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; } = null!;

        public DateTime Date { get; set; }

     
    }
}
