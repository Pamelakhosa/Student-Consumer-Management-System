using System.ComponentModel.DataAnnotations;

namespace ASPNETCore_DB.Models
{
    public class Consumer
    {
        [Key]
        public int ConsumerId { get; set; }//end property

        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }//end property

        [Required]
        [Display(Name = "Contact Number")]
        public string? ContactNumber { get; set; }//end property

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }//end property

        public string? Photo { get; set; }//end property
   
    }//end class
}//end namespace
