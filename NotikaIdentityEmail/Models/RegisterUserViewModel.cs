using System.ComponentModel.DataAnnotations;

namespace NotikaIdentityEmail.Models
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı boş bırakılamaz")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
