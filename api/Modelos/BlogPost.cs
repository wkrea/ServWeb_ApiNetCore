using System;
using System.ComponentModel.DataAnnotations;
namespace api.Modelos
{
    public class BlogPost
    {
        [Key]
        public int PostId { get; set; }
        
        [Required]
        public string Creator { get; set; }

        [Required(ErrorMessage = "Por favor suministre un Titulo"), MaxLength(30)]  
        [DataType(DataType.Text)]  
        [Display(Name = "Titulo")]  
        public string Titulo { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
    }
}