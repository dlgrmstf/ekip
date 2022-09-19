using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EkipProjesi.Core.Kullanici;
using SmsApiNode;
using SmsApiNode.Operations;
using SmsApiNode.Types;

namespace EkipProjesi.Data.Repositories
{
    public class SMSRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public SMSRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        static void Main()
        {
            var messenger = new Messenger("panel4.ekomesaj.com", "ordinatrum123..", "MAUciB87");
            GetCredit(messenger);
            GetSenders(messenger);
        }
        static void GetCredit(Messenger messenger)
        {
            var getCreditResponse = messenger.GetCredit();

            if (getCreditResponse.Err == null)
            {
                long Credit = getCreditResponse.Credit;
            }
            else
            {
                int Statu = getCreditResponse.Err.Status;
                string Kodu = getCreditResponse.Err.Code;
                string Mesajı = getCreditResponse.Err.Message;
            }
        }

        static void GetSenders(Messenger messenger)
        {
            var getSendersResponse = messenger.GetSenders();

            if (getSendersResponse.Err == null)
            {
                foreach (Sender item in getSendersResponse.Senders)
                {
                    string Title = item.Title;
                    int Status = item.Status;
                    string Uuid = item.Uuid;                    
                }
            }
            else
            {
                int Statu = getSendersResponse.Err.Status;
                string Kodu = getSendersResponse.Err.Code;
                string Mesajı = getSendersResponse.Err.Message;               
            }
        }

        public bool SMSOnayKoduGonder(string KullaniciAdi)
        {
            try
            {
                var messenger = new Messenger("panel4.ekomesaj.com", "ordinatrum123..", "MAUciB87");

                var kullaniciTelefonNo = _db.Kullanicilar.Where(x => x.KullaniciAdi == KullaniciAdi).Select(x => x.Telefon).FirstOrDefault();

                SendSingleSms singleSmsRequest = new SendSingleSms();
                singleSmsRequest.Number = long.Parse(kullaniciTelefonNo);
                singleSmsRequest.Sender = "EKIP";

                singleSmsRequest.Title = "Test Gönderimi";

                Random random = new Random();
                string characters = "1234567890";
                string DogrulamaKodu = "";
                for (int i = 0; i < 6; i++)
                {
                    DogrulamaKodu += characters[random.Next(characters.Length)];
                }

                singleSmsRequest.Content = "Doğrulama kodunuz:" + DogrulamaKodu + "  Güvenliğiniz için bu kodu personelimiz dahil hiç kimseyle paylaşmayınız.";
                singleSmsRequest.Validity = 180;

                var singleSmsResponse = messenger.SendSingleSms(singleSmsRequest);

                if (singleSmsResponse.Err == null)
                {
                    long PaketId = singleSmsResponse.PackageId;
                    return true;
                }
                else
                {
                    int Statu = singleSmsResponse.Err.Status;
                    string Code = singleSmsResponse.Err.Code;
                    string Message = singleSmsResponse.Err.Message;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}