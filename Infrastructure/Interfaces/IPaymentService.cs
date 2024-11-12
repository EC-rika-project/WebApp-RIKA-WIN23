using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces;

public interface IPaymentService
{

    PaymentModel GetPaymentMethod(string UserId);

    ControllerResultModel CreatePaymentMethod(PaymentModel paymentModel);

    ControllerResultModel UpdatePaymentMethod(PaymentModel paymentModel);

    ControllerResultModel RemovePaymentMethod(string userId);


}
