using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class Rate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Rate ID")]
        public int Rate_ID { get; set; }

        [Required(ErrorMessage = "Service cost is required")]
        [DisplayName("Sevice cost"), DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Base_Cost { get; set; }

        [Required(ErrorMessage = "Cost per CM  is required")]
        [DisplayName("Cost per CM"), DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Rate_PerCM { get; set; }

        [Required(ErrorMessage = "Cost per KG is required")]
        [DisplayName("Cost per KG"), DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Rate_PerKG { get; set; }

        [Required(ErrorMessage = "Cost per KM is required")]
        [DisplayName("Cost per KM"), DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Rate_PerKM { get; set; }

        public int ClientCat_ID { get; set; }
        public virtual ClientCategory ClientCategory { get; set; }



    }
}