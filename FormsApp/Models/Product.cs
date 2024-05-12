using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name="Urun Id")]
        
        public int ProductId { get; set; }
        
        [Required(ErrorMessage ="ÜRÜN ADI BOŞ GEÇİLEMEZ")]
       
        [Display(Name = "Urun Ad")]

        public string? Name { get; set; } = string.Empty;
        [Required(ErrorMessage ="ÜRÜN FİYATI BOŞ GEÇİLEMEZ")]
        [Range(0,100000) ]
        [Display(Name = "Fıyat")]
        public decimal? Price { get; set; }
        
        
        //[Required(ErrorMessage ="RESİM BOŞ GEÇİLEMEZ")]
        [Display(Name ="Resım")]
        public string? Image { get; set; } = string.Empty;

        
        public bool IsActive { get; set; }


        
        [Required(ErrorMessage ="KATEGORİ BOŞ GEÇİLEMEZ")]
        public int CategoryId { get; set; }



    }
}
