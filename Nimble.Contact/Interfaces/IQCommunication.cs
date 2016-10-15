using System;
using System.IO;

namespace Nimble.Contact.Interfaces
{
    public interface IQCommunication
    {
        Stream GetLoginQR();
        QRStatus GetQRStatus(Action loginSuccessAction = null);
        Stream RefreshQR();
    }
}