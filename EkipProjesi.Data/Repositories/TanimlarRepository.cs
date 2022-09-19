using EkipProjesi.Core;
using EkipProjesi.Core.Personel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class TanimlarRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public TanimlarRepository()
        {
            _db = new EKIPEntities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        #region ÜNVANLAR
        public bool UnvanEkle(IUnvan model)
        {
            try
            {
                Unvanlar u = new Unvanlar();
                u.UnvanAdi = model.UnvanAdi;
                u.Status = true;
                _db.Unvanlar.Add(u);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool UnvanDuzenle(IUnvan model)
        {
            try
            {
                Unvanlar u = _db.Unvanlar.Where(x => x.ID == model.ID).FirstOrDefault();
                u.UnvanAdi = model.UnvanAdi;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool UnvanStatus(int id, bool status)
        {
            try
            {
                Unvanlar u = _db.Unvanlar.Where(x => x.ID == id).FirstOrDefault();

                u.Status = status;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public List<IUnvan> Unvanlar(int isletmeID)
        {
            List<IUnvan> model = new List<IUnvan>();
            try
            {
                model = (from u in _db.Unvanlar
                         orderby u.UnvanAdi
                         select new IUnvan
                         {
                             ID = u.ID,
                             UnvanAdi = u.UnvanAdi,
                             Status = u.Status,
                             PersonelSayisi = _db.PersonelGorevHareketleri.Where(x => x.UnvanId == u.ID && x.Status == true).Count()
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion        

        #region İL-İLÇE
        public List<IIl> Iller()
        {
            List<IIl> model = new List<IIl>();
            try
            {
                model = (from i in _db.Il
                         orderby i.ILAdi
                         select new IIl
                         {
                             IlID = i.KOD.Value,
                             IlAdi = i.ILAdi,
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }

        public List<IIlce> Ilceler(int IlId)
        {
            List<IIlce> model = new List<IIlce>();
            try
            {
                model = (from ilce in _db.Ilce
                         orderby ilce.ILCEAdi
                         where ilce.ILKOD == IlId
                         select new IIlce
                         {
                             IlceID = ilce.ID,
                             IlceAdi = ilce.ILCEAdi
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion        

    }
}
