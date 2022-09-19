using EkipProjesi.Core;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Core.Randevu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class VakaTakipRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public VakaTakipRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        #region Ekip No
        public static string DecToHex(long a)
        {

            try
            {
                int n = 1;
                long b = a;
                while (b > 15)
                {
                    b /= 16;
                    n++;
                }
                string[] t = new string[n];
                int i = 0, j = n - 1;
                do
                {
                    if (a % 16 == 10) t[i] = "A";
                    else if (a % 16 == 11) t[i] = "B";
                    else if (a % 16 == 12) t[i] = "C";
                    else if (a % 16 == 13) t[i] = "D";
                    else if (a % 16 == 14) t[i] = "E";
                    else if (a % 16 == 15) t[i] = "F";
                    else t[i] = (a % 16).ToString();
                    a /= 16;
                    i++;
                }
                while ((a * 16) > 15);
                string[] r = new string[n];
                for (i = 0; i < n; i++)
                {
                    r[i] = t[j];
                    j--;
                }
                string res = string.Concat(r);
                return res;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string repl_en(string gln)
        {
            gln = gln.Replace("E", "R");
            gln = gln.Replace("F", "G");
            gln = gln.Replace("A", "S");
            gln = gln.Replace("C", "X");
            gln = gln.Replace("B", "N");
            gln = gln.Replace("D", "L");

            gln = gln.Replace("1", "K");
            gln = gln.Replace("3", "M");
            gln = gln.Replace("5", "V");
            gln = gln.Replace("7", "P");

            return gln;
        }
        public static string repl_de(string gln)
        {
            gln = gln.Replace("R", "E");
            gln = gln.Replace("G", "F");
            gln = gln.Replace("S", "A");
            gln = gln.Replace("X", "C");
            gln = gln.Replace("N", "B");
            gln = gln.Replace("L", "D");

            gln = gln.Replace("K", "1");
            gln = gln.Replace("M", "3");
            gln = gln.Replace("V", "5");
            gln = gln.Replace("P", "7");



            return gln;
        }
        public static string Reverse(string text, string type_ = "en")
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            if (type_ == "de")
                return repl_de(reverse);
            else
                return repl_en(reverse);
        }
        #endregion

        #region Kayıt İşlemleri      
        public bool HastaEkle(IHastalar model, int UserID)
        {
            try
            {
                var kontrol = _db.Hastalar.Where(x => x.HastaTCKimlikNo == model.HastaTCKimlikNo).Any();

                if (!kontrol)
                {
                    Hastalar h = new Hastalar();

                    h.KurumKodu = model.KurumKodu;
                    h.HastaAdi = model.HastaAdi;
                    h.HastaSoyadi = model.HastaSoyadi;
                    h.DogumTarihi = model.DogumTarihi;
                    h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                    h.Cinsiyet = model.Cinsiyet;
                    h.Telefon = model.Telefon;
                    h.HastaEkipNo = Reverse(DecToHex(Convert.ToInt64(model.HastaTCKimlikNo)).ToString());
                    h.KayitTarihi = DateTime.Now;
                    h.KaydedenKullaniciID = UserID;

                    _db.Hastalar.Add(h);
                    _db.SaveChanges();

                    model.HastaID = h.HastaID;

                    //h.HastaEkipNo = model.HastaID.ToString();
                    //_db.SaveChanges();

                    if(h.HastaID > 0)
                    {
                        HastaIlkKayitBilgileri hi = new HastaIlkKayitBilgileri();

                        hi.HastaID = h.HastaID;
                        hi.KaydedenKurum = (from x in _db.Kurumlar where x.KurumKodu == h.KurumKodu select x.KurumAdi).FirstOrDefault();
                        hi.KayitTarihi = DateTime.Now;

                        _db.HastaIlkKayitBilgileri.Add(hi);
                        _db.SaveChanges();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool HastaYakinBilgileriEkle(IHastaYakinBilgileri model)
        {
            try
            {
                HastaYakinBilgileri h = new HastaYakinBilgileri();

                h.HastaID = model.HastaID;
                h.YakinAdi = model.YakinAdi;
                h.YakinSoyadi = model.YakinSoyadi;
                h.YakinlikDerecesi = model.YakinlikDerecesi;
                h.Telefon = model.Telefon;
                h.Adres = model.Adres;
                _db.HastaYakinBilgileri.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool HastaIlkKayitBilgisiEkle(IHastaIlkKayitBilgileri model)
        {
            try
            {
                HastaIlkKayitBilgileri h = new HastaIlkKayitBilgileri();

                h.HastaID = model.HastaID;
                h.KayitTarihi = DateTime.Now;
                h.KurumTuru = model.KurumTuru;
                h.KaydedenKurum = model.KaydedenKurum;
                h.EldeEdilenBilgiler = model.EldeEdilenBilgiler;
                h.VakaBilgiKaynagi = model.VakaBilgiKaynagi;

                _db.HastaIlkKayitBilgileri.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool HastaIzlemBilgisiEkle(IHastaIzlemBilgileri model)
        {
            try
            {
                HastaIzlemBilgileri h = new HastaIzlemBilgileri();

                h.HastaID = model.HastaID;
                h.IzlemTarihi = DateTime.Now;
                h.IzlemYapanKurumTuru = model.IzlemYapanKurumTuru;
                h.IzlemYapanKurum = model.IzlemYapanKurum;
                h.IzlemVakaKaynagi = model.IzlemVakaKaynagi;
                h.EldeEdilenBilgiler = model.EldeEdilenBilgiler;
                h.IzlemSonucu = model.IzlemSonucu;
                h.IzlemAciklama = model.IzlemAciklama;
                h.IzlemBasligi = model.IzlemBasligi;

                _db.HastaIzlemBilgileri.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool HastaTimelineBilgisiEkle(IHastaTimeline model)
        {
            try
            {
                HastaTimeline h = new HastaTimeline();

                h.HastaID = model.HastaID;
                h.Baslik = model.Baslik;
                h.Aciklama = model.Aciklama;
                h.Tarih = model.Tarih;

                _db.HastaTimeline.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool HastaNotBilgisiEkle(IHastaNotlari model, int kaydedenKullaniciID)
        {
            try
            {
                HastaNotlari h = new HastaNotlari();

                h.HastaID = model.HastaID;
                h.NotBilgisi = model.NotBilgisi;
                h.KayitTarihi = DateTime.Now;
                h.KaydedenKullaniciID = kaydedenKullaniciID;

                _db.HastaNotlari.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion

        #region Bilgileri Getir
        public List<IHastalar> HastaBilgileriGetir()
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastaYakinBilgileri> HastaYakinBilgileriGetir(int hastaid)
        {
            List<IHastaYakinBilgileri> bilgiler = new List<IHastaYakinBilgileri>();
            try
            {

                bilgiler = (from b in _db.HastaYakinBilgileri
                         select new IHastaYakinBilgileri
                         {
                             HastaID = b.HastaID,
                             YakinAdi = b.YakinAdi,
                             YakinSoyadi = b.YakinSoyadi,
                             YakinlikDerecesi = b.YakinlikDerecesi,
                             Telefon = b.Telefon,
                             Adres = b.Adres

                         }).Where(x => x.HastaID == hastaid).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastaIlkKayitBilgileri> HastaIlkKayitBilgileriGetir(int hastaid)
        {
            List<IHastaIlkKayitBilgileri> bilgiler = new List<IHastaIlkKayitBilgileri>();
            try
            {

                bilgiler = (from b in _db.HastaIlkKayitBilgileri
                            select new IHastaIlkKayitBilgileri
                            {
                                ID = b.ID,
                                HastaID = b.HastaID,
                                KurumTuru = b.KurumTuru,
                                KaydedenKurum = b.KaydedenKurum,
                                EldeEdilenBilgiler = b.EldeEdilenBilgiler,
                                KayitTarihi = b.KayitTarihi,
                                VakaBilgiKaynagi = b.VakaBilgiKaynagi

                            }).Where(x => x.HastaID == hastaid).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastaIzlemBilgileri> HastaIzlemBilgileriGetir(int hastaid)
        {
            List<IHastaIzlemBilgileri> bilgiler = new List<IHastaIzlemBilgileri>();
            try
            {

                bilgiler = (from b in _db.HastaIzlemBilgileri
                            orderby b.IzlemTarihi descending
                            select new IHastaIzlemBilgileri
                            {
                                ID = b.ID,
                                HastaID = b.HastaID,
                                IzlemYapanKurumTuru = b.IzlemYapanKurumTuru,
                                IzlemYapanKurum = b.IzlemYapanKurum,
                                EldeEdilenBilgiler = b.EldeEdilenBilgiler,
                                IzlemSonucu = b.IzlemSonucu,
                                IzlemVakaKaynagi = b.IzlemVakaKaynagi,
                                IzlemTarihi = b.IzlemTarihi,
                                IzlemBasligi = b.IzlemBasligi,
                                IzlemAciklama = b.IzlemAciklama

                            }).Where(x => x.HastaID == hastaid).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastaTimeline> HastaTimelineBilgileriGetir(int hastaid)
        {
            List<IHastaTimeline> bilgiler = new List<IHastaTimeline>();
            try
            {

                bilgiler = (from b in _db.HastaTimeline
                            orderby b.Tarih descending
                            select new IHastaTimeline
                            {
                                ID = b.ID,
                                HastaID = b.HastaID,
                                Baslik = b.Baslik,
                                Aciklama = b.Aciklama,
                                Tarih = (DateTime)b.Tarih

                            }).Where(x => x.HastaID == hastaid).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuBilgileri> HastaRandevuBilgileriGetir(int id)
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            var hastaTc = _db.Hastalar.Where(x => x.HastaID == id).Select(x => x.HastaTCKimlikNo).FirstOrDefault();
            try
            {

                randevular = (from b in _db.RandevuBilgileri
                              where b.HastaTCKimlikNo == hastaTc
                            select new IRandevuBilgileri
                            {
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumID = b.KurumID,
                                PoliklinikTuruID = b.PoliklinikTuruID,
                                RandevuBaslangicTarihi = b.RandevuBaslangicTarihi,
                                RandevuBitisTarihi = b.RandevuBitisTarihi,
                                RandevuDurumu = b.RandevuDurumu,
                                KurumAdi = _db.Kurumlar.Where(x=>x.KurumID == b.KurumID).Select(x=>x.KurumAdi).FirstOrDefault(),
                                PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                                KaydedenKullaniciID = (int)b.KaydedenKullaniciID,
                                KaydedenKullaniciKurum = _db.KullaniciBirimBilgileri.Where(x=>x.KullaniciID == b.KaydedenKullaniciID).Select(x=>x.Kurum).FirstOrDefault(),
                                KaydedenKullaniciKurumTuru = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == b.KaydedenKullaniciID).Select(x => x.Bina).FirstOrDefault()
                            }).ToList();
                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastaNotlari> HastaNotBilgileriGetir(int hastaid)
        {
            List<IHastaNotlari> notlar = new List<IHastaNotlari>();
            try
            {

                notlar = (from b in _db.HastaNotlari
                            select new IHastaNotlari
                            {
                               HastaID = b.HastaID,
                               NotBilgisi = b.NotBilgisi,
                               KaydedenKullaniciID = b.KaydedenKullaniciID,
                               KayitTarihi = b.KayitTarihi,
                               ID = b.ID,
                               KullaniciAdi = _db.Kullanicilar.Where(x=>x.ID == b.KaydedenKullaniciID).Select(x=>x.Ad).FirstOrDefault() + " " + _db.Kullanicilar.Where(x => x.ID == b.KaydedenKullaniciID).Select(x => x.Soyad).FirstOrDefault()
                            }).Where(x => x.HastaID == hastaid).ToList();

                return notlar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IHastalar VakaDetay(int id)
        {
            IHastalar model = new IHastalar();

            try
            {
                model = (from r in _db.Hastalar
                         where r.HastaID == id
                         select new IHastalar
                         {
                             HastaID = r.HastaID,
                             HastaAdi = r.HastaAdi,
                             HastaSoyadi = r.HastaSoyadi,
                             HastaTCKimlikNo = r.HastaTCKimlikNo,
                             Telefon = r.Telefon,
                             KurumKodu = r.KurumKodu,
                             Cinsiyet = r.Cinsiyet,
                             DogumTarihi = (DateTime)r.DogumTarihi,
                             HastaEkipNo = r.HastaEkipNo
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IHastalar> KendiHastalari(int UserID)
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            where b.KaydedenKullaniciID == UserID
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastalar> KurumdakiHastalar(int UserID)
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            join k in _db.Kullanicilar on UserID equals k.ID
                            join kr in _db.Kurumlar on b.KurumKodu equals kr.KurumKodu
                            where k.KurumID == kr.KurumID
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IKurumlar KullanicininKurumu(int UserID)
        {
            IKurumlar KullaniciKurumKodu = new IKurumlar();
            try
            {

                KullaniciKurumKodu = (from b in _db.Kurumlar
                             join k in _db.Kullanicilar on b.KurumID equals k.KurumID
                             where k.ID == UserID
                            select new IKurumlar
                            {
                                KurumKodu = b.KurumKodu

                            }).FirstOrDefault();

                return KullaniciKurumKodu;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastalar> KendiHastasiMi(int UserID, string EkipNo,string TC)
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            where b.KaydedenKullaniciID == UserID
                            && b.HastaEkipNo == EkipNo || b.HastaTCKimlikNo == TC
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IHastalar> KurumdakiHastasiMi(int UserID, string EkipNo, string TC)
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            join k in _db.Kullanicilar on UserID equals k.ID
                            join kr in _db.Kurumlar on b.KurumKodu equals kr.KurumKodu
                            where k.KurumID == kr.KurumID && b.HastaEkipNo == EkipNo || b.HastaTCKimlikNo == TC
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion
    }
}