using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using EkipProjesi.Core;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Cryptography.Cryptography;

namespace EkipProjesi.Data.Repositories
{
    public class ExcelRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public ExcelRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        public void ExcelLog(IExcel model)
        {
            try
            {
                ExcelYuklemeleri excel = new ExcelYuklemeleri();
                excel.Durum = model._ExcelDurumlari;
                excel.ExcelTuru = model._ExcelTurleri;
                excel.SatirSayisi = model.SatirSayisi;
                excel.SessionID = model.SessionID;
                excel.UserID = model.UserID;
                if (model.HataliSatirSayisi == null) model.HataliSatirSayisi = 0;
                excel.HataliSatirSayisi = model.HataliSatirSayisi;
                excel.ExcelID = model.ExcelID;
                excel.Tarih = DateTime.Now;

                _db.ExcelYuklemeleri.Add(excel);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #region Genel İşlemler
        public string ToSlug(string input)
        {
            input = input.Trim();
            input = Regex.Replace(input, @"[^\w\@-]", "-").ToLower();
            Dictionary<string, string> replacements = new Dictionary<string, string> { { "ğ", "g" }, { "ü", "u" }, { "ş", "s" }, { "ı", "i" }, { "ö", "o" }, { "ç", "c" }, { "--", "" }, { " ", "" } };

            foreach (var key in replacements.Keys)
            {
                input = Regex.Replace(input, key, replacements[key]);
            }
            while (input.IndexOf("--") > -1)
                input = input.Replace("--", "-");
            return input;
        }

        string IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address == email)
                {
                    return email;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        string IsPhoneValid(string tel)
        {
            tel = tel.Replace(" ", String.Empty);
            tel = tel.Replace("(", "");
            tel = tel.Replace(")", "");
            tel = tel.Replace("+", "");
            if (tel.First() == '9') tel = tel.Remove(0, 1);
            if (tel.First() != '0') tel = "0" + tel;
            if (tel.Length == 11)
            {
                string s = "+" + string.Format("{0}({1}) {2} {3} {4}", tel.Substring(0, 1), tel.Substring(1, 3), tel.Substring(4, 3), tel.Substring(7, 2), tel.Substring(9, 2));
                return s;
            }
            else
            {
                return "";
            }
        }
        public string IsValidWebSite(string uriName)
        {
            try
            {
                Uri uriResult;
                bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                    && uriResult.Scheme == Uri.UriSchemeHttp;
                if (result)
                {
                    return uriName;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Kontroller
        public bool ExcelPersonelKontrol(List<string> data, int userID, out IKullanici kullanici, out IKullaniciRol rol, out string hata)
        {
            kullanici = new IKullanici();
            hata = "";
            string temp = "";
            try
            {
                #region zorunlu alanlar
                if (PersonelKontrol(data[2], out hata))
                {
                    kullanici.TC = data[2].ToLower();
                    kullanici.TC = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kullanici.TC.Trim());
                }
                else
                {
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[0]))
                {
                    kullanici.Ad = data[0];
                }
                else
                {
                    hata = "Ad alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[1]))
                {
                    kullanici.Soyad = data[1];
                }
                else
                {
                    hata = "Soyad alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[3]))
                {
                    kullanici.DogumTarihi = data[3];
                }
                else
                {
                    hata = "Doğum tarihi alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[4]))
                {
                    kullanici.Cinsiyet = data[4];
                }
                else
                {
                    hata = "Cinsiyet alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[5]))
                {
                    kullanici.Meslek = data[5];
                }
                else
                {
                    hata = "Unvan alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[6]))
                {
                    kullanici.Telefon = data[6];
                }
                else
                {
                    hata = "Cep Telefonu alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[7]))
                {
                    kullanici.KurumTelefonu = data[7];
                }
                else
                {
                    hata = "Kurum Telefonu alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[8]))
                {
                    kullanici.Email = data[8];
                }
                else
                {
                    hata = "Kişisel E-Posta alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[9]))
                {
                    kullanici.KurumEposta = data[9];
                }
                else
                {
                    hata = "Kurum E-Posta alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                if (!string.IsNullOrEmpty(data[10]))
                {
                    string t = data[10];
                    try
                    {
                        kullanici.KurumID = _db.Kurumlar.Where(x => x.KurumAdi == t).FirstOrDefault().KurumID;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                else
                {
                    hata = "Kurum alanı boş olamaz!";
                    rol = null;
                    return false;
                }

                int k = kullanici.KurumID;

                if (!string.IsNullOrEmpty(data[11]))
                {
                    string p = data[11];
                    try
                    {
                        kullanici.HizmetMerkeziID = _db.KurumHizmetMerkezleri.Where(x => x.Ad == p && x.KurumID == k).FirstOrDefault().ID;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                else
                {
                    hata = "Hizmet merkezi alanı boş olamaz!";
                    rol = null;
                    return false;
                }
                #endregion

                #region Kullanıcı Rolü
                rol = new IKullaniciRol();
                if (!string.IsNullOrEmpty(data[12]))
                {
                    string z = data[12];
                    try
                    {
                        rol.RolId = _db.Roller.Where(x => x.Rol == z).FirstOrDefault().ID;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                else
                {
                    hata = "Kullanıcı rolü alanı boş olamaz!";
                    return false;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                rol = null;
                return false;
            }
        }
        public bool PersonelKontrol(string tc, out string mesaj)
        {
            mesaj = "";
            try
            {
                if (!string.IsNullOrEmpty(tc))
                {
                    tc = tc.ToLower().Replace(" ", "");
                    if (_db.Kullanicilar.Where(x => x.TC.ToLower().Replace(" ", "").Contains(tc)).Count() == 0)
                    {

                    }
                    else
                    {
                        mesaj = "TC Kimlik Numarası Sistemde Kayıtlıdır";
                        return false;
                    }
                }
                else
                {
                    mesaj = "TC Kimlik Numarası Boş Olamaz";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion

        #region Kayıt
        public List<int> CokluPersonelKayit(List<IKullanici> kullanicilar, IKullaniciRol roller, int UserID)
        {
            List<int> hata = new List<int>();
            bool error = false;
            bool error2 = false;
            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    int a = 3;
                    foreach (var model in kullanicilar)
                    {
                        model.KaydedenKullaniciID = UserID;
                        try
                        {
                            error = false;
                            error2 = false;
                            Kullanicilar f = new Kullanicilar();

                            if (!string.IsNullOrEmpty(model.TC))
                                f.TC = model.TC;

                            f.KullaniciDurumu = true;
                            f.KullaniciAdi = model.TC.Trim();
                            f.Ad = model.Ad;
                            f.Soyad = model.Soyad;
                            f.AdSoyad = model.Ad;
                            f.Telefon = model.Telefon;
                            f.KurumEposta = model.KurumEposta;
                            f.KurumTelefonu = model.KurumTelefonu;
                            f.DogumTarihi = model.DogumTarihi;
                            f.Cinsiyet = model.Cinsiyet;
                            f.Meslek = model.Meslek;
                            f.KurumID = model.KurumID;
                            f.HizmetMerkeziID = model.HizmetMerkeziID;
                            f.ExcelID = model.ExcelID;
                            f.KaydedenKullaniciID = model.KaydedenKullaniciID;
                            if (f.TC != null && f.Ad != null && f.Soyad != null)
                            {
                                string val1 = f.TC.Trim().Substring(0, 5);
                                string val2 = string.Concat(f.Ad.Trim().Substring(0, 1), f.Soyad.Trim().Substring(0, 1));
                                f.Sifre = CryptoUtilities.Encrypt(string.Concat(val1, val2));
                            }
                            else
                            {
                                f.Sifre = CryptoUtilities.Encrypt(f.TC);
                            }
                            if (error2 || error)
                            {
                                hata.Add(a);
                            }
                            else
                            {
                                f.KayitTarihi = DateTime.Now;
                                _db.Kullanicilar.Add(f);
                                _db.SaveChanges();
                            }
                            if (f.ID > 0)
                            {
                                f.PersonelId = f.ID;

                                KullaniciRolleri r = new KullaniciRolleri();
                                r.KullaniciId = f.ID;
                                r.RolId = (int)model.RolID;
                                _db.KullaniciRolleri.Add(r);
                                _db.SaveChanges();

                                #region İletişim Bilgileri
                                KullaniciIletisimBilgileri ib = new KullaniciIletisimBilgileri();
                                ib.KullaniciID = f.ID;
                                ib.Telefon = model.Telefon;
                                ib.KurumTelefonu = model.KurumTelefonu;

                                _db.KullaniciIletisimBilgileri.Add(ib);
                                _db.SaveChanges();
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            hata.Add(a);
                        }
                        a++;
                    }

                    if (hata.Count() == 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return hata;
        }

        #endregion
    }
}