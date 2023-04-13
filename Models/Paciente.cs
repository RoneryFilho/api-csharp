using System.ComponentModel.DataAnnotations;

namespace Models {
    public class Paciente {

        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Nome não estar vazio")]
        [StringLength(200, ErrorMessage = "O nome não pode ter mais de 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
        public string Email { get; set; }

        public Paciente() {
            this.Codigo = 0;
            this.Nome = "";
            this.Email = "";
        }

    }
}