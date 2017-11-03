using System.ComponentModel.DataAnnotations;

namespace Aula02Api.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; }        
    }
}
