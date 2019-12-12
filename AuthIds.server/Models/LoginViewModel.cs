using System.ComponentModel.DataAnnotations;

namespace AuthIds.server.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Identifiant")]
        [Required(ErrorMessage = "L'identifiant est obligatoire")]
        public string Login { get; set; }

        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}
