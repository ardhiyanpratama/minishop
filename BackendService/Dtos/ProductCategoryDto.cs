using System.ComponentModel.DataAnnotations;

namespace BackendService.Dtos
{
    public class ProductCategoryDto
    {
        [Required(ErrorMessage = "Produk kategori tidak boleh kosong")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Produk kategori minimum 6 characters")]
        public string? Name { get; set; }
    }
}
