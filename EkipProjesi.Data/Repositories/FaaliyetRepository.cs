using EkipProjesi.Core;
using EkipProjesi.Core.Faaliyet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class FaaliyetRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public FaaliyetRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion
        public bool FaaliyetEkle(IFaaliyet model)
        {
            try
            {
                Faaliyetler h = new Faaliyetler();

                h.FaaliyetAdi = model.FaaliyetAdi;
                h.FaaliyetKonusu = model.FaaliyetKonusu;
                h.FaaliyetTuru = model.FaaliyetTuru;
                h.FaaliyetTarihi = model.FaaliyetTarihi;
                h.GerceklestigiYer = model.GerceklestigiYer;
                h.GerceklestirenKurum = model.GerceklestirenKurum;
                h.GerceklestirenKisi = model.GerceklestirenKisi;
                h.UlasilanKisiSayisi = model.UlasilanKisiSayisi;
                h.FaaliyetRaporu = model.FaaliyetRaporu;
                h.KayitTarihi = DateTime.Now;

                _db.Faaliyetler.Add(h);
                _db.SaveChanges();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public List<IFaaliyet> Faaliyetler()
        {
            List<IFaaliyet> faaliyetler = new List<IFaaliyet>();
            try
            {

                faaliyetler = (from f in _db.Faaliyetler
                               orderby f.FaaliyetTarihi descending
                            select new IFaaliyet
                            {
                               ID = f.ID,
                               FaaliyetAdi = f.FaaliyetAdi,
                               FaaliyetKonusu = f.FaaliyetKonusu,
                               FaaliyetRaporu = f.FaaliyetRaporu,
                               FaaliyetTuru = f.FaaliyetTuru,
                               FaaliyetTarihi = (DateTime)f.FaaliyetTarihi,
                               GerceklestigiYer = f.GerceklestigiYer,
                               GerceklestirenKisi = f.GerceklestirenKisi,
                               GerceklestirenKurum = f.GerceklestirenKurum,
                               UlasilanKisiSayisi = (int)f.UlasilanKisiSayisi

                            }).ToList();

                return faaliyetler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IFaaliyet FaaliyetDetay(int id)
        {
            IFaaliyet faaliyet = new IFaaliyet();
            try
            {

                faaliyet = (from f in _db.Faaliyetler
                            where f.ID == id
                               select new IFaaliyet
                               {
                                   ID = f.ID,
                                   FaaliyetAdi = f.FaaliyetAdi,
                                   FaaliyetKonusu = f.FaaliyetKonusu,
                                   FaaliyetRaporu = f.FaaliyetRaporu,
                                   FaaliyetTuru = f.FaaliyetTuru,
                                   FaaliyetTarihi = (DateTime)f.FaaliyetTarihi,
                                   GerceklestigiYer = f.GerceklestigiYer,
                                   GerceklestirenKisi = f.GerceklestirenKisi,
                                   GerceklestirenKurum = f.GerceklestirenKurum,
                                   UlasilanKisiSayisi = (int)f.UlasilanKisiSayisi

                               }).FirstOrDefault();

                return faaliyet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool FaaliyetDuzenle(IFaaliyet model, out string hata)
        {
            hata = "";
            try
            {
                Faaliyetler h = _db.Faaliyetler.FirstOrDefault(x => x.ID == model.ID);

                h.FaaliyetAdi = model.FaaliyetAdi;
                h.FaaliyetKonusu = model.FaaliyetKonusu;
                h.FaaliyetTuru = model.FaaliyetTuru;
                h.FaaliyetTarihi = model.FaaliyetTarihi;
                h.GerceklestigiYer = model.GerceklestigiYer;
                h.GerceklestirenKurum = model.GerceklestirenKurum;
                h.GerceklestirenKisi = model.GerceklestirenKisi;
                h.UlasilanKisiSayisi = model.UlasilanKisiSayisi;
                h.FaaliyetRaporu = model.FaaliyetRaporu;
                h.KayitTarihi = DateTime.Now;

                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                hata = "Kayıt İşlemi Sırasında Bir Hata Meydana Geldi." + ex.Message + " - " + ex.InnerException;
                return false;
            }
        }
        public List<IFaaliyet> FaaliyetFiltreleme(DateTime? baslangictarihi, DateTime? bitistarihi)
        {
            List<IFaaliyet> faaliyetler = new List<IFaaliyet>();
            try
            {

                faaliyetler = (from f in _db.Faaliyetler
                               where f.KayitTarihi >= baslangictarihi && f.KayitTarihi <= bitistarihi
                               orderby f.FaaliyetTarihi descending
                               select new IFaaliyet
                               {
                                   ID = f.ID,
                                   FaaliyetAdi = f.FaaliyetAdi,
                                   FaaliyetKonusu = f.FaaliyetKonusu,
                                   FaaliyetRaporu = f.FaaliyetRaporu,
                                   FaaliyetTuru = f.FaaliyetTuru,
                                   FaaliyetTarihi = (DateTime)f.FaaliyetTarihi,
                                   GerceklestigiYer = f.GerceklestigiYer,
                                   GerceklestirenKisi = f.GerceklestirenKisi,
                                   GerceklestirenKurum = f.GerceklestirenKurum,
                                   UlasilanKisiSayisi = (int)f.UlasilanKisiSayisi

                               }).ToList();

                return faaliyetler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
    }
}