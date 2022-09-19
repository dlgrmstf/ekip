using EkipProjesi.Core;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Cryptography.Cryptography;
using System;
using System.Globalization;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class HesapRepository
    {
        private EKIPEntities _db;

        public HesapRepository()
        {
            _db = new EKIPEntities();
        }

        public bool YeniKullaniciDTO(IKullanici model)
        {
            try
            {
                Kullanicilar k = new Kullanicilar();
                k.AdSoyad = model.Ad;
                k.KullaniciAdi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.KullaniciAdi);
                k.Sifre = CryptoUtilities.Encrypt(model.Sifre);
                k.KullaniciDurumu = true;
                k.PersonelId = model.TipID;

                _db.Kullanicilar.Add(k);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Giris(IKullanici model, out int id, out string ad, out int tipID)
        {
            try
            {
                string s = CryptoUtilities.Encrypt(model.Sifre);
                IKullanici ku = (from k in _db.Kullanicilar
                                   where k.KullaniciAdi == model.TC && k.Sifre == s && k.KullaniciDurumu == true
                                   select new IKullanici
                                   {
                                       KullaniciAdi = k.KullaniciAdi,
                                       ID = k.ID,
                                       TipID = k.PersonelId
                                   }).FirstOrDefault();
                if (ku != null)
                {
                    ad = ku.KullaniciAdi;
                    id = ku.ID;
                    tipID = ku.TipID.Value;
                    return true;
                }
                else
                {
                    ad = "";
                    id = 0;
                    tipID = 0;
                    return false;
                }
            }
            catch
            {
                ad = "";
                id = 0;
                tipID = 0;
                return false;
            }
        }
    }
}
