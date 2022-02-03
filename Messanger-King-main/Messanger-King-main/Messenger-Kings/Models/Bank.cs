using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger_Kings.Models
{
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Bank ID")]
        public int Bank_ID { get; set; }

        [Required(ErrorMessage = "Account Holder is required")]
        [DisplayName("Account holder")]
        public string Bank_Account_Holder { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [DisplayName("Account number")]
        public string Bank_Account_Number { get; set; }

        [Required(ErrorMessage = "Debit oder date is Required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Debit Date")]
        public DateTime Debit_Date { get; set; }



        [DisplayName("ID Number")]
        public string Client_ID { get; set; }
        public virtual Client Client { get; set; }

        [DisplayName("Bank Name")]
        public int BankCat_ID { get; set; }
        public virtual BankCategory BankCategory { get; set; }

        [DisplayName("Type of Account")]
        public int AccountCat_ID { get; set; }
        public virtual AccountCategory  AccountCategory{ get; set; }

    

    }
}