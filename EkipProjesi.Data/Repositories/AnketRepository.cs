using EkipProjesi.Core;
using EkipProjesi.Core.Formlar;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.UyariDuyuruBildirimMesaj;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class AnketRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        private MesajRepository _mesajRepo;
        public AnketRepository()
        {
            _db = new EKIPEntities();
            _mesajRepo = new MesajRepository();
        }

        #region Anket Oluşturma
        public bool AnketOlustur(AnketDTO model)
        {
            try
            {
                Anketler a = new Anketler();
                a.Aciklama = model.Aciklama;
                a.AnketTipID = model.AnketTipID;
                a.BaslangicTarihi = model.BaslangicTarihi;
                a.Baslik = model.Baslik;
                a.BitisTarihi = model.BitisTarihi;
                a.IsletmeID = model.IsletmeID;
                a.KayitTarihi = DateTime.Now;
                a.Status = true;
                a.UserID = model.UserID;

                _db.Anketler.Add(a);
                _db.SaveChanges();
                model.ID = a.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public AnketDTO FormDetayBilgi(int id)
        {
            AnketDTO model = new AnketDTO();

            model.UserID = model.UserID;

            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         where a.ID == id
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             Baslik = a.Baslik,
                             AnketTipID = a.AnketTipID,
                             BaslangicTarihi = a.BaslangicTarihi,
                             BitisTarihi = a.BitisTarihi,
                             ID = a.ID,
                             IsletmeID = a.IsletmeID,
                             KayitTarihi = a.KayitTarihi,
                             Status = a.Status,
                             UserID = a.UserID,
                             AnketTipAdi = t.TipAdi,
                             SoruSayisi = _db.AnketSorulari.Count(x => x.AnketID == a.ID),
                             AnketKullanicilari = (from ak in _db.AnketKullanicilari
                                                   join k in _db.Hastalar on ak.PersonelID equals k.HastaID
                                                   where ak.AnketID == a.ID
                                                   select new AnketKullanicilariDTO
                                                   {
                                                       ID = ak.ID,
                                                       HastaAdi = k.HastaAdi,
                                                       HastaSoyadi = k.HastaSoyadi,
                                                       HastaID = k.HastaID,
                                                       Tarih = ak.Tarih,
                                                       //KullaniciMail = k.KullaniciAdi,
                                                       TamamlandiMi = ak.TamamlandiMi
                                                   }).Distinct().ToList()
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }

        public bool AnketGuncelle(AnketDTO model)
        {
            try
            {
                Anketler a = _db.Anketler.FirstOrDefault(x => x.ID == model.ID);
                a.Aciklama = model.Aciklama;
                a.AnketTipID = model.AnketTipID;
                a.BaslangicTarihi = model.BaslangicTarihi;
                a.Baslik = model.Baslik;
                a.BitisTarihi = model.BitisTarihi;
                a.Status = model.Status;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public string AnketYayinaAlKaldir(long id, int kullaniciId, out int islemId, out string anketBaslik, out string anketTarih, out List<string> kullaniciMailList)
        {
            anketBaslik = "";
            anketTarih = "";
            kullaniciMailList = new List<string>();
            try
            {
                islemId = Convert.ToInt32(id);
                AnketDTO a = AnketDetay(id);
                List<AnketSorulariDTO> sorular = AnketSorulari(id, 0);
                if (!a.Status)
                {
                    if (a.AnketKullanicilari.Count() > 0)
                    {
                        if (sorular.Any(x => x.Cevaplar.Count() == 0))
                        {
                            return "Cevabı Olmayan Sorular Bulunmaktadır.";
                        }
                        else
                        {
                            Anketler q = _db.Anketler.FirstOrDefault(x => x.ID == id);
                            q.Status = true;
                            _db.SaveChanges();
                            //MesajDBO mesaj = new MesajDBO();
                            //mesaj.BaslangicTarihi = q.BaslangicTarihi;
                            //mesaj.BitisTarihi = q.BitisTarihi;
                            //mesaj.Baslik = q.Baslik;
                            //mesaj.Icerik = q.Aciklama;
                            //mesaj.Kullanicilar = a.AnketKullanicilari.Select(x => x.KullaniciID).Distinct().ToList();
                            //mesaj.IliskiliModulId = (int)CHOS.Core.Moduller.Anket;
                            //mesaj.IliskiliTabloId = q.ID;
                            //mesaj.KaydedenKullaniciId = kullaniciId;
                            //mesaj.KayitTarihi = DateTime.Today;
                            //mesaj.MesajDurum = (int)MesajDurum.Aktif;

                            //_mesajRepo.MesajKontrol(mesaj);
                            anketBaslik = q.Baslik;
                            anketTarih = q.BitisTarihi.ToString();

                            foreach (var ak in a.AnketKullanicilari)
                            {
                                kullaniciMailList.Add(ak.KullaniciMail);
                            }

                            return "";
                        }
                    }
                    else
                    {
                        return "Bu Ankete Kullanıcı Bulunamamıştır.";
                    }
                }
                else
                {
                    Anketler q = _db.Anketler.FirstOrDefault(x => x.ID == id);
                    q.Status = false;
                    _db.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                islemId = Convert.ToInt32(id);
                log.Error(ex);
                return "Kayıt Sırasında Hata Oluştu";
            }
        }

        //public bool AnketMesajGonder(long id, int userid)
        //{
        //    try
        //    {
        //        AnketDTO a = AnketDetay(id);
        //        MesajDBO mesaj = new MesajDBO();
        //        mesaj.BaslangicTarihi = a.BaslangicTarihi;
        //        mesaj.BitisTarihi = a.BitisTarihi;
        //        mesaj.Baslik = a.Baslik;
        //        mesaj.Icerik = a.Aciklama;
        //        mesaj.Kullanicilar = a.AnketKullanicilari.Select(x => x.KullaniciID).Distinct().ToList();
        //        mesaj.IliskiliModulId = (int)CHOS.Core.Moduller.Anket;
        //        mesaj.IliskiliTabloId = a.ID;
        //        mesaj.KaydedenKullaniciId = userid;
        //        mesaj.KayitTarihi = DateTime.Today;
        //        mesaj.MesajDurum = (int)MesajDurum.Aktif;

        //        return _mesajRepo.MesajKontrol(mesaj);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public List<AnketDTO> Anketler()
        {
            List<AnketDTO> model = new List<AnketDTO>();
            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         orderby a.Baslik
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             Baslik = a.Baslik,
                             AnketTipID = a.AnketTipID,
                             BaslangicTarihi = a.BaslangicTarihi,
                             BitisTarihi = a.BitisTarihi,
                             ID = a.ID,
                             IsletmeID = a.IsletmeID,
                             KayitTarihi = a.KayitTarihi,
                             Status = a.Status,
                             UserID = a.UserID,
                             AnketTipAdi = t.TipAdi,
                             KullaniciSayisi = _db.AnketKullanicilari.Count(x => x.AnketID == a.ID),
                             TamamlayanSayisi = _db.AnketKullanicilari.Count(x => x.AnketID == a.ID && x.TamamlandiMi == true),
                             SoruSayisi = _db.AnketSorulari.Count(x => x.AnketID == a.ID)
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public AnketDTO AnketDetay(long id)
        {
            AnketDTO model = new AnketDTO();
            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         where a.ID == id
                         orderby a.Baslik
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             Baslik = a.Baslik,
                             AnketTipID = a.AnketTipID,
                             BaslangicTarihi = a.BaslangicTarihi,
                             BitisTarihi = a.BitisTarihi,
                             ID = a.ID,
                             IsletmeID = a.IsletmeID,
                             KayitTarihi = a.KayitTarihi,
                             Status = a.Status,
                             UserID = a.UserID,
                             AnketTipAdi = t.TipAdi,
                             SoruSayisi = _db.AnketSorulari.Count(x => x.AnketID == a.ID),
                             AnketKullanicilari = (from ak in _db.AnketKullanicilari
                                                   join k in _db.Hastalar on ak.PersonelID equals k.HastaID
                                                   where ak.AnketID == a.ID
                                                   select new AnketKullanicilariDTO
                                                   {
                                                       ID = ak.ID,
                                                       HastaAdi = k.HastaAdi,
                                                       HastaSoyadi = k.HastaSoyadi,
                                                       HastaID = k.HastaID,
                                                       HastaTCKimlikNo = k.HastaTCKimlikNo,
                                                       Tarih = ak.Tarih,
                                                       TamamlandiMi = ak.TamamlandiMi
                                                   }).Distinct().ToList()
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public bool AnketKullaniciEkle(List<int> personeller, long anket)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    personeller.Distinct().ToList();
                    MesajDBO mesaj = new MesajDBO();
                    Anketler a = _db.Anketler.FirstOrDefault(x => x.ID == anket);
                    mesaj.BaslangicTarihi = a.BaslangicTarihi;
                    mesaj.BitisTarihi = a.BitisTarihi;
                    mesaj.Baslik = a.Baslik;
                    mesaj.Icerik = a.Aciklama;

                    mesaj.IliskiliModulId = (int)EkipProjesi.Core.Moduller.Anket;
                    mesaj.IliskiliTabloId = a.ID;
                    mesaj.KaydedenKullaniciId = a.UserID;
                    mesaj.KayitTarihi = DateTime.Today;
                    mesaj.MesajDurum = (int)MesajDurum.Aktif;
                    List<int> ekli = _db.AnketKullanicilari.Where(x => x.AnketID == anket).Select(x => x.PersonelID).ToList();
                    if (a.Status)
                    {
                        foreach (var p in personeller.Where(x => !ekli.Contains(x)))
                        {
                            AnketKullanicilari k = new AnketKullanicilari();
                            k.AnketID = anket;
                            k.PersonelID = p;
                            k.Tarih = DateTime.Now;

                            _db.AnketKullanicilari.Add(k);
                        }
                        mesaj.Kullanicilar = personeller.Where(x => !ekli.Contains(x)).ToList();
                    }
                    else
                    {
                        _db.AnketKullanicilari.RemoveRange(_db.AnketKullanicilari.Where(x => x.AnketID == anket));
                        _db.SaveChanges();
                        foreach (var p in personeller.Distinct().ToList())
                        {
                            AnketKullanicilari k = new AnketKullanicilari();
                            k.AnketID = anket;
                            k.PersonelID = p;
                            k.Tarih = DateTime.Now;

                            _db.AnketKullanicilari.Add(k);
                        }
                    }

                    _db.SaveChanges();

                    trans.Commit();
                    _mesajRepo.MesajKontrol(mesaj);
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    log.Error(ex);
                    return false;
                }
            }
        }

        public bool AnketKullaniciSil(long anket, int user)
        {
            try
            {
                _db.AnketKullanicilari.Remove(_db.AnketKullanicilari.FirstOrDefault(x => x.AnketID == anket && x.PersonelID == user));
                _db.SaveChanges();
                //_mesajRepo.MesajPasifYap(anket, Core.Moduller.Anket, user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AnketKopyala(AnketDTO model)
        {
            try
            {
                AnketDTO eski = AnketDetay(model.ID);
                eski.AnketGruplari = AnketGrupSorular(model.ID);
                if (AnketOlustur(model))
                {
                    foreach (var g in eski.AnketGruplari)
                    {
                        g.AnketID = model.ID;
                        g.UserID = model.UserID;
                        if (AnketGrubuEkle(g))
                        {
                            foreach (var s in g.Sorular)
                            {
                                s.AnketID = model.ID;
                                s.GrupID = g.ID;
                                if (AnketSoruEkle2(s))
                                {
                                    foreach (var c in s.Cevaplar)
                                    {
                                        c.SoruID = s.ID;
                                        if (AnketCevapEkle(c))
                                        {

                                        }
                                        else { return false; }
                                    }
                                }
                                else { return false; }
                            }
                        }
                        else { return false; }
                    }
                }
                else { return false; }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion
        #region Anket Tipleri
        public bool AnketTipKayit(AnketTipleriDTO model, int UserID)
        {
            try
            {
                AnketTipleri t = new AnketTipleri();
                t.Aciklama = model.Aciklama;
                t.TipAdi = model.TipAdi;
                t.Status = true;
                t.UserID = UserID;
                t.KayitTarihi = DateTime.Now;
                _db.AnketTipleri.Add(t);
                _db.SaveChanges();
                model.ID = t.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return true;
            }
        }
        public bool AnketTipGuncelle(AnketTipleriDTO model)
        {
            try
            {
                AnketTipleri t = _db.AnketTipleri.FirstOrDefault(x => x.ID == model.ID);
                t.Aciklama = model.Aciklama;
                t.TipAdi = model.TipAdi;
                t.Status = model.Status;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return true;
            }
        }
        public List<AnketTipleriDTO> AnketTipleri()
        {
            List<AnketTipleriDTO> model = new List<AnketTipleriDTO>();
            try
            {
                model = (from t in _db.AnketTipleri
                         orderby t.TipAdi
                         select new AnketTipleriDTO
                         {
                             TipAdi = t.TipAdi,
                             Aciklama = t.Aciklama,
                             ID = t.ID,
                             Status = t.Status,
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        public AnketTipleriDTO FormTipiDetay(int id)
        {
            AnketTipleriDTO model = new AnketTipleriDTO();

            model.UserID = model.UserID;

            try
            {
                model = (from r in _db.AnketTipleri
                         where r.ID == id
                         select new AnketTipleriDTO
                         {
                             ID = r.ID,
                             TipAdi = r.TipAdi,
                             Aciklama = r.Aciklama,
                             Status = r.Status,
                             UserID = r.UserID
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion
        #region Soru Bankası
        public bool SoruKayit(AnketSoruBankasiDTO model, int UserID)
        {
            try
            {
                AnketSoruBankasi a = new AnketSoruBankasi();
                a.Aciklama = model.Aciklama;
                a.KayitTarihi = DateTime.Now;
                a.Soru = model.Soru;
                a.SoruKodu = model.SoruKodu;
                a.Status = true;
                a.UserID = UserID;

                _db.AnketSoruBankasi.Add(a);
                _db.SaveChanges();
                model.ID = (int)a.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool SoruGuncelle(AnketSoruBankasiDTO model, int UserID)
        {
            try
            {
                AnketSoruBankasi a = _db.AnketSoruBankasi.FirstOrDefault(x => x.ID == model.ID);
                a.Aciklama = model.Aciklama;
                a.Soru = model.Soru;
                a.SoruKodu = model.SoruKodu;
                a.Status = model.Status;
                a.UserID = UserID;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool SoruSil(int id)
        {
            try
            {
                AnketSoruBankasi model = _db.AnketSoruBankasi.FirstOrDefault(x => x.ID == id);

                _db.AnketSoruBankasi.Remove(model);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public AnketSoruBankasiDTO SoruDetay(int id, int UserID)
        {
            AnketSoruBankasiDTO model = new AnketSoruBankasiDTO();

            model.UserID = UserID;

            try
            {
                model = (from r in _db.AnketSoruBankasi
                         where r.ID == id
                         select new AnketSoruBankasiDTO
                         {
                             ID = r.ID,
                             Soru = r.Soru,
                             SoruKodu = r.SoruKodu,
                             Aciklama = r.Aciklama,
                             Status = r.Status,
                             UserID = r.UserID
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<AnketSoruBankasiDTO> SoruBankasi(bool? status)
        {
            List<AnketSoruBankasiDTO> model = new List<AnketSoruBankasiDTO>();
            try
            {
                model = (from s in _db.AnketSoruBankasi
                         where (status == null ? 1 == 1 : s.Status == status.Value)
                         select new AnketSoruBankasiDTO
                         {
                             Aciklama = s.Aciklama,
                             ID = s.ID,
                             KayitTarihi = s.KayitTarihi,
                             Soru = s.Soru,
                             SoruKodu = s.SoruKodu,
                             Status = s.Status
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        public AnketSoruBankasiDTO SoruBankasiDetay(long id)
        {
            AnketSoruBankasiDTO model = new AnketSoruBankasiDTO();
            try
            {
                model = (from s in _db.AnketSoruBankasi
                         where s.ID == id
                         select new AnketSoruBankasiDTO
                         {
                             Aciklama = s.Aciklama,
                             ID = s.ID,
                             KayitTarihi = s.KayitTarihi,
                             Soru = s.Soru,
                             SoruKodu = s.SoruKodu,
                             Status = s.Status
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        #endregion
        #region Anket Gruplari
        public bool AnketGrubuEkle(AnketGruplariDTO model)
        {
            try
            {
                AnketGruplari a = new AnketGruplari();
                a.Aciklama = model.Aciklama;
                a.GrupAdi = model.GrupAdi.Trim();
                a.AnketID = model.AnketID;
                a.Sira = model.Sira;
                a.UserID = model.UserID;
                a.Status = true;
                a.KayitTarihi = DateTime.Now;

                _db.AnketGruplari.Add(a);
                _db.SaveChanges();
                model.ID = a.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketGrubuGuncelle(AnketGruplariDTO model)
        {
            try
            {
                AnketGruplari a = _db.AnketGruplari.FirstOrDefault(x => x.ID == model.ID);
                a.Aciklama = model.Aciklama;
                a.GrupAdi = model.GrupAdi.Trim();
                a.Sira = model.Sira;
                a.Status = model.Status;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<AnketGruplariDTO> AnketGruplari(long anketid)
        {
            List<AnketGruplariDTO> model = new List<AnketGruplariDTO>();
            try
            {
                model = (from a in _db.AnketGruplari
                         where a.AnketID == anketid
                         orderby a.Sira
                         select new AnketGruplariDTO
                         {
                             Aciklama = a.Aciklama,
                             GrupAdi = a.GrupAdi,
                             ID = a.ID,
                             Sira = a.Sira,
                             Status = a.Status,
                             AnketID = a.AnketID,
                             KayitTarihi = a.KayitTarihi,
                             UserID = a.UserID
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        public List<AnketGruplariDTO> AnketGrupSorular(long anketid)
        {
            List<AnketGruplariDTO> model = new List<AnketGruplariDTO>();
            try
            {
                model = (from a in _db.AnketGruplari
                         where a.AnketID == anketid
                         orderby a.Sira
                         select new AnketGruplariDTO
                         {
                             Aciklama = a.Aciklama,
                             GrupAdi = a.GrupAdi,
                             ID = a.ID,
                             Sira = a.Sira,
                             Status = a.Status,
                             Sorular = (from s in _db.AnketSorulari
                                        join b in _db.AnketSoruBankasi on s.SoruID equals b.ID
                                        where s.GrupID == a.ID
                                        select new AnketSorulariDTO
                                        {
                                            Agirlik = s.Agirlik,
                                            SoruID = s.SoruID,
                                            SoruTipi = s.SoruTipi,
                                            ZorunluMu = s.ZorunluMu,
                                            Cevaplar = (from c in _db.AnketCevaplari
                                                        where c.SoruID == s.ID
                                                        select new AnketCevaplariDTO
                                                        {
                                                            CevapAdi = c.CevapAdi,
                                                            Puan = c.Puan
                                                        }).ToList()
                                        }).ToList()
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        public bool AnketGrupSil(long id)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    List<long> soruid = _db.AnketSorulari.Where(x => x.GrupID == id).Select(x => x.ID).ToList();
                    _db.AnketCevaplari.RemoveRange(_db.AnketCevaplari.Where(x => soruid.Contains(x.SoruID)));

                    _db.AnketSorulari.RemoveRange(_db.AnketSorulari.Where(x => x.GrupID == id));
                    _db.AnketGruplari.Remove(_db.AnketGruplari.FirstOrDefault(x => x.ID == id));
                    _db.SaveChanges();
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    log.Error(ex);
                    return false;
                }
            }
        }
        #endregion
        #region Anket Soru Cevap İlişki
        public bool AnketSoruEkle2(AnketSorulariDTO model)
        {
            try
            {
                AnketSorulari a = new AnketSorulari();
                a.Agirlik = model.Agirlik;
                a.AnketID = model.AnketID;
                a.CevapSayisi = 0;
                a.GrupID = model.GrupID;
                a.SoruID = model.SoruID;
                a.SoruTipi = model.SoruTipi;
                a.ZorunluMu = model.ZorunluMu;

                _db.AnketSorulari.Add(a);
                _db.SaveChanges();
                model.ID = a.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketSoruEkle(long grupid, long anketid, List<long> sorular)
        {
            try
            {
                foreach (var model in sorular)
                {
                    if (!_db.AnketSorulari.Any(x => x.GrupID == grupid && x.SoruID == model))
                    {
                        AnketSorulari a = new AnketSorulari();
                        a.Agirlik = 0;
                        a.AnketID = anketid;
                        a.CevapSayisi = 0;
                        a.GrupID = grupid;
                        a.SoruID = model;
                        a.SoruTipi = 0;
                        a.ZorunluMu = false;

                        _db.AnketSorulari.Add(a);
                        _db.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketSoruPuanGuncelle(List<AnketSorulariDTO> list)
        {
            try
            {
                foreach (var model in list)
                {
                    AnketSorulari a = _db.AnketSorulari.FirstOrDefault(x => x.ID == model.ID);
                    a.Agirlik = model.Agirlik;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketSoruTipGuncelle(AnketSorulariDTO model)
        {
            try
            {
                AnketSorulari a = _db.AnketSorulari.FirstOrDefault(x => x.ID == model.ID);
                if (a.SoruTipi != model.SoruTipi)
                {
                    _db.AnketCevaplari.RemoveRange(_db.AnketCevaplari.Where(x => x.SoruID == model.ID));
                }
                a.SoruTipi = model.SoruTipi;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketSoruZorunluGuncelle(AnketSorulariDTO model)
        {
            try
            {
                AnketSorulari a = _db.AnketSorulari.FirstOrDefault(x => x.ID == model.ID);
                a.ZorunluMu = model.ZorunluMu;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketSoruSil(long id)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.AnketCevaplari.RemoveRange(_db.AnketCevaplari.Where(x => x.SoruID == id));
                    _db.AnketSorulari.Remove(_db.AnketSorulari.FirstOrDefault(x => x.ID == id));
                    _db.SaveChanges();
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    trans.Rollback();
                    return false;
                }
            }

        }
        public List<AnketSorulariDTO> AnketSorulari(long anketid, long grupid)
        {
            List<AnketSorulariDTO> model = new List<AnketSorulariDTO>();
            try
            {
                model = (from a in _db.AnketSorulari
                         join g in _db.AnketGruplari on a.GrupID equals g.ID
                         join s in _db.AnketSoruBankasi on a.SoruID equals s.ID
                         where a.AnketID == anketid
                         where (grupid == 0 ? 1 == 1 : a.GrupID == grupid)
                         select new AnketSorulariDTO
                         {
                             Agirlik = a.Agirlik,
                             AnketID = a.AnketID,
                             CevapSayisi = a.CevapSayisi,
                             GrupID = a.GrupID,
                             GrupAdi = g.GrupAdi,
                             ID = a.ID,
                             SoruAciklama = s.Aciklama,
                             SoruAdi = s.Soru,
                             SoruID = a.SoruID,
                             SoruTipi = a.SoruTipi,
                             ZorunluMu = a.ZorunluMu,
                             _SoruTipi = a.SoruTipi,
                             Cevaplar = (from c in _db.AnketCevaplari
                                         where c.SoruID == a.ID
                                         select new AnketCevaplariDTO
                                         {
                                             ID = c.ID,
                                             SoruID = c.SoruID,
                                             CevapAdi = c.CevapAdi,
                                             Puan = c.Puan
                                         }).ToList(),
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public bool AnketCevapEkle(AnketCevaplariDTO model)
        {
            try
            {
                if (!_db.AnketCevaplari.Any(x => x.SoruID == model.SoruID && x.CevapAdi == model.CevapAdi.Trim()))
                {
                    AnketCevaplari a = new AnketCevaplari();
                    a.CevapAdi = model.CevapAdi.Trim();
                    a.Puan = model.Puan;
                    a.SoruID = model.SoruID;

                    _db.AnketCevaplari.Add(a);
                    _db.SaveChanges();
                    model.ID = a.ID;
                }
                else
                {
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
        public bool AnketCevapGuncelle(AnketCevaplariDTO model)
        {
            try
            {
                AnketCevaplari a = _db.AnketCevaplari.FirstOrDefault(x => x.ID == model.ID);
                a.CevapAdi = model.CevapAdi;
                a.Puan = model.Puan;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool AnketCevapSil(long id)
        {
            try
            {
                _db.AnketCevaplari.Remove(_db.AnketCevaplari.FirstOrDefault(x => x.ID == id));
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
        #region Kullanıcı Anketleri

        //public List<IHastalar> PersonelIsletme(int id)
        //{
        //    List<IHastalar> model = new List<IHastalar>();
        //    try
        //    {
        //        model = (from p in _db.Hastalar
        //                 join k in _db.Kullanicilar on p.PersonelID equals k.PersonelId
        //                 where p.IsletmeId == id && p.PersonelDurumu == true && k.KullaniciDurumu == true && k.PersonelId > 0
        //                 select new IViewPersonel
        //                 {
        //                     Ad = p.Ad + " " + p.Soyad,
        //                     TC = p.TC,
        //                     UstBirimId = p.UstBirimId,
        //                     UstBirimAdi = p.UstBirimAdi,
        //                     BirimAdi = p.BirimAdi,
        //                     BirimId = p.BirimId,
        //                     AltBirimAdi = p.AltBirimAdi,
        //                     AltBirimId = p.AltBirimId,
        //                     PersonelID = k.ID,
        //                     KimlikID = p.PersonelID
        //                 }).Distinct().OrderBy(x => x.Ad).ToList();

        //        var kullanicilar = (from k in _db.Kullanicilar
        //                            join i in _db.KullaniciIsletme on k.ID equals i.KullaniciId
        //                            where i.IsletmeId == id && k.KullaniciDurumu == true && k.PersonelId == 0
        //                            select new IViewPersonel
        //                            {
        //                                Ad = k.AdSoyad,
        //                                PersonelID = k.ID,
        //                                TC = k.TC
        //                            }).ToList();

        //        model.AddRange(kullanicilar);
        //        model = model.Distinct().OrderBy(x => x.Ad).ToList();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return model;
        //}
        //public List<AnketDTO> KullaniciAktifAnketler(int id)
        //{
        //    List<AnketDTO> model = new List<AnketDTO>();

        //    try
        //    {
        //        model = (from a in _db.Anketler
        //                 join t in _db.AnketTipleri on a.AnketTipID equals t.ID
        //                 join k in _db.AnketKullanicilari on a.ID equals k.AnketID
        //                 where a.Status == true && (DateTime.Today >= DbFunctions.TruncateTime(a.BaslangicTarihi) && DateTime.Today <= DbFunctions.TruncateTime(a.BitisTarihi)) && k.TamamlandiMi == false && k.PersonelID == id
        //                 select new AnketDTO
        //                 {
        //                     Aciklama = a.Aciklama,
        //                     BitisTarihi = a.BitisTarihi,
        //                     BaslangicTarihi = a.BaslangicTarihi,
        //                     Baslik = a.Baslik,
        //                     ID = a.ID,
        //                     AnketTipAdi = t.TipAdi
        //                 }).Distinct().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //    return model;
        //}

        public List<AnketDTO> KullaniciAktifAnketler(int id)
        {
            List<AnketDTO> model = new List<AnketDTO>();

            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         join k in _db.AnketKullanicilari on a.ID equals k.AnketID
                         where a.Status == true && (DateTime.Today >= DbFunctions.TruncateTime(a.BaslangicTarihi) && DateTime.Today <= DbFunctions.TruncateTime(a.BitisTarihi)) && k.TamamlandiMi == false && k.PersonelID == id
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             BitisTarihi = a.BitisTarihi,
                             BaslangicTarihi = a.BaslangicTarihi,
                             Baslik = a.Baslik,
                             ID = a.ID,
                             AnketTipAdi = t.TipAdi
                         }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public AnketDTO KullaniciAnketDetayi(long id, int userid)
        {
            AnketDTO model = new AnketDTO();
            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         join k in _db.AnketKullanicilari on a.ID equals k.AnketID
                         where a.ID == id && a.Status == true && k.TamamlandiMi == false && k.PersonelID == userid
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             BitisTarihi = a.BitisTarihi,
                             BaslangicTarihi = a.BaslangicTarihi,
                             Baslik = a.Baslik,
                             ID = a.ID,
                             AnketTipAdi = t.TipAdi
                         }).FirstOrDefault();
                if (model == null)
                {
                    return model = new AnketDTO();
                }
                if (DateTime.Today >= model.BaslangicTarihi && DateTime.Today <= model.BitisTarihi)
                {
                    model.AnketGruplari = KullaniciAnketGruplari(model.ID, userid);
                }
                else
                {
                    model = new AnketDTO();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public List<AnketGruplariDTO> KullaniciAnketGruplari(long anketid, int userid)
        {
            List<AnketGruplariDTO> model = new List<AnketGruplariDTO>();
            try
            {
                model = (from a in _db.AnketGruplari
                         where a.AnketID == anketid && a.Status == true
                         orderby a.Sira
                         select new AnketGruplariDTO
                         {
                             Aciklama = a.Aciklama,
                             GrupAdi = a.GrupAdi,
                             ID = a.ID,
                             Sira = a.Sira,
                             AnketID = a.AnketID,
                             Status = a.Status,
                             Sorular = (from sa in _db.AnketSorulari
                                        join s in _db.AnketSoruBankasi on sa.SoruID equals s.ID
                                        where sa.GrupID == a.ID
                                        select new AnketSorulariDTO
                                        {
                                            Agirlik = sa.Agirlik,
                                            CevapSayisi = sa.CevapSayisi,
                                            ID = sa.ID,
                                            SoruAciklama = s.Aciklama,
                                            SoruAdi = s.Soru,
                                            SoruID = sa.SoruID,
                                            SoruTipi = sa.SoruTipi,
                                            ZorunluMu = sa.ZorunluMu,
                                            _SoruTipi = sa.SoruTipi,
                                            Cevaplar = (from c in _db.AnketCevaplari
                                                        where c.SoruID == sa.ID
                                                        select new AnketCevaplariDTO
                                                        {
                                                            ID = c.ID,
                                                            SoruID = c.SoruID,
                                                            CevapAdi = c.CevapAdi,
                                                            Puan = c.Puan
                                                        }).ToList(),
                                            KullaniciCevaplari = (from k in _db.AnketKullaniciCevap
                                                                  where k.AnketSoruID == sa.ID && k.UserID == userid
                                                                  select new AnketKullaniciCevapDTO
                                                                  {
                                                                      Cevap = k.Cevap,
                                                                      CevapID = k.CevapID,
                                                                      AnketSoruID = sa.ID
                                                                  }).ToList(),
                                        }).ToList()
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public bool CevapKayit(List<AnketKullaniciCevapDTO> model)
        {
            try
            {
                foreach (var c in model)
                {
                    if (string.IsNullOrEmpty(c.Cevap)) c.Cevap = "";
                    if (_db.AnketKullaniciCevap.Any(x => x.AnketSoruID == c.AnketSoruID && x.UserID == c.UserID))
                    {
                        _db.AnketKullaniciCevap.RemoveRange(_db.AnketKullaniciCevap.Where(x => x.AnketSoruID == c.AnketSoruID && x.UserID == c.UserID));
                        _db.SaveChanges();
                    }
                }
                foreach (var c in model)
                {
                    AnketKullaniciCevap a = new AnketKullaniciCevap();
                    a.AnketSoruID = c.AnketSoruID;
                    a.Cevap = c.Cevap;
                    a.CevapID = c.CevapID;
                    a.Tarih = DateTime.Now;
                    a.UserID = c.UserID;

                    _db.AnketKullaniciCevap.Add(a);
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool KullaniciAnketBitir(int userid, long anketid)
        {
            try
            {
                List<AnketSorulariDTO> sorular = AnketSorulari(anketid, 0).Where(x => x.ZorunluMu == true).ToList();
                bool kontrol = true;
                foreach (var s in sorular)
                {
                    if (_db.AnketKullaniciCevap.Any(x => x.AnketSoruID == s.ID && x.UserID == userid))
                    {
                        kontrol = false;
                    }
                }
                if (kontrol)
                {
                    AnketKullanicilari a = _db.AnketKullanicilari.FirstOrDefault(x => x.PersonelID == userid && x.AnketID == anketid);
                    a.Tarih = DateTime.Now;
                    a.TamamlandiMi = true;
                    _db.SaveChanges();

                    //var s = _mesajRepo.MesajPasifYap(anketid, Core.Moduller.Anket, userid);
                }
                else
                {
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
        #region Anket Raporlar
        public AnketDTO AnketVerileri(long id)
        {
            AnketDTO model = new AnketDTO();
            try
            {
                model = (from a in _db.Anketler
                         join t in _db.AnketTipleri on a.AnketTipID equals t.ID
                         where a.ID == id
                         orderby a.Baslik
                         select new AnketDTO
                         {
                             Aciklama = a.Aciklama,
                             Baslik = a.Baslik,
                             AnketTipID = a.AnketTipID,
                             BaslangicTarihi = a.BaslangicTarihi,
                             BitisTarihi = a.BitisTarihi,
                             ID = a.ID,
                             IsletmeID = a.IsletmeID,
                             KayitTarihi = a.KayitTarihi,
                             Status = a.Status,
                             UserID = a.UserID,
                             AnketTipAdi = t.TipAdi,
                             KullaniciSayisi = _db.AnketKullanicilari.Count(x => x.AnketID == id),
                             AnketKullanicilari = (from ak in _db.AnketKullanicilari                   
                                                   join p in _db.Hastalar on ak.PersonelID equals p.HastaID
                                                   where ak.AnketID == a.ID && ak.TamamlandiMi == true
                                                   select new AnketKullanicilariDTO
                                                   {
                                                       ID = ak.ID,
                                                       HastaID = ak.PersonelID,
                                                       HastaAdi = p.HastaAdi,
                                                       HastaSoyadi = p.HastaSoyadi,
                                                       HastaTCKimlikNo = p.HastaTCKimlikNo,
                                                       Tarih = ak.Tarih,
                                                       TamamlandiMi = ak.TamamlandiMi,
                                                       Cevaplar = (from c in _db.AnketKullaniciCevap
                                                                   join s in _db.AnketSorulari on c.AnketSoruID equals s.ID
                                                                   where s.AnketID == a.ID && c.UserID == ak.PersonelID
                                                                   orderby c.AnketSoruID
                                                                   select new AnketKullaniciCevapDTO
                                                                   {
                                                                       AnketSoruID = c.AnketSoruID,
                                                                       Cevap = c.Cevap,
                                                                       CevapID = c.CevapID
                                                                   }).ToList(),
                                                   }).ToList()
                         }).FirstOrDefault();
                model.Sorular = AnketKullaniciSorulari(id, 0);

                foreach (var k in model.AnketKullanicilari)
                {
                    if (k.Egitim == null) k.Egitim = 0;
                    k._EgitimSekli = 0;
                    if (k.DogumTarihi == null) k.DogumTarihi = DateTime.Today;
                    foreach (var c in k.Cevaplar)
                    {
                        if (c.CevapID > 0)
                        {
                            c.Cevap = model.Sorular.FirstOrDefault(x => x.ID == c.AnketSoruID).Cevaplar.FirstOrDefault(x => x.ID == c.CevapID).CevapAdi;
                        }
                    }
                }
                foreach (var k in model.AnketKullanicilari)
                {
                    k.Cevaplar = (from c in k.Cevaplar
                                  group c by c.AnketSoruID into s
                                  select new AnketKullaniciCevapDTO
                                  {
                                      AnketSoruID = s.Key,
                                      Cevaplar = s.ToList(),
                                      Cevap = string.Join(",", s.ToList().Select(x => x.Cevap))
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        public List<AnketSorulariDTO> AnketKullaniciSorulari(long anketid, long grupid)
        {
            List<AnketSorulariDTO> model = new List<AnketSorulariDTO>();
            try
            {
                model = (from a in _db.AnketSorulari
                         join g in _db.AnketGruplari on a.GrupID equals g.ID
                         join s in _db.AnketSoruBankasi on a.SoruID equals s.ID
                         where a.AnketID == anketid
                         where (grupid == 0 ? 1 == 1 : a.GrupID == grupid)
                         select new AnketSorulariDTO
                         {
                             Agirlik = a.Agirlik,
                             AnketID = a.AnketID,
                             CevapSayisi = a.CevapSayisi,
                             GrupID = a.GrupID,
                             GrupAdi = g.GrupAdi,
                             ID = a.ID,
                             SoruAciklama = s.Aciklama,
                             SoruAdi = s.Soru,
                             SoruID = a.SoruID,
                             SoruTipi = a.SoruTipi,
                             ZorunluMu = a.ZorunluMu,
                             _SoruTipi = a.SoruTipi,
                             //KullaniciCevaplari = (from k in _db.AnketKullanicilari
                             //                      join kc in _db.AnketKullaniciCevap on k.PersonelID equals kc.UserID
                             //                      join ku in _db.Kullanicilar on kc.UserID equals ku.ID
                             //                      where k.AnketID == anketid && kc.AnketSoruID == a.ID
                             //                      select new AnketKullaniciCevapDTO
                             //                      {
                             //                          Cevap = kc.Cevap,
                             //                          CevapID = kc.CevapID,
                             //                          KullaniciAdi = ku.AdSoyad
                             //                      }).ToList(),
                             Cevaplar = (from c in _db.AnketCevaplari
                                         where c.SoruID == a.ID
                                         select new AnketCevaplariDTO
                                         {
                                             ID = c.ID,
                                             SoruID = c.SoruID,
                                             CevapAdi = c.CevapAdi,
                                             Puan = c.Puan
                                         }).ToList(),
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }
        #endregion
    }
}