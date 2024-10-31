using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models;

public class PaymentModel
{
    public string PaymentId { get; set; } // Unik identifierare för betalningen
    public string UserId { get; set; } // Identifierare för användaren som gör betalningen
    public string PaymentMethod { get; set; } // Typ av betalningsmetod (t.ex. "Kreditkort", "PayPal", "Swish")
    public string CardHolderName { get; set; } // Namn på kortinnehavaren (om betalningsmetoden är kreditkort)
    public string CardNumber { get; set; } // Kortnummer (kan vara krypterat eller maskerat)
    public string ExpirationDate { get; set; } // Utgångsdatum för kortet
    public string Cvc { get; set; } // CVC-kod för kortet (kan vara krypterat eller maskerat)
    public string PaymentStatus { get; set; } // Status för betalningen (t.ex. "Pending", "Completed", "Failed")
    public decimal Amount { get; set; } // Beloppet som ska betalas
}
