using System;
using System.ComponentModel.DataAnnotations;

namespace Aula02Api.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Descrição da tarefa é obrigatória.")]   
        [MinLength(3)]    
        public string Descricao { get; set; }

        public DateTime Data { get; set; }
        public bool Terminada { get; set; }
    }
}