using System.ComponentModel.DataAnnotations;

namespace Aula02Api.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do usuário obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail do usuário obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha do usuário obrigatória")]
        public string Password { get; set; }
    }
}
