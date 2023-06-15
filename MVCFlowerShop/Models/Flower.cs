using System.ComponentModel.DataAnnotations;



namespace MVC_FlowerShop.Models
{
    public class Flower
    {
        [Key] ///primary key
        public int FlowerID { get; set; }

        [Required]
        [Display(Name = "Flower Name")]
        public string FlowerName { get; set; }

        [Required]
        [Display(Name = "Flower Type")]
        public string FlowerType { get; set; }


        [Required]
        [Display(Name = "Price")]
        public decimal FlowerPrice { get; set; }


        [Required]
        [Display(Name = "Flower Produced Date")]
        [DataType(DataType.Date)]
        public DateTime FlowerProducedDate { get; set; }


    }
}
