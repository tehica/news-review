using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProgramiranje.Services;

namespace WebProgramiranje.IServices
{
    public interface IEmailSenderNew
    {
        void SendEmail(Message message);
    }
}
