using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models{
    public class RegisterViewModel{
        [Required]
        [Display(Name ="Username")]
         public string? UserName { get; set; }
         [Required]
         [Display(Name ="Ad Soyad")]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Eposta")]
        public string? Email { get; set; }
        [Required]
        [StringLength(20,ErrorMessage ="Max 20 karakter giriniz..."),MinLength(6,ErrorMessage ="Min. 6 karakter giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Parolanız eşleşmiyor...")]
        [Display(Name ="Parola Tekrar")]
        public string? ConfirmPassword { get; set; }
    }
}