using System.ComponentModel.DataAnnotations;

namespace BackendService.Dtos
{
    public class ProductVariantDto
    {
        [Required(ErrorMessage = "Produk kategori tidak boleh kosong")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Produk kategori minimum 6 characters")]
        public string? Name { get; set; }
        public Guid? MsProductId { get; set; }
        public double? Qty { get; set; }
        public double? Price { get; set; }
        public IFormFile? File { get; set; }
    }
}
