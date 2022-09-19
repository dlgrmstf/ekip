using EkipProjesi.Core.UyariDuyuruBildirimMesaj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class MesajRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public MesajRepository()
        {
            _db = new EKIPEntities();
        }

        public bool MesajEkle(MesajDBO model)
        {
            try
            {
                Mesajlar m = new Mesajlar();
                m.BaslangicTarihi = model.BaslangicTarihi;
                m.Baslik = model.Baslik;
                m.BitisTarihi = model.BitisTarihi;
                m.Icerik = model.Icerik;
                m.IliskiliModulId = model.IliskiliModulId;
                m.IliskiliTabloId = model.IliskiliTabloId;
                m.KaydedenKullaniciId = model.KaydedenKullaniciId;
                m.KayitTarihi = model.KayitTarihi;
                m.MesajDurum = model.MesajDurum;

                _db.Mesajlar.Add(m);
                _db.SaveChanges();
                model.ID = m.ID;

                if (model.Kullanicilar != null && model.Kullanicilar.Count() > 0)
                {
                    foreach (var item in model.Kullanicilar)
                    {
                        MesajGidenKullanici k = new MesajGidenKullanici();
                        k.Durum = true;
                        k.KullaniciId = item;
                        k.MesajId = m.ID;

                        _db.MesajGidenKullanici.Add(k);
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
        public bool MesajKontrol(MesajDBO model)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    if (_db.Mesajlar.Any(x => x.IliskiliModulId == model.IliskiliModulId && x.IliskiliTabloId == model.IliskiliTabloId))
                    {
                        Mesajlar m = _db.Mesajlar.FirstOrDefault(x => x.IliskiliModulId == model.IliskiliModulId && x.IliskiliTabloId == model.IliskiliTabloId);
                        m.BaslangicTarihi = model.BaslangicTarihi;
                        m.BitisTarihi = model.BitisTarihi;
                        m.Baslik = model.Baslik;
                        m.Icerik = model.Icerik;
                        m.MesajDurum = model.MesajDurum;
                        _db.SaveChanges();

                        if (model.Kullanicilar != null && model.Kullanicilar.Count() > 0)
                        {
                            //_db.MesajGidenKullanici.RemoveRange(_db.MesajGidenKullanici.Where(x => x.MesajId == m.ID));
                            //_db.SaveChanges();
                            List<int> kullanicilar = _db.MesajGidenKullanici.Where(x => x.MesajId == m.ID).Select(x => x.KullaniciId.Value).ToList();
                            foreach (var item in model.Kullanicilar)
                            {
                                if (!kullanicilar.Any(x => x == item))
                                {
                                    MesajGidenKullanici k = new MesajGidenKullanici();
                                    k.Durum = true;
                                    k.KullaniciId = item;
                                    k.MesajId = m.ID;

                                    _db.MesajGidenKullanici.Add(k);

                                }
                            }
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        MesajEkle(model);
                    }
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
    }
}