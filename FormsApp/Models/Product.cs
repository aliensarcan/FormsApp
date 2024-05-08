using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name="Urun Id")]
        public int ProductId { get; set; }
       
        [Display(Name = "Urun Ad")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Urun Fiyat")]
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }



    }
}
