using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Medico
    {
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome Obrigatório")] //é obrigatorio e nao pode string vazia
        [StringLength(200, ErrorMessage = "Nome não pode ser maior que 200 caracteres")]
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="CRM obrigatório")]
        [StringLength(9, ErrorMessage = "Nome não pode ser maior que 9 caracteres")]
        public string Crm { get; set; }

        public Medico()
        {
            this.Codigo = 0;
            this.Nome = "";
            this.DataNascimento = null;
            this.Crm = "";
        }
    }
}