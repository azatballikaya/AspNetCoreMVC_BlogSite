using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models{
    public class LoginViewModel{
        [Required]
        [EmailAddress]
        [Display(Name ="Eposta")]
        public string? Email { get; set; }
        [Required]
        [StringLength(20,ErrorMessage ="Max 20 karakter giriniz..."),MinLength(6,ErrorMessage ="Min. 6 karakter giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
    }
}