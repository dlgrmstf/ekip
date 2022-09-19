using EkipProjesi.Data;
using EkipProjesi.API.Models;
using EkipProjesi.API.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using static EkipProjesi.API.Models.Models;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using EkipProjesi.Core.Hastalar;

namespace EkipProjesi.API.Controllers
{
    /// <summary>
    /// Arındırma Modulu için gerekli metodları içermektedir. Hastane hizmet bilgileri, hastane personel bilgileri ve hasta başvuru bilgilerinin kayıt ve güncelleme metodları mevcuttur. Hasta başvuru bilgisi kaydında bulunan karar bilgisi için kararkodukontrol metodu eklenmiştir. Bu metoddan karar koduna göre karar ve açıklamasına(karar için gerekli alanlara) ulaşılabilir.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Geçersiz istek</response>
    /// <response code="500">Sunucuya bağlanılamadı</response>
    [RoutePrefix("api/arindirma")]
    public class ArindirmaModuluController : ApiController
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        #endregion

        /// <summary>
        /// API Test
        /// </summary>
        /// <returns></returns>
        /// <response code="200">API is UP!</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [HttpGet, Route("test")]
        public IHttpActionResult Test()
        {
            try
            {
                return Ok("API is Up!");

            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        /// <summary>
        /// Yeni Hasta Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta kaydı bu bilgilere göre oluşturulacaktır. Hasta TC Kimlik Numarasına ait kayıt sistemimizde mevcut ise kayıt gerçekleşmez ve HastaID döndürülür. Bu HastaID üzerinden işlem yapılabilir. KayitTarihi hastanın sisteme dahil olduğu tarihtir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen hastaya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastaekle")]
        public IHttpActionResult HastaEkle(HastaEkleModel model)
        {
            try
            {
                string hastaID = "0";

                _db = new EKIPEntities();

                Hastalar h = new Hastalar();

                if (!string.IsNullOrEmpty(model.HastaTCKimlikNo))
                    h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Hasta TC Kimlik Numarası Boş Olamaz."
                    });
                }

                if (!string.IsNullOrEmpty(model.HastaEkipNo))
                    h.HastaEkipNo = model.HastaEkipNo;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "HastaEkipNo Boş Olamaz."
                    });
                }

                bool kontrol = _db.Hastalar.Where(x => x.HastaTCKimlikNo.Equals(model.HastaTCKimlikNo)).Any();
                bool ekipNoKontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();
                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (kontrol == false && ekipNoKontrol == false)
                    {
                        if (model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.HastaAdi))
                            h.HastaAdi = model.HastaAdi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta Adı Boş Olamaz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.HastaSoyadi))
                            h.HastaSoyadi = model.HastaSoyadi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta Soyadı Boş Olamaz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.Telefon))
                            h.Telefon = model.Telefon;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta Telefonu Boş Olamaz."
                            });
                        }

                        if (model.KayitTarihi != null)
                            h.KayitTarihi = model.KayitTarihi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kayıt Tarihi Boş Olamaz."
                            });
                        }

                        _db.Hastalar.Add(h);
                        _db.SaveChanges();

                        hastaID = (from x in _db.Hastalar where x.HastaTCKimlikNo == model.HastaTCKimlikNo select x.HastaID).FirstOrDefault().ToString();

                        HastaIlkKayitBilgileri hi = new HastaIlkKayitBilgileri();

                        hi.HastaID = Convert.ToInt32(hastaID);
                        hi.KaydedenKurum = (from x in _db.Kurumlar where x.KurumKodu == model.KurumKodu select x.KurumAdi).FirstOrDefault();
                        hi.KayitTarihi = DateTime.Now;

                        return Ok(new DataResult<string>()
                        {
                            Data = hastaID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Kayıt Başarılı!"
                        });
                    }
                    else
                    {
                        string hastaID2 = (from x in _db.Hastalar where x.HastaTCKimlikNo == model.HastaTCKimlikNo select x.HastaID).FirstOrDefault().ToString();

                        return Ok(new DataResult<string>()
                        {
                            Data = hastaID2,
                            Basarili = false,
                            Mesaj = "Bu TC Kimlik Numarasına/Hasta Ekip Numarasına Ait Kayıt Bulunmaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Hizmet Bilgisi Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hizmet bilgisi kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen hizmet bilgisi kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hizmetbilgisiekle")]
        public IHttpActionResult HizmetBilgisiEkle(HizmetBilgisiEkleModel model)
        {
            try
            {
                string hizmetID = "0";

                _db = new EKIPEntities();

                ArindirmaHizmetBilgileri h = new ArindirmaHizmetBilgileri();

                if (model != null && model.KurumKodu != null && model.KurumKodu != 0)
                    h.KurumKodu = model.KurumKodu;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                    });
                }

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (!kurumkontrol)
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Eksik ya da Yanlış Değer Girdiniz."
                    });
                }
                else
                {
                    if (model.KayitTarihi != null)
                        h.KayitTarihi = model.KayitTarihi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Kayıt Tarihi Boş Olamaz."
                        });
                    }
                    if (model.AktifAmatemPoliklinikSayisi != null)
                        h.AktifAmatemPoliklinikSayisi = model.AktifAmatemPoliklinikSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Amatem Poliklinik Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AktifCematemPoliklinikSayisi != null)
                        h.AktifCematemPoliklinikSayisi = model.AktifCematemPoliklinikSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Çematem Poliklinik Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemYeniBasvuruSayisi != null)
                        h.AmatemYeniBasvuruSayisi = model.AmatemYeniBasvuruSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Yeni Başvuru Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemYeniBasvuruSayisi != null)
                        h.CematemYeniBasvuruSayisi = model.CematemYeniBasvuruSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Yeni Başvuru Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemTakipBasvurusuSayisi != null)
                        h.AmatemTakipBasvurusuSayisi = model.AmatemTakipBasvurusuSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Takip Başvurusu Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemTakipBasvurusuSayisi != null)
                        h.CematemTakipBasvurusuSayisi = model.CematemTakipBasvurusuSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Takip Başvurusu Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AktifAmatemYatakSayisi != null)
                        h.AktifAmatemYatakSayisi = model.AktifAmatemYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Amatem Yatak Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AktifCematemYatakSayisi != null)
                        h.AktifCematemYatakSayisi = model.AktifCematemYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Çematem Yatak Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemBosYatakSayisi != null)
                        h.AmatemBosYatakSayisi = model.AmatemBosYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Boş Yatak Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemDoluYatakSayisi != null)
                        h.AmatemDoluYatakSayisi = model.AmatemDoluYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Dolu Yatak Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemBosYatakSayisi != null)
                        h.CematemBosYatakSayisi = model.CematemBosYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Boş Yatak Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemDoluYatakSayisi != null)
                        h.CematemDoluYatakSayisi = model.CematemDoluYatakSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Dolu Yatak Sayısı Boş Olamaz."
                        });
                    }

                    h.LogTarihi = DateTime.Now;

                    _db.ArindirmaHizmetBilgileri.Add(h);
                    _db.SaveChanges();

                    var kayitTarihi = h.KayitTarihi.ToString("dd.MM.yyyy HH:mm:ss");

                    hizmetID = _db.ArindirmaHizmetBilgileri.Where(x => x.KayitTarihi == model.KayitTarihi && x.KurumKodu == model.KurumKodu).Select(x => x.ID).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = hizmetID.ToString(),
                        Basarili = true,
                        Hata = null,
                        Mesaj = "Kayıt Başarılı!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Personel Bilgisi Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Personel bilgisi kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen personel kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("personelbilgisiekle")]
        public IHttpActionResult PersonelBilgisiEkle(PersonelBilgisiEkleModel model)
        {
            try
            {
                string personelHizmetBilgisiID = "0";

                _db = new EKIPEntities();

                ArindirmaPersonelBilgileri h = new ArindirmaPersonelBilgileri();

                if (model.KurumKodu != null && model.KurumKodu != 0)
                    h.KurumKodu = model.KurumKodu;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                    });
                }

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (!kurumkontrol)
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Eksik ya da Yanlış Değer Girdiniz."
                    });
                }
                else
                {
                    if (model.KayitTarihi != null)
                        h.KayitTarihi = model.KayitTarihi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Kayıt Tarihi Boş Olamaz."
                        });
                    }
                    if (model.TumUzmanHekimSayisi != null)
                        h.TumUzmanHekimSayisi = model.TumUzmanHekimSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Tüm Uzman Hekim Sayısı Boş Olamaz."
                        });
                    }
                    if (model.TumAsistanHekimSayisi != null)
                        h.TumAsistanHekimSayisi = model.TumAsistanHekimSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Tüm Asistan Hekim Sayısı Boş Olamaz."
                        });
                    }
                    if (model.TumPratisyenHekimSayisi != null)
                        h.TumPratisyenHekimSayisi = model.TumPratisyenHekimSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Tüm Pratisyen Hekim Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifPsikiyatriUzmaniSayisi != null)
                        h.HastaneAktifPsikiyatriUzmaniSayisi = model.HastaneAktifPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifCocukPsikiyatriUzmaniSayisi != null)
                        h.HastaneAktifCocukPsikiyatriUzmaniSayisi = model.HastaneAktifCocukPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifPsikiyatriAsistaniSayisi != null)
                        h.HastaneAktifPsikiyatriAsistaniSayisi = model.HastaneAktifPsikiyatriAsistaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Psikiyatri Asistanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifCocukPsikiyatriAsistaniSayisi != null)
                        h.HastaneAktifCocukPsikiyatriAsistaniSayisi = model.HastaneAktifCocukPsikiyatriAsistaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Çocuk Psikiyatri Asistanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifPsikologSayisi != null)
                        h.HastaneAktifPsikologSayisi = model.HastaneAktifPsikologSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Psikolog Sayısı Boş Olamaz."
                        });
                    }
                    if (model.HastaneAktifHemsireSayisi != null)
                        h.HastaneAktifHemsireSayisi = model.HastaneAktifHemsireSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Hemşire Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemAktifPsikiyatriUzmaniSayisi != null)
                        h.AmatemAktifPsikiyatriUzmaniSayisi = model.AmatemAktifPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemAktifCocukPsikiyatriUzmaniSayisi != null)
                        h.CematemAktifCocukPsikiyatriUzmaniSayisi = model.CematemAktifCocukPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Aktif Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemAktifPsikiyatriAsistaniSayisi != null)
                        h.AmatemAktifPsikiyatriAsistaniSayisi = model.AmatemAktifPsikiyatriAsistaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Aktif Psikiyatri Asistanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemAktifCocukPsikiyatriAsistaniSayisi != null)
                        h.CematemAktifCocukPsikiyatriAsistaniSayisi = model.CematemAktifCocukPsikiyatriAsistaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Aktif Çocuk Psikiyatri Asistanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemAktifPsikologSayisi != null)
                        h.AmatemAktifPsikologSayisi = model.AmatemAktifPsikologSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Aktif Psikolog Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemAktifPsikologSayisi != null)
                        h.CematemAktifPsikologSayisi = model.CematemAktifPsikologSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Aktif Psikolog Sayısı Boş Olamaz."
                        });
                    }
                    if (model.AmatemAktifHemsireSayisi != null)
                        h.AmatemAktifHemsireSayisi = model.AmatemAktifHemsireSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Amatem Aktif Hemşire Sayısı Boş Olamaz."
                        });
                    }
                    if (model.CematemAktifHemsireSayisi != null)
                        h.CematemAktifHemsireSayisi = model.CematemAktifHemsireSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Çematem Aktif Hemşire Sayısı Boş Olamaz."
                        });
                    }
                    if (model.MaddeBagimliligiEgitimiAlmisTumHekimSayisi != null)
                        h.MaddeBagimliligiEgitimiAlmisTumHekimSayisi = model.MaddeBagimliligiEgitimiAlmisTumHekimSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Madde Bağımlılığı Eğitimi Almış Tüm Hekim Sayısı Boş Olamaz."
                        });
                    }
                    if (model.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi != null)
                        h.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi = model.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Madde Bağımlılığı Eğitimi Almış Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi != null)
                        h.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi = model.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Madde Bağımlılığı Eğitimi Almış Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                        });
                    }
                    if (model.MaddeBagimliligiEgitimiAlmisPsikologSayisi != null)
                        h.MaddeBagimliligiEgitimiAlmisPsikologSayisi = model.MaddeBagimliligiEgitimiAlmisPsikologSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Madde Bağımlılığı Eğitimi Almış Psikolog Sayısı Boş Olamaz."
                        });
                    }
                    if (model.MaddeBagimliligiEgitimiAlmisHemsireSayisi != null)
                        h.MaddeBagimliligiEgitimiAlmisHemsireSayisi = model.MaddeBagimliligiEgitimiAlmisHemsireSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Madde Bağımlılığı Eğitimi Almış Hemşire Sayısı Boş Olamaz."
                        });
                    }

                    h.LogTarihi = DateTime.Now;

                    _db.ArindirmaPersonelBilgileri.Add(h);
                    _db.SaveChanges();

                    personelHizmetBilgisiID = (from x in _db.ArindirmaPersonelBilgileri where x.KayitTarihi == model.KayitTarihi select x.ID).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = personelHizmetBilgisiID.ToString(),
                        Basarili = true,
                        Hata = null,
                        Mesaj = "Kayıt Başarılı!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Hasta Başvuru Bilgisi Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta başvuru kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen başvuruya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("basvurubilgisiekle")]
        public IHttpActionResult BasvuruBilgisiEkle(HastaBasvuruBilgisiEkleModel model)
        {
            try
            {
                string basvuruID = "0";

                _db = new EKIPEntities();

                ArindirmaBasvuruBilgileri h = new ArindirmaBasvuruBilgileri();

                if (!string.IsNullOrEmpty(model.HastaEkipNo))
                {
                    bool hastaEkipNokontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();

                    if (hastaEkipNokontrol)
                        h.HastaEkipNo = model.HastaEkipNo;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu Hasta Ekip No sistemimizde bulunmamaktadır."
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Hasta Ekip No Boş Olamaz veya Yanlış Değer Girdiniz."
                    });
                }

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (!kurumkontrol)
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "İlgili KurumKodu sistemimizde bulunmamaktadır."
                    });
                }
                else
                {
                    if (model.KurumKodu != null && model.KurumKodu != 0)
                        h.KurumKodu = model.KurumKodu;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz!"
                        });
                    }

                    if (model.HastaProtokolNo != null && model.HastaProtokolNo != "0")
                    {
                        bool basvurunokontrol = _db.ArindirmaBasvuruBilgileri.Where(x => x.HastaProtokolNo.Contains(model.HastaProtokolNo)).Any();

                        if (!basvurunokontrol)
                        {
                            if (!string.IsNullOrEmpty(model.HastaProtokolNo))
                                h.HastaProtokolNo = model.HastaProtokolNo;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Başvuru Numarası Boş Olamaz!"
                                });
                            }

                            if (!string.IsNullOrEmpty(model.BeyanAdresi))
                                h.BeyanAdresi = model.BeyanAdresi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Beyan Adresi Boş Olamaz!"
                                });
                            }

                            if (!string.IsNullOrEmpty(model.BeyanTelefonu))
                                h.BeyanTelefonu = model.BeyanTelefonu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Beyan Telefonu Boş Olamaz!"
                                });
                            }

                            if (model.KayitTarihi != null)
                                h.KayitTarihi = model.KayitTarihi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kayıt Tarihi Boş Olamaz!"
                                });
                            }
                            if (model.PoliklinikMuayeneTarihSaati != null)
                                h.PoliklinikMuayeneTarihSaati = model.PoliklinikMuayeneTarihSaati;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Poliklinik Muayene Tarihi ve Saati Boş Olamaz!"
                                });
                            }
                            if (model.PoliklinikTuruID != null)
                                h.PoliklinikTuruID = model.PoliklinikTuruID;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "PoliklinikTuruID Boş Olamaz!"
                                });
                            }
                            if (model.MuayeneyiGerceklestirenHekim != null)
                                h.MuayeneyiGerceklestirenHekim = model.MuayeneyiGerceklestirenHekim;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Muayeneyi Gercekleştiren Hekim Boş Olamaz!"
                                });
                            }
                            if (model.MuayeneyiGerceklestirenHekimTuru != null)
                                h.MuayeneyiGerceklestirenHekimTuru = model.MuayeneyiGerceklestirenHekimTuru;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Muayeneyi Gercekleştiren Hekim Türü Boş Olamaz!"
                                });
                            }

                            if (model.MaddeBilgisi != null)
                                h.MaddeBilgisi = JsonConvert.SerializeObject(model.MaddeBilgisi);
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.EslikEdenHastalikOykusu != null)
                                h.EslikEdenHastalikOykusu = model.EslikEdenHastalikOykusu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Eşlik Eden Hastalık Öyküsü Boş Olamaz!"
                                });
                            }
                            if (model.PsikiyatrikHastalikOykusu != null)
                                h.PsikiyatrikHastalikOykusu = model.PsikiyatrikHastalikOykusu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Psikiyatrik Hastalık Öyküsü Boş Olamaz!"
                                });
                            }
                            if (model.PsikiyatrikHastalikOykusu == true)
                            {
                                if (model.PsikiyatrikHastalikOykusuAciklama != null)
                                    h.PsikiyatrikHastalikOykusuAciklama = model.PsikiyatrikHastalikOykusuAciklama;
                                else
                                {
                                    return Ok(new DataResult<string>()
                                    {
                                        Data = "-1",
                                        Basarili = false,
                                        Mesaj = "Psikiyatrik Hastalık Öyküsü Açıklaması Boş Olamaz!"
                                    });
                                }
                            }
                            else
                            {
                                h.PsikiyatrikHastalikOykusuAciklama = null;
                            }
                            if (model.KullanmaktaOlduguDigerIlacBilgisi != null)
                                h.KullanmaktaOlduguDigerIlacBilgisi = model.KullanmaktaOlduguDigerIlacBilgisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kullanmakta Olduğu Diğer İlaç Bilgisi Boş Olamaz!"
                                });
                            }
                            if (model.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu != null)
                                h.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = model.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Son Başvurudan Sonra Alkol/Madde Kullanımı Olmuş Mu Bilgisi Boş Olamaz!"
                                });
                            }
                            if (model.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi != null)
                                h.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = model.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Son Başvurudan Sonra Önerilen İlaçları Düzenli Kullanmış Mı Bilgisi Boş Olamaz!"
                                });
                            }
                            if (model.YoksunlukBulgusu != null)
                                h.YoksunlukBulgusu = model.YoksunlukBulgusu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Yoksunluk Bulgusu Boş Olamaz!"
                                });
                            }
                            if (model.IntoksikasyonBulgusu != null)
                                h.IntoksikasyonBulgusu = model.IntoksikasyonBulgusu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Intoksikasyon Bulgusu Boş Olamaz!"
                                });
                            }
                            if (model.IdrarToksikolojiBulgusu != null)
                                h.IdrarToksikolojiBulgusu = model.IdrarToksikolojiBulgusu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Idrar Toksikoloji Bulgusu Boş Olamaz!"
                                });
                            }

                            if (model.KararID != null && model.KararID != 0)
                            {
                                if (_db.ArindirmaBasvuruKararlari.Where(x => x.KararID.Equals(model.KararID)).Any())
                                    h.KararID = model.KararID;
                                else
                                {
                                    return Ok(new DataResult<string>()
                                    {
                                        Data = "-1",
                                        Basarili = false,
                                        Mesaj = "İlgili KararID sistemimizde bulunmamaktadır!"
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "KararID Boş Olamaz!"
                                });
                            }

                            h.LogTarihi = DateTime.Now;

                            _db.ArindirmaBasvuruBilgileri.Add(h);
                            _db.SaveChanges();

                            Core.Hastalar.HastaMaddeKullanimBilgileri m = new Core.Hastalar.HastaMaddeKullanimBilgileri();

                            StreamReader r = new StreamReader(model.MaddeBilgisi);

                            string json = r.ReadToEnd();

                            List<Core.Hastalar.HastaMaddeKullanimBilgileri> items = JsonConvert.DeserializeObject<List<Core.Hastalar.HastaMaddeKullanimBilgileri>>(json);

                            dynamic array = JsonConvert.DeserializeObject(json);
                            foreach (var item in array)
                            {
                                m.HastaEkipNo = item["HastaEkipNo"].ToString();
                                m.KurumKodu = (int)item["KurumKodu"];
                                m.HastaProtokolNo = item["HastaProtokolNo"].ToString();
                                m.MaddeTuru = item["MaddeTuru"].ToString();
                                m.Sure = item["Sure"].ToString();
                                m.KullanimSuresiAy = (bool)item["KullanimSuresiAy"];
                                m.KullanimSuresiYil = (bool)item["KullanimSuresiYil"];
                                m.Miktar = item["Miktar"].ToString();
                                m.Yontem = item["Yontem"].ToString();
                                m.Siklik = item["Siklik"].ToString();
                                m.EnSonKullanmaZamani = item["EnSonKullanmaZamani"].ToString();
                                m.EnSonKullanmaZamaniGun = (bool)item["EnSonKullanmaZamaniGun"];
                                m.EnSonKullanmaZamaniAy = (bool)item["EnSonKullanmaZamaniAy"];
                                m.EnSonKullanmaZamaniYil = (bool)item["EnSonKullanmaZamaniYil"];

                                _db.HastaMaddeKullanimBilgileri.Add(item);
                                _db.SaveChanges();
                            }

                            //JObject json = JObject.Parse(model.MaddeBilgisi);
                            //m.HastaEkipNo = json["HastaEkipNo"].ToString();
                            //m.KurumKodu = (int)json["KurumKodu"];
                            //m.HastaProtokolNo = json["HastaProtokolNo"].ToString();
                            //m.MaddeTuru = json["MaddeTuru"].ToString();
                            //m.Sure = json["Sure"].ToString();
                            //m.KullanimSuresiAy = (bool)json["KullanimSuresiAy"];
                            //m.KullanimSuresiYil = (bool)json["KullanimSuresiYil"];
                            //m.Miktar = json["Miktar"].ToString();
                            //m.Yontem = json["Yontem"].ToString();
                            //m.Siklik = json["Siklik"].ToString();
                            //m.EnSonKullanmaZamani = json["EnSonKullanmaZamani"].ToString();
                            //m.EnSonKullanmaZamaniGun = (bool)json["EnSonKullanmaZamaniGun"];
                            //m.EnSonKullanmaZamaniAy = (bool)json["EnSonKullanmaZamaniAy"];
                            //m.EnSonKullanmaZamaniYil = (bool)json["EnSonKullanmaZamaniYil"];

                            //_db.HastaMaddeKullanimBilgileri.Add(m);
                            //_db.SaveChanges();

                            basvuruID = (from x in _db.ArindirmaBasvuruBilgileri where x.HastaEkipNo == model.HastaEkipNo && x.HastaProtokolNo == model.HastaProtokolNo select x.ID).FirstOrDefault().ToString();

                            HastaBeyanBilgileri hb = new HastaBeyanBilgileri();

                            hb.BasvuruID = Convert.ToInt32(basvuruID);
                            hb.BeyanAdresi = model.BeyanAdresi;
                            hb.BeyanTelefonu = model.BeyanTelefonu;
                            hb.BeyanAldigiKurum = (from x in _db.Kurumlar where x.KurumKodu == model.KurumKodu select x.KurumAdi).FirstOrDefault();
                            hb.BeyanTarihi = DateTime.Now;

                            return Ok(new DataResult<string>()
                            {
                                Data = basvuruID.ToString(),
                                Basarili = true,
                                Hata = null,
                                Mesaj = "Kayıt Başarılı!"
                            });
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Bu HastaProtokol Numarasına Ait Kayıt Bulunmaktadır!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "HastaProtokol Numarası Boş Olamaz veya Eksik Değer Girdiniz!"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Hasta Yatış Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta yatış kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen hasta yatış bilgisine ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastayatisekle")]
        public IHttpActionResult HastaYatisEkle(HastaYatisEkleModel model)
        {
            try
            {
                string yatisID = "0";

                _db = new EKIPEntities();

                Yatislar h = new Yatislar();

                if (model.HastaEkipNo != null)
                {
                    bool hastaEkipNokontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();

                    if (hastaEkipNokontrol)
                        h.HastaEkipNo = model.HastaEkipNo;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu Hasta Ekip No sistemimizde bulunmamaktadır."
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Hasta Ekip No Boş Olamaz veya Yanlış Değer Girdiniz."
                    });
                }

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (model.KurumKodu != null && model.KurumKodu != 0)
                        h.KurumKodu = model.KurumKodu;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                        });
                    }

                    if (model.YatisTarihi != null)
                        h.YatisTarihi = model.YatisTarihi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Yatış Tarihi Boş Olamaz."
                        });
                    }

                    h.YatisSonlanmaTarihi = model.YatisSonlanmaTarihi;

                    h.YatisSonlanmaID = model.YatisSonlanmaID;

                    h.YatisSonlanmaAciklama = model.YatisSonlanmaAciklama;

                    if (_db.ArindirmaBasvuruKararlari.Where(x => x.KararID.Equals(model.KararID)).Any())
                        h.KararID = model.KararID;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "İlgili KararID sistemimizde bulunmamaktadır!"
                        });
                    }

                    h.LogTarihi = DateTime.Now;

                    _db.Yatislar.Add(h);
                    _db.SaveChanges();

                    yatisID = (from x in _db.Yatislar where x.HastaEkipNo == model.HastaEkipNo && x.KurumKodu == model.KurumKodu && x.YatisTarihi == model.YatisTarihi select x.ID).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = yatisID.ToString(),
                        Basarili = true,
                        Hata = null,
                        Mesaj = "Kayıt Başarılı!"
                    });
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Hasta Fiziksel Hastalık Öyküsü Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta fiziksel hastalık öyküsü kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen fiziksel hastalık öyküsü kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastafizikselhastalikoykusuekle")]
        public IHttpActionResult HastaFizikselHastalikOykusuEkle(HastaFizikselHastalikOykusuEkleModel model)
        {
            try
            {
                string hastalikOykusuID = "0";

                _db = new EKIPEntities();

                FizikselHastalikOykuleri h = new FizikselHastalikOykuleri();

                bool kontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();

                bool basvurunokontrol = _db.ArindirmaBasvuruBilgileri.Where(x => x.HastaProtokolNo.Equals(model.HastaProtokolNo)).Any();

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (kontrol == true)
                    {
                        if (basvurunokontrol)
                        {
                            if (!string.IsNullOrEmpty(model.HastaEkipNo))
                            {
                                h.HastaEkipNo = model.HastaEkipNo;
                            }
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "HastaEkipNo Boş Olamaz!"
                                });
                            }

                            if (model.Astim != null)
                                h.Astim = model.Astim;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Astım Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.BobrekYetmezligi != null)
                                h.BobrekYetmezligi = model.BobrekYetmezligi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Böbrek Yetmezliği Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.Diger != null)
                                h.Diger = model.Diger;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Diğer Bilgisi Boş Olamaz!"
                                });
                            }

                            h.DigerAciklama = model.DigerAciklama;

                            if (model.DM != null)
                                h.DM = model.DM;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "DM Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.Epilepsi != null)
                                h.Epilepsi = model.Epilepsi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Epilepsi Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.Hepatit != null)
                                h.Hepatit = model.Hepatit;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hepatit Bilgisi Boş Olamaz!"
                                });
                            }

                            h.HepatitAciklama = model.HepatitAciklama;

                            if (model.Hipertansiyon != null)
                                h.Hipertansiyon = model.Hipertansiyon;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hipertansiyon Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.HIV != null)
                                h.HIV = model.HIV;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "HIV Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.IlacAlerjisi != null)
                                h.IlacAlerjisi = model.IlacAlerjisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "İlaç Alerjisi Bilgisi Boş Olamaz!"
                                });
                            }

                            h.IlacAlerjisiAciklama = model.IlacAlerjisiAciklama;

                            if (model.KalpHastaligi != null)
                                h.KalpHastaligi = model.KalpHastaligi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kalp Hastalığı Bilgisi Boş Olamaz!"
                                });
                            }

                            h.KalpHastaligiAciklama = model.KalpHastaligiAciklama;

                            if (model.Siroz != null)
                                h.Siroz = model.Siroz;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Siroz Bilgisi Boş Olamaz!"
                                });
                            }

                            if (model.Yok != null)
                                h.Yok = model.Yok;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Yok Bilgisi Boş Olamaz!"
                                });
                            }
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Bu HastaProtokol Numarasına Ait Kayıt Bulunmamaktadır!"
                            });
                        }

                        h.LogTarihi = DateTime.Now;

                        _db.FizikselHastalikOykuleri.Add(h);
                        _db.SaveChanges();

                        hastalikOykusuID = (from x in _db.FizikselHastalikOykuleri where x.HastaEkipNo == model.HastaEkipNo && x.KurumKodu == model.KurumKodu && x.HastaProtokolNo == model.HastaProtokolNo select x.HastalikOykusuID).FirstOrDefault().ToString();

                        return Ok(new DataResult<string>()
                        {
                            Data = hastalikOykusuID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Kayıt Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu Hasta Ekip No Sistemimizde Bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Hastane Poliklinik Bilgisi Kaydı Eklenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hastane poliklinik bilgisi kaydı bu bilgilere göre oluşturulacaktır.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen poliklinik bilgisi kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastanepoliklinikbilgisiekle")]
        public IHttpActionResult HastanePoliklinikBilgisiEkle(HastanePoliklinikBilgisiEkleModel model)
        {
            try
            {
                string bilgiID = "0";

                _db = new EKIPEntities();

                PoliklinikTurleri h = new PoliklinikTurleri();

                if (model != null && model.KurumKodu != null && model.KurumKodu != 0)
                    h.KurumKodu = model.KurumKodu;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                    });
                }

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (!kurumkontrol)
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Kurum Kodu Eksik ya da Yanlış Değer Girdiniz."
                    });
                }
                else
                {
                    if (model.KayitTarihi != null)
                        h.KayitTarihi = model.KayitTarihi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Kayıt Tarihi Boş Olamaz."
                        });
                    }
                    if (model.PoliklinikAdi != null)
                        h.PoliklinikAdi = model.PoliklinikAdi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Poliklinik Adı Boş Olamaz."
                        });
                    }
                    if (model.AktifOdaSayisi != null)
                        h.AktifOdaSayisi = model.AktifOdaSayisi;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Aktif Oda Sayısı Boş Olamaz."
                        });
                    }

                    h.LogTarihi = DateTime.Now;

                    _db.PoliklinikTurleri.Add(h);
                    _db.SaveChanges();

                    bilgiID = (from x in _db.PoliklinikTurleri where x.KayitTarihi == model.KayitTarihi && x.KurumKodu == model.KurumKodu && x.PoliklinikAdi == model.PoliklinikAdi select x.ID).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = bilgiID.ToString(),
                        Basarili = true,
                        Hata = null,
                        Mesaj = "Kayıt Başarılı!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }       

        /// <summary>
        /// Hasta Yatış Kaydı Yatış Sonlanma Nedenleri Kontrol Metodu
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur ve hatasız girilmelidir. Yatış Sonlanma Nedenleri bu bilgilere göre kontrol edilecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı işlem yatış sonlanma nedenine ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("yatissonlanmanedenlerikontrol")]
        public IHttpActionResult YatisSonlanmaNedenleriKontrol(YatisSonlanmaNedenleriKontrolModel model)
        {
            string yatis = "";
            try
            {
                _db = new EKIPEntities();

                if (_db.YatisSonlanmaNedenleri.Where(x => x.ID == model.ID).Count() != 0)
                {
                    yatis = (from x in _db.YatisSonlanmaNedenleri where x.ID == model.ID select x.SonlanmaNedeni).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = "SonlanmaNedeni:" + yatis.ToString(),
                        Basarili = true,
                        Hata = null,
                        Mesaj = "True. İlgili SonlanmaNedeni sistemimizde bulunmaktadır. SonlanmaNedeni verilmiştir."
                    });
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "null",
                        Basarili = false,
                        Hata = null,
                        Mesaj = "False. İlgili SonlanmaNedeni sistemimizde bulunmamaktadır."
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kontrol Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Hasta Kaydı Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta kaydı bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen hastaya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastaguncelle")]
        public IHttpActionResult HastaGuncelle(HastaGuncelleModel model)
        {
            try
            {
                string hastaID = "0";

                _db = new EKIPEntities();

                if (_db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any())
                {
                    if (model.HastaEkipNo != null)
                    {
                        Hastalar h = _db.Hastalar.Where(x => x.HastaEkipNo == model.HastaEkipNo).FirstOrDefault();

                        if (!string.IsNullOrEmpty(model.HastaTCKimlikNo))
                            h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta TC Kimlik Numarası Boş Olamaz."
                            });
                        }

                        bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                        if (model.KurumKodu != null && kurumkontrol == true)
                        {
                            if (model.KurumKodu != null && model.KurumKodu != 0)
                                h.KurumKodu = model.KurumKodu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                                });
                            }

                            if (!string.IsNullOrEmpty(model.HastaAdi))
                                h.HastaAdi = model.HastaAdi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hasta Adı Boş Olamaz."
                                });
                            }

                            if (!string.IsNullOrEmpty(model.HastaSoyadi))
                                h.HastaSoyadi = model.HastaSoyadi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hasta Soyadı Boş Olamaz."
                                });
                            }

                            if (!string.IsNullOrEmpty(model.Telefon))
                                h.Telefon = model.Telefon;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hasta Telefonu Boş Olamaz."
                                });
                            }

                            if (model.KayitTarihi != null)
                                h.KayitTarihi = model.KayitTarihi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kayıt Tarihi Boş Olamaz."
                                });
                            }

                            h.OnamDurumu = model.OnamDurumu;

                            h.GuncellemeTarihi = DateTime.Now;

                            _db.SaveChanges();

                            hastaID = (from x in _db.Hastalar where x.HastaEkipNo == model.HastaEkipNo select x.HastaID).FirstOrDefault().ToString();

                            return Ok(new DataResult<string>()
                            {
                                Data = hastaID.ToString(),
                                Basarili = true,
                                Hata = null,
                                Mesaj = "Güncelleme Başarılı!"
                            });
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Hasta Ekip No Boş Olamaz ya da Eksik Değer Girdiniz!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Bu Hasta Ekip No sistemimizde bulunmamaktadır!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Hizmet Bilgisi Kaydı Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hizmet bilgisi kaydı bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni güncellenen hizmet bilgisi kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hizmetbilgisiguncelle")]
        public IHttpActionResult HizmetBilgisiGuncelle(HizmetBilgisiGuncelleModel model)
        {
            try
            {
                string hizmetID = "0";

                _db = new EKIPEntities();

                if (_db.ArindirmaHizmetBilgileri.Where(x => x.ID.Equals(model.ID)).Any())
                {
                    if (model.ID != null && model.ID != 0)
                    {
                        ArindirmaHizmetBilgileri h = _db.ArindirmaHizmetBilgileri.Where(x => x.ID == model.ID).FirstOrDefault();

                        if (model != null && model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                        if (!kurumkontrol)
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Eksik ya da Yanlış Değer Girdiniz."
                            });
                        }
                        else
                        {
                            if (model.KayitTarihi != null)
                                h.KayitTarihi = model.KayitTarihi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kayıt Tarihi Boş Olamaz."
                                });
                            }
                            if (model.AktifAmatemPoliklinikSayisi != null)
                                h.AktifAmatemPoliklinikSayisi = model.AktifAmatemPoliklinikSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Amatem Poliklinik Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AktifCematemPoliklinikSayisi != null)
                                h.AktifCematemPoliklinikSayisi = model.AktifCematemPoliklinikSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Çematem Poliklinik Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemYeniBasvuruSayisi != null)
                                h.AmatemYeniBasvuruSayisi = model.AmatemYeniBasvuruSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Yeni Başvuru Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemYeniBasvuruSayisi != null)
                                h.CematemYeniBasvuruSayisi = model.CematemYeniBasvuruSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Yeni Başvuru Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemTakipBasvurusuSayisi != null)
                                h.AmatemTakipBasvurusuSayisi = model.AmatemTakipBasvurusuSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Takip Başvurusu Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemTakipBasvurusuSayisi != null)
                                h.CematemTakipBasvurusuSayisi = model.CematemTakipBasvurusuSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Takip Başvurusu Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AktifAmatemYatakSayisi != null)
                                h.AktifAmatemYatakSayisi = model.AktifAmatemYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Amatem Yatak Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AktifCematemYatakSayisi != null)
                                h.AktifCematemYatakSayisi = model.AktifCematemYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Çematem Yatak Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemBosYatakSayisi != null)
                                h.AmatemBosYatakSayisi = model.AmatemBosYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Boş Yatak Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemDoluYatakSayisi != null)
                                h.AmatemDoluYatakSayisi = model.AmatemDoluYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Dolu Yatak Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemBosYatakSayisi != null)
                                h.CematemBosYatakSayisi = model.CematemBosYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Boş Yatak Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemDoluYatakSayisi != null)
                                h.CematemDoluYatakSayisi = model.CematemDoluYatakSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Dolu Yatak Sayısı Boş Olamaz."
                                });
                            }

                            h.GuncellemeTarihi = DateTime.Now;

                            _db.SaveChanges();

                            hizmetID = (from x in _db.ArindirmaHizmetBilgileri where x.KayitTarihi == model.KayitTarihi select x.ID).FirstOrDefault().ToString();

                            return Ok(new DataResult<string>()
                            {
                                Data = hizmetID.ToString(),
                                Basarili = true,
                                Hata = null,
                                Mesaj = "Güncelleme Başarılı!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "ID Boş Olamaz ya da Eksik Değer Girdiniz!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Bu ID'ye ait kayıt sistemimizde bulunmamaktadır!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Personel Bilgisi Kaydı Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Personel bilgisi kaydı bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen personel bilgisi kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("personelbilgisiguncelle")]
        public IHttpActionResult PersonelBilgisiGuncelle(PersonelBilgisiGuncelleModel model)
        {
            try
            {
                string personelHizmetBilgisiID = "0";

                _db = new EKIPEntities();

                if (_db.ArindirmaPersonelBilgileri.Where(x => x.ID.Equals(model.ID)).Any())
                {
                    if (model.ID != null && model.ID != 0)
                    {
                        ArindirmaPersonelBilgileri h = _db.ArindirmaPersonelBilgileri.Where(x => x.ID == model.ID).FirstOrDefault();

                        bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                        if (!kurumkontrol)
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Eksik ya da Yanlış Değer Girdiniz."
                            });
                        }
                        else
                        {
                            if (model.KayitTarihi != null)
                                h.KayitTarihi = model.KayitTarihi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kayıt Tarihi Boş Olamaz."
                                });
                            }
                            if (model.TumUzmanHekimSayisi != null)
                                h.TumUzmanHekimSayisi = model.TumUzmanHekimSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Tüm Uzman Hekim Sayısı Boş Olamaz."
                                });
                            }
                            if (model.TumAsistanHekimSayisi != null)
                                h.TumAsistanHekimSayisi = model.TumAsistanHekimSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Tüm Asistan Hekim Sayısı Boş Olamaz."
                                });
                            }
                            if (model.TumPratisyenHekimSayisi != null)
                                h.TumPratisyenHekimSayisi = model.TumPratisyenHekimSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Tüm Pratisyen Hekim Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifPsikiyatriUzmaniSayisi != null)
                                h.HastaneAktifPsikiyatriUzmaniSayisi = model.HastaneAktifPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifCocukPsikiyatriUzmaniSayisi != null)
                                h.HastaneAktifCocukPsikiyatriUzmaniSayisi = model.HastaneAktifCocukPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifPsikiyatriAsistaniSayisi != null)
                                h.HastaneAktifPsikiyatriAsistaniSayisi = model.HastaneAktifPsikiyatriAsistaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Psikiyatri Asistanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifCocukPsikiyatriAsistaniSayisi != null)
                                h.HastaneAktifCocukPsikiyatriAsistaniSayisi = model.HastaneAktifCocukPsikiyatriAsistaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Çocuk Psikiyatri Asistanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifPsikologSayisi != null)
                                h.HastaneAktifPsikologSayisi = model.HastaneAktifPsikologSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Psikolog Sayısı Boş Olamaz."
                                });
                            }
                            if (model.HastaneAktifHemsireSayisi != null)
                                h.HastaneAktifHemsireSayisi = model.HastaneAktifHemsireSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Hemşire Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemAktifPsikiyatriUzmaniSayisi != null)
                                h.AmatemAktifPsikiyatriUzmaniSayisi = model.AmatemAktifPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Aktif Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemAktifCocukPsikiyatriUzmaniSayisi != null)
                                h.CematemAktifCocukPsikiyatriUzmaniSayisi = model.CematemAktifCocukPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Aktif Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemAktifPsikiyatriAsistaniSayisi != null)
                                h.AmatemAktifPsikiyatriAsistaniSayisi = model.AmatemAktifPsikiyatriAsistaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Aktif Psikiyatri Asistanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemAktifCocukPsikiyatriAsistaniSayisi != null)
                                h.CematemAktifCocukPsikiyatriAsistaniSayisi = model.CematemAktifCocukPsikiyatriAsistaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Aktif Çocuk Psikiyatri Asistanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemAktifPsikologSayisi != null)
                                h.AmatemAktifPsikologSayisi = model.AmatemAktifPsikologSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Aktif Psikolog Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemAktifPsikologSayisi != null)
                                h.CematemAktifPsikologSayisi = model.CematemAktifPsikologSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Aktif Psikolog Sayısı Boş Olamaz."
                                });
                            }
                            if (model.AmatemAktifHemsireSayisi != null)
                                h.AmatemAktifHemsireSayisi = model.AmatemAktifHemsireSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Amatem Aktif Hemşire Sayısı Boş Olamaz."
                                });
                            }
                            if (model.CematemAktifHemsireSayisi != null)
                                h.CematemAktifHemsireSayisi = model.CematemAktifHemsireSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Çematem Aktif Hemşire Sayısı Boş Olamaz."
                                });
                            }
                            if (model.MaddeBagimliligiEgitimiAlmisTumHekimSayisi != null)
                                h.MaddeBagimliligiEgitimiAlmisTumHekimSayisi = model.MaddeBagimliligiEgitimiAlmisTumHekimSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bağımlılığı Eğitimi Almış Tüm Hekim Sayısı Boş Olamaz."
                                });
                            }
                            if (model.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi != null)
                                h.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi = model.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bağımlılığı Eğitimi Almış Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi != null)
                                h.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi = model.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bağımlılığı Eğitimi Almış Çocuk Psikiyatri Uzmanı Sayısı Boş Olamaz."
                                });
                            }
                            if (model.MaddeBagimliligiEgitimiAlmisPsikologSayisi != null)
                                h.MaddeBagimliligiEgitimiAlmisPsikologSayisi = model.MaddeBagimliligiEgitimiAlmisPsikologSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bağımlılığı Eğitimi Almış Psikolog Sayısı Boş Olamaz."
                                });
                            }
                            if (model.MaddeBagimliligiEgitimiAlmisHemsireSayisi != null)
                                h.MaddeBagimliligiEgitimiAlmisHemsireSayisi = model.MaddeBagimliligiEgitimiAlmisHemsireSayisi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Madde Bağımlılığı Eğitimi Almış Hemşire Sayısı Boş Olamaz."
                                });
                            }

                            h.GuncellemeTarihi = DateTime.Now;

                            _db.SaveChanges();

                            personelHizmetBilgisiID = (from x in _db.ArindirmaPersonelBilgileri where x.KayitTarihi == model.KayitTarihi select x.ID).FirstOrDefault().ToString();

                            return Ok(new DataResult<string>()
                            {
                                Data = personelHizmetBilgisiID.ToString(),
                                Basarili = true,
                                Hata = null,
                                Mesaj = "Güncelleme Başarılı!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "ID Boş Olamaz ya da Eksik Değer Girdiniz!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Bu ID'ye ait kayıt sistemimizde bulunmamaktadır!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Hasta Başvuru Bilgisi Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Başvuru bilgisi kaydı bu bilgilere göre güncellenecektir. HastaID ve HastaProtokolNo alanlarında güncelleme yapılamaz.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen başvuru bilgisi kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("basvurubilgisiguncelle")]
        public IHttpActionResult BasvuruBilgisiGuncelle(HastaBasvuruBilgisiGuncelleModel model)
        {
            try
            {
                string basvuruID = "0";

                _db = new EKIPEntities();

                if (model.BasvuruID != null && model.BasvuruID > 0)
                {
                    if (_db.ArindirmaBasvuruBilgileri.Where(x => x.ID.Equals(model.BasvuruID)).Any())
                    {
                        ArindirmaBasvuruBilgileri h = _db.ArindirmaBasvuruBilgileri.Where(x => x.ID == model.BasvuruID && x.HastaProtokolNo == model.HastaProtokolNo).FirstOrDefault();

                        bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                        if (!kurumkontrol)
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "İlgili KurumKodu sistemimizde bulunmamaktadır."
                            });
                        }
                        else
                        {
                            if (model.KurumKodu != null && model.KurumKodu > 0)
                                h.KurumKodu = model.KurumKodu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz!"
                                });
                            }

                            if (model.HastaProtokolNo != null && model.HastaProtokolNo != "0")
                            {
                                bool basvurunokontrol = _db.ArindirmaBasvuruBilgileri.Where(x => x.HastaProtokolNo.Equals(model.HastaProtokolNo)).Any();

                                if (basvurunokontrol)
                                {
                                    if (!string.IsNullOrEmpty(model.HastaProtokolNo) && model.HastaProtokolNo != "0")
                                    {
                                        if (!string.IsNullOrEmpty(model.BeyanAdresi))
                                            h.BeyanAdresi = model.BeyanAdresi;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Beyan Adresi Boş Olamaz!"
                                            });
                                        }
                                        if (!string.IsNullOrEmpty(model.BeyanTelefonu))
                                            h.BeyanTelefonu = model.BeyanTelefonu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Beyan Telefonu Boş Olamaz!"
                                            });
                                        }
                                        if (model.KayitTarihi != null)
                                            h.KayitTarihi = model.KayitTarihi;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Kayıt Tarihi Boş Olamaz!"
                                            });
                                        }
                                        if (model.PoliklinikMuayeneTarihSaati != null)
                                            h.PoliklinikMuayeneTarihSaati = model.PoliklinikMuayeneTarihSaati;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Poliklinik Muayene Tarihi ve Saati Boş Olamaz!"
                                            });
                                        }
                                        if (model.PoliklinikTuruID != null)
                                            h.PoliklinikTuruID = model.PoliklinikTuruID;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Poliklinik Türü Boş Olamaz!"
                                            });
                                        }
                                        if (model.MuayeneyiGerceklestirenHekim != null)
                                            h.MuayeneyiGerceklestirenHekim = model.MuayeneyiGerceklestirenHekim;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Muayeneyi Gercekleştiren Hekim Boş Olamaz!"
                                            });
                                        }
                                        if (model.MuayeneyiGerceklestirenHekimTuru != null)
                                            h.MuayeneyiGerceklestirenHekimTuru = model.MuayeneyiGerceklestirenHekimTuru;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Muayeneyi Gercekleştiren Hekim Türü Boş Olamaz!"
                                            });
                                        }
                                        if (model.MaddeBilgisi != null)
                                            h.MaddeBilgisi = JsonConvert.SerializeObject(model.MaddeBilgisi);
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Madde Bilgisi Boş Olamaz!"
                                            });
                                        }
                                        if (model.EslikEdenHastalikOykusu != null)
                                            h.EslikEdenHastalikOykusu = model.EslikEdenHastalikOykusu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Eşlik Eden Hastalık Öyküsü Boş Olamaz!"
                                            });
                                        }
                                        if (model.PsikiyatrikHastalikOykusu != null)
                                            h.PsikiyatrikHastalikOykusu = model.PsikiyatrikHastalikOykusu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Psikiyatrik Hastalık Öyküsü Boş Olamaz!"
                                            });
                                        }
                                        if (model.PsikiyatrikHastalikOykusu == true)
                                        {
                                            if (model.PsikiyatrikHastalikOykusuAciklama != null)
                                                h.PsikiyatrikHastalikOykusuAciklama = model.PsikiyatrikHastalikOykusuAciklama;
                                            else
                                            {
                                                return Ok(new DataResult<string>()
                                                {
                                                    Data = "-1",
                                                    Basarili = false,
                                                    Mesaj = "Psikiyatrik Hastalık Öyküsü Açıklaması Boş Olamaz!"
                                                });
                                            }
                                        }
                                        else
                                        {
                                            h.PsikiyatrikHastalikOykusuAciklama = null;
                                        }
                                        if (model.KullanmaktaOlduguDigerIlacBilgisi != null)
                                            h.KullanmaktaOlduguDigerIlacBilgisi = model.KullanmaktaOlduguDigerIlacBilgisi;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Kullanmakta Olduğu Diğer İlaç Bilgisi Boş Olamaz!"
                                            });
                                        }
                                        if (model.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu != null)
                                            h.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = model.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Son Başvurudan Sonra Alkol/Madde Kullanımı Olmuş Mu Bilgisi Boş Olamaz!"
                                            });
                                        }
                                        if (model.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi != null)
                                            h.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = model.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Son Başvurudan Sonra Önerilen İlaçları Düzenli Kullanmış Mı Bilgisi Boş Olamaz!"
                                            });
                                        }
                                        if (model.YoksunlukBulgusu != null)
                                            h.YoksunlukBulgusu = model.YoksunlukBulgusu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Yoksunluk Bulgusu Boş Olamaz!"
                                            });
                                        }
                                        if (model.IntoksikasyonBulgusu != null)
                                            h.IntoksikasyonBulgusu = model.IntoksikasyonBulgusu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Intoksikasyon Bulgusu Boş Olamaz!"
                                            });
                                        }
                                        if (model.IdrarToksikolojiBulgusu != null)
                                            h.IdrarToksikolojiBulgusu = model.IdrarToksikolojiBulgusu;
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "Idrar Toksikoloji Bulgusu Boş Olamaz!"
                                            });
                                        }

                                        if (model.KararID != null && model.KararID != 0)
                                        {
                                            if (_db.ArindirmaBasvuruKararlari.Where(x => x.KararID.Equals(model.KararID)).Any())
                                                h.KararID = model.KararID;
                                            else
                                            {
                                                return Ok(new DataResult<string>()
                                                {
                                                    Data = "-1",
                                                    Basarili = false,
                                                    Mesaj = "İlgili KararID sistemimizde bulunmamaktadır!"
                                                });
                                            }
                                        }
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "-1",
                                                Basarili = false,
                                                Mesaj = "KararID Boş Olamaz!"
                                            });
                                        }

                                        h.GuncellemeTarihi = DateTime.Now;

                                        _db.SaveChanges();

                                        Data.HastaMaddeKullanimBilgileri m = _db.HastaMaddeKullanimBilgileri.Where(x => x.HastaProtokolNo == model.HastaProtokolNo).FirstOrDefault();

                                        StreamReader r = new StreamReader(model.MaddeBilgisi);

                                        string json = r.ReadToEnd();

                                        List<Data.HastaMaddeKullanimBilgileri> items = JsonConvert.DeserializeObject<List<Data.HastaMaddeKullanimBilgileri>>(json);

                                        dynamic array = JsonConvert.DeserializeObject(json);
                                        foreach (var item in array)
                                        {
                                            m.HastaEkipNo = item["HastaEkipNo"].ToString();
                                            m.KurumKodu = (int)item["KurumKodu"];
                                            m.HastaProtokolNo = item["HastaProtokolNo"].ToString();
                                            m.MaddeTuru = item["MaddeTuru"].ToString();
                                            m.Sure = item["Sure"].ToString();
                                            m.KullanimSuresiAy = (bool)item["KullanimSuresiAy"];
                                            m.KullanimSuresiYil = (bool)item["KullanimSuresiYil"];
                                            m.Miktar = item["Miktar"].ToString();
                                            m.Yontem = item["Yontem"].ToString();
                                            m.Siklik = item["Siklik"].ToString();
                                            m.EnSonKullanmaZamani = item["EnSonKullanmaZamani"].ToString();
                                            m.EnSonKullanmaZamaniGun = (bool)item["EnSonKullanmaZamaniGun"];
                                            m.EnSonKullanmaZamaniAy = (bool)item["EnSonKullanmaZamaniAy"];
                                            m.EnSonKullanmaZamaniYil = (bool)item["EnSonKullanmaZamaniYil"];

                                            _db.HastaMaddeKullanimBilgileri.Add(item);
                                            _db.SaveChanges();
                                        }

                                        basvuruID = (from x in _db.ArindirmaBasvuruBilgileri where x.HastaProtokolNo == model.HastaProtokolNo select x.ID).FirstOrDefault().ToString();

                                        HastaBeyanBilgileri hb = _db.HastaBeyanBilgileri.Where(x => x.BasvuruID == model.BasvuruID).FirstOrDefault();

                                        hb.BeyanAdresi = model.BeyanAdresi;
                                        hb.BeyanTelefonu = model.BeyanTelefonu;
                                        hb.BeyanAldigiKurum = (from x in _db.Kurumlar where x.KurumKodu == model.KurumKodu select x.KurumAdi).FirstOrDefault();
                                        hb.GuncellemeTarihi = DateTime.Now;

                                        return Ok(new DataResult<string>()
                                        {
                                            Data = basvuruID.ToString(),
                                            Basarili = true,
                                            Hata = null,
                                            Mesaj = "Güncelleme Başarılı!"
                                        });
                                    }
                                    else
                                    {
                                        return Ok(new DataResult<string>()
                                        {
                                            Data = "-1",
                                            Basarili = false,
                                            Mesaj = "Başvuru Numarası Boş Olamaz ya da Eksik Değer Girdiniz!"
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new DataResult<string>()
                                    {
                                        Data = "-1",
                                        Basarili = false,
                                        Mesaj = "Bu Başvuru Numarasına Ait Kayıt Bulunmamaktadır!"
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Başvuru Numarası Boş Olamaz veya Eksik Değer Girdiniz!"
                                });
                            }
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu BasvuruID sistemimizde bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "BasvuruID Boş Olamaz ya da Eksik Değer Girdiniz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Kurum Poliklinik Türleri Kontrol Metodu
        /// </summary>
        /// <param name="KurumKodu">KurumKodu alanı zorunludur ve hatasız girilmelidir. Poliklinik türleri buna göre kontrol edilecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı işlem kuruma ait EKİP Projesi tarafındaki kurum & poliklinik bilgilerini geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("randevupoliklinikturukontrol")]
        public IHttpActionResult RandevuPoliklinikTuruKontrol(int KurumKodu)
        {
            try
            {
                _db = new EKIPEntities();

                if (_db.Kurumlar.Where(x => x.KurumKodu == KurumKodu).Count() != 0)
                {
                    int kurumid = _db.Kurumlar.Where(x => x.KurumKodu == KurumKodu).Select(x => x.KurumID).FirstOrDefault();

                    List<PoliklinikTuruKontrolModel> bilgiler = new List<PoliklinikTuruKontrolModel>();

                    bilgiler = (from p in _db.KurumPoliklinikTurleri
                                where p.KurumID == kurumid
                                select new PoliklinikTuruKontrolModel
                                {
                                    PoliklinikTuruID = p.ID,
                                    KurumID = p.KurumID,
                                    PoliklinikTuruAdi = p.PoliklinikTuru
                                }).ToList();

                    return Ok(bilgiler);
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "null",
                        Basarili = false,
                        Hata = null,
                        Mesaj = "False. İlgili kurum kodu sistemimizde bulunmamaktadır."
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kontrol Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Randevu Kayıtları Sorgulama Metodu
        /// Modelde yer alan alanlar zorunludur ve hatasız girilmelidir. Randevu bilgileri buna göre sorgulanacaktır. Günlük randevu sorgulaması için Randevu Bitis Tarihini Randevu Baslangic Tarihinden 1 gün fazla giriniz.
        /// </summary>
        /// <param name="KurumKodu">Kurum Kodu giriniz.</param>
        /// <param name="PoliklinikTuruID">Poliklinik Türü Adı giriniz.</param>
        /// <param name="RandevuBaslangicTarihi">Randevu Başlangıç Tarihi giriniz.</param>
        /// <param name="RandevuBitisTarihi">Randevu Bitiş Tarihi giriniz.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı ise işlem kuruma ait EKİP Projesi tarafındaki randevuya ait unique ID'yi ve bilgileri geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("randevusorgula")]
        public async Task<IHttpActionResult> RandevuSorgula(int KurumKodu, int PoliklinikTuruID, DateTime RandevuBaslangicTarihi, DateTime RandevuBitisTarihi)
        {
            try
            {
                _db = new EKIPEntities();

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(KurumKodu)).Any();

                int kurumid = _db.Kurumlar.Where(x => x.KurumKodu == KurumKodu).Select(x => x.KurumID).FirstOrDefault();

                bool poliklinikturidkontrol = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == kurumid && x.ID == PoliklinikTuruID).Any();

                if (KurumKodu != null && KurumKodu > 0)
                {
                    if (kurumkontrol)
                    {
                        if (_db.RandevuBilgileri.Where(x => x.KurumID == kurumid).Count() != 0)
                        {
                            if (poliklinikturidkontrol)
                            {
                                if (RandevuBaslangicTarihi != null)
                                {
                                    if (RandevuBitisTarihi != null)
                                    {
                                        List<RandevuSorgulaModel> randevular = new List<RandevuSorgulaModel>();

                                        randevular = (from r in _db.RandevuBilgileri
                                                      join k in _db.Kurumlar on r.KurumID equals k.KurumID
                                                      where r.KurumID == kurumid
                                                      join p in _db.KurumPoliklinikTurleri on r.PoliklinikTuruID equals p.ID
                                                      where r.PoliklinikTuruID == PoliklinikTuruID
                                                      where r.RandevuBaslangicTarihi >= RandevuBaslangicTarihi && r.RandevuBitisTarihi <= RandevuBitisTarihi
                                                      orderby r.RandevuBaslangicTarihi
                                                      select new RandevuSorgulaModel
                                                      {
                                                          ID = r.ID,
                                                          KurumID = k.KurumID,
                                                          KurumKodu = k.KurumKodu,
                                                          KurumAdi = k.KurumAdi,
                                                          PoliklinikTuru = p.PoliklinikTuru,
                                                          PoliklinikTuruID = p.ID,
                                                          HastaAdi = r.HastaAdi,
                                                          HastaSoyadi = r.HastaSoyadi,
                                                          HastaTCKimlikNo = r.HastaTCKimlikNo,
                                                          HastaEkipNo = _db.Hastalar.Where(x => x.HastaID == r.ID).Select(x => x.HastaEkipNo).FirstOrDefault(),
                                                          Aciklama = r.Aciklama,
                                                          RandevuBaslangicTarihi = r.RandevuBaslangicTarihi,
                                                          RandevuBitisTarihi = (DateTime)r.RandevuBitisTarihi,
                                                          Telefon = r.Telefon,
                                                          YakinTelefonu = r.YakinTelefonu
                                                      }).ToList();

                                        if (randevular != null)
                                        {
                                            return Ok(randevular);
                                        }
                                        else
                                        {
                                            return Ok(new DataResult<string>()
                                            {
                                                Data = "null",
                                                Basarili = false,
                                                Hata = null,
                                                Mesaj = "Randevu bulunamamıştır."
                                            });
                                        }

                                    }
                                    else
                                    {
                                        return Ok(new DataResult<string>()
                                        {
                                            Data = "null",
                                            Basarili = false,
                                            Hata = null,
                                            Mesaj = "Randevu Bitiş Tarihi Boş Olamaz."
                                        });
                                    }
                                }
                                else
                                {
                                    return Ok(new DataResult<string>()
                                    {
                                        Data = "null",
                                        Basarili = false,
                                        Hata = null,
                                        Mesaj = "Randevu Başlangıç Tarihi Boş Olamaz."
                                    });
                                }
                            }
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "null",
                                    Basarili = false,
                                    Hata = null,
                                    Mesaj = "Bu Poliklinik Türü Sistemimizde Bulunmamaktadır."
                                });
                            }
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "null",
                                Basarili = false,
                                Hata = null,
                                Mesaj = "Bu kuruma ait kayıt sistemimizde bulunmamaktadır."
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "İlgili KurumKodu sistemimizde bulunmamaktadır."
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "null",
                        Basarili = false,
                        Hata = null,
                        Mesaj = "Kurum Kodu Boş Olamaz."
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Sorgulama Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Randevu Durumu Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Randevu durumu bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen randevuya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("randevudurumuguncelle")]
        public IHttpActionResult RandevuDurumuGuncelle(RandevuDurumuGuncelleModel model)
        {
            try
            {
                string randevuID = "0";

                _db = new EKIPEntities();

                if (_db.RandevuBilgileri.Where(x => x.ID.Equals(model.RandevuID)).Any())
                {
                    if (!string.IsNullOrEmpty(model.RandevuDurumu))
                    {
                        RandevuBilgileri r = _db.RandevuBilgileri.Where(x => x.ID.Equals(model.RandevuID)).FirstOrDefault();
                        r.RandevuDurumu = model.RandevuDurumu;
                        _db.SaveChanges();

                        randevuID = (from x in _db.RandevuBilgileri where x.ID == model.RandevuID select x.ID).FirstOrDefault().ToString();

                        return Ok(new DataResult<string>()
                        {
                            Data = randevuID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Güncelleme Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Randevu Durumu Boş Olamaz!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Bu RandevuID sistemimizde bulunmamaktadır!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Acil Servis Vaka Bilgisi Eklenir
        /// </summary>
        /// <param name="model">Acil Servis Vaka Bilgisi Eklenir. Modelde yer alan alanlar zorunludur. Vaka kaydı bu bilgilere göre eklenecektir.  Metod, başarılı işlem sonucunda eklenen vaka bilgisine ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen hastaya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("acilservisvakabilgisiekle")]
        public IHttpActionResult AcilServisVakaBilgisiEkle(AcilServisVakaBilgisiEkleModel model)
        {
            try
            {
                string basvuruID = "0";

                _db = new EKIPEntities();

                AcilServisVakaBilgileri h = new AcilServisVakaBilgileri();

                if (!string.IsNullOrEmpty(model.HastaTCKimlikNo))
                    h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Hasta TC Kimlik Numarası Boş Olamaz."
                    });
                }

                bool kontrol = _db.Hastalar.Where(x => x.HastaTCKimlikNo.Equals(model.HastaTCKimlikNo)).Any();
                bool ekipNoKontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();
                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (kontrol == true && ekipNoKontrol == true)
                    {
                        if (!string.IsNullOrEmpty(model.HastaEkipNo))
                            h.HastaEkipNo = model.HastaEkipNo;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "HastaEkipNo Boş Olamaz."
                            });
                        }

                        if (model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.HastaAdi))
                            h.HastaAdi = model.HastaAdi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta Adı Boş Olamaz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.HastaSoyadi))
                            h.HastaSoyadi = model.HastaSoyadi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hasta Soyadı Boş Olamaz."
                            });
                        }

                        if (model.BasvuruTarihi != null)
                            h.BasvuruTarihi = model.BasvuruTarihi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Başvuru Tarihi Boş Olamaz."
                            });
                        }

                        if (model.HekimAdi != null)
                            h.HekimAdi = model.HekimAdi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Hekim Adı Boş Olamaz."
                            });
                        }

                        h.TaburculukTarihi = model.TaburculukTarihi;

                        if (model.GerceklestirilenIslemler != null)
                            h.GerceklestirilenIslemler = model.GerceklestirilenIslemler;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Gerçekleştirilen İşlemler Boş Olamaz."
                            });
                        }

                        h.TaburculukNotlari = model.TaburculukNotlari;

                        _db.AcilServisVakaBilgileri.Add(h);
                        _db.SaveChanges();

                        basvuruID = (from x in _db.AcilServisVakaBilgileri where x.HastaEkipNo == model.HastaEkipNo && x.BasvuruTarihi == model.BasvuruTarihi select x.ID).FirstOrDefault().ToString();

                        AcilServisVakaTanilari m = new AcilServisVakaTanilari();

                        StreamReader r = new StreamReader(model.ICDTaniKod);
                        //JObject json = JObject.Parse(model.ICDKodlari);

                        string json = r.ReadToEnd();

                        List<AcilServisVakaTanilari> items = JsonConvert.DeserializeObject<List<AcilServisVakaTanilari>>(json);

                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {
                            m.AcilServisVakaID = (int)item[basvuruID];
                            m.TaniKoduID = (int)item["TaniKoduID"];

                            _db.AcilServisVakaTanilari.Add(item);
                            _db.SaveChanges();
                        }

                        return Ok(new DataResult<string>()
                        {
                            Data = basvuruID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Kayıt Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu TC Kimlik Numarası/Hasta Ekip Numarası Sistemimizde Bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Tanı Kodları Sorgulama Metodu
        /// Modelde yer alan alanlar zorunludur ve hatasız girilmelidir. Tanı kodları buna göre sorgulanacaktır.
        /// </summary>
        /// <param name="KurumKodu">Kurum Kodu giriniz.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı ise işlem kuruma ait EKİP Projesi tarafındaki randevuya ait unique ID'yi ve bilgileri geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("tanikodusorgula")]
        public async Task<IHttpActionResult> TaniKoduSorgula(int KurumKodu)
        {
            try
            {
                _db = new EKIPEntities();

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(KurumKodu)).Any();

                int kurumid = _db.Kurumlar.Where(x => x.KurumKodu == KurumKodu).Select(x => x.KurumID).FirstOrDefault();

                if (KurumKodu != null && KurumKodu > 0)
                {
                    if (kurumkontrol)
                    {
                        List<TaniKoduSorgulaModel> tanilar = new List<TaniKoduSorgulaModel>();

                        tanilar = (from t in _db.TaniKodlari
                                   orderby t.ID
                                   select new TaniKoduSorgulaModel
                                   {
                                       ID = t.ID,
                                       ICDKodu = t.ICDKodu,
                                       Tani = t.Tani
                                   }).ToList();

                        if (tanilar != null)
                        {
                            return Ok(tanilar);
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "null",
                                Basarili = false,
                                Hata = null,
                                Mesaj = "Tanı kodu bulunamamıştır."
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "İlgili KurumKodu sistemimizde bulunmamaktadır."
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "null",
                        Basarili = false,
                        Hata = null,
                        Mesaj = "Kurum Kodu Boş Olamaz."
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Sorgulama Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Acil Servis Vaka Bilgisi Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Acil servis vaka bilgisi bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen hastaya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("acilservisvakabilgisiguncelle")]
        public IHttpActionResult AcilServisVakaBilgisiGuncelle(AcilServisVakaBilgisiGuncelleModel model)
        {
            try
            {
                string basvuruID = "0";

                _db = new EKIPEntities();

                if (model.BasvuruID != null && model.BasvuruID > 0)
                {
                    AcilServisVakaBilgileri h = _db.AcilServisVakaBilgileri.Where(x => x.ID == model.BasvuruID).FirstOrDefault();

                    if (!string.IsNullOrEmpty(model.HastaTCKimlikNo))
                        h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Hasta TC Kimlik Numarası Boş Olamaz."
                        });
                    }

                    bool kontrol = _db.Hastalar.Where(x => x.HastaTCKimlikNo.Equals(model.HastaTCKimlikNo)).Any();
                    bool ekipNoKontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();
                    bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                    if (model.KurumKodu != null && kurumkontrol == true)
                    {
                        if (kontrol == true && ekipNoKontrol == true)
                        {
                            if (!string.IsNullOrEmpty(model.HastaEkipNo))
                                h.HastaEkipNo = model.HastaEkipNo;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "HastaEkipNo Boş Olamaz."
                                });
                            }

                            if (model.KurumKodu != null && model.KurumKodu != 0)
                                h.KurumKodu = model.KurumKodu;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                                });
                            }

                            if (!string.IsNullOrEmpty(model.HastaAdi))
                                h.HastaAdi = model.HastaAdi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hasta Adı Boş Olamaz."
                                });
                            }

                            if (!string.IsNullOrEmpty(model.HastaSoyadi))
                                h.HastaSoyadi = model.HastaSoyadi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hasta Soyadı Boş Olamaz."
                                });
                            }

                            if (model.BasvuruTarihi != null)
                                h.BasvuruTarihi = model.BasvuruTarihi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Başvuru Tarihi Boş Olamaz."
                                });
                            }

                            if (model.HekimAdi != null)
                                h.HekimAdi = model.HekimAdi;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Hekim Adı Boş Olamaz."
                                });
                            }

                            h.TaburculukTarihi = model.TaburculukTarihi;

                            if (model.GerceklestirilenIslemler != null)
                                h.GerceklestirilenIslemler = model.GerceklestirilenIslemler;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "Gerçekleştirilen İşlemler Boş Olamaz."
                                });
                            }

                            h.TaburculukNotlari = model.TaburculukNotlari;

                            _db.SaveChanges();

                            AcilServisVakaTanilari m = _db.AcilServisVakaTanilari.Where(x => x.ID == model.ICDTaniKodID).FirstOrDefault();

                            StreamReader r = new StreamReader(model.ICDTaniKod);

                            string json = r.ReadToEnd();

                            List<AcilServisVakaTanilari> items = JsonConvert.DeserializeObject<List<AcilServisVakaTanilari>>(json);

                            dynamic array = JsonConvert.DeserializeObject(json);
                            foreach (var item in array)
                            {
                                m.AcilServisVakaID = item["AcilServisVakaID"].ToString();
                                m.TaniKoduID = (int)item["TaniKoduID"];

                                _db.HastaMaddeKullanimBilgileri.Add(item);
                                _db.SaveChanges();
                            }

                            basvuruID = model.BasvuruID.ToString();

                            return Ok(new DataResult<string>()
                            {
                                Data = basvuruID.ToString(),
                                Basarili = true,
                                Hata = null,
                                Mesaj = "Güncelleme Başarılı!"
                            });
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Bu TC Kimlik Numarası/Hasta Ekip Numarası Sistemimizde Bulunmamaktadır!"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Güncellenecek vakaya ait BasvuruID boş olamaz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Hasta Başvuru Kaydı Karar Kodları Kontrol Metodu
        /// </summary>
        /// <param name="KararKodu">KurumKodu alanı zorunludur ve hatasız girilmelidir. Poliklinik türleri buna göre kontrol edilecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı işlem kuruma ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("kararkodlarikontrol")]
        public IHttpActionResult KararKodlariKontrol(int KararKodu)
        {
            string karar = "";
            string kararAciklama = "";
            try
            {
                _db = new EKIPEntities();

                if (_db.ArindirmaBasvuruKararlari.Where(x => x.KararID == KararKodu).Count() != 0)
                {
                    karar = (from x in _db.ArindirmaBasvuruKararlari where x.KararID == KararKodu select x.Karar).FirstOrDefault().ToString();
                    kararAciklama = (from x in _db.ArindirmaBasvuruKararlari where x.KararID == KararKodu select x.Aciklama).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = "KararKodu:" + KararKodu.ToString() + " " + "Karar:" + karar.ToString() + " " + "Karar Açıklama:" + kararAciklama,
                        Basarili = true,
                        Hata = null,
                        Mesaj = "True. İlgili karar kodu sistemimizde bulunmaktadır. Karar bilgisi ve karar açıklaması verilmiştir."
                    });
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "null",
                        Basarili = false,
                        Hata = null,
                        Mesaj = "False. İlgili karar kodu sistemimizde bulunmamaktadır."
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kontrol Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// Hasta Yatış Kaydı Güncellenir
        /// </summary>
        /// <param name="model">Modelde yer alan alanlar zorunludur. Hasta yatış kaydı bu bilgilere göre güncellenecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen hasta yatış bilgisine ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastayatisguncelle")]
        public IHttpActionResult HastaYatisGuncelle(HastaYatisGuncelleModel model)
        {
            try
            {
                string yatisID = "0";

                _db = new EKIPEntities();

                Yatislar h = _db.Yatislar.Where(x => x.ID == model.YatisID).FirstOrDefault();

                bool kontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();

                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (kontrol == true)
                    {
                        if (!string.IsNullOrEmpty(model.HastaEkipNo))
                            h.HastaEkipNo = model.HastaEkipNo;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "HastaEkipNo Boş Olamaz."
                            });
                        }

                        if (model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        if (model.YatisTarihi != null)
                            h.YatisTarihi = model.YatisTarihi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Yatış Tarihi Boş Olamaz."
                            });
                        }

                        if (model.GunlukIzlemBilgisi != null)
                            h.GunlukIzlemBilgisi = model.GunlukIzlemBilgisi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Günlük İzlem Bilgisi Boş Olamaz."
                            });
                        }

                        h.YatisSonlanmaTarihi = model.YatisSonlanmaTarihi;

                        h.YatisSonlanmaID = model.YatisSonlanmaID;

                        h.YatisSonlanmaAciklama = model.YatisSonlanmaAciklama;

                        if (model.KararID != null && model.KararID != 0)
                        {
                            if (_db.ArindirmaBasvuruKararlari.Where(x => x.KararID.Equals(model.KararID)).Any())
                                h.KararID = model.KararID;
                            else
                            {
                                return Ok(new DataResult<string>()
                                {
                                    Data = "-1",
                                    Basarili = false,
                                    Mesaj = "İlgili KararID sistemimizde bulunmamaktadır!"
                                });
                            }
                        }
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "KararID Boş Olamaz!"
                            });
                        }

                        _db.SaveChanges();

                        yatisID = model.YatisID.ToString();

                        return Ok(new DataResult<string>()
                        {
                            Data = yatisID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Güncelleme Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu Hasta Ekip No Sistemimizde Bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// SMS Onam Kodu Gönderilir.
        /// </summary>
        /// <param name="HastaEkipNo">Hasta bilgilerinin Ekip Projesi ile paylaşılmasına dair SMS ile onam gönderme metodudur. Tüm parametreler doğru ve eksiksiz girilmelidir. Onam SMS'i buna göre gönderilecektir. </param>
        /// <param name="Telefon">Hasta bilgilerinin Ekip Projesi ile paylaşılmasına dair SMS ile onam gönderme metodudur. Tüm parametreler doğru ve eksiksiz girilmelidir. Onam SMS'i buna göre gönderilecektir. </param>
        /// <param name="KurumKodu">Hasta bilgilerinin Ekip Projesi ile paylaşılmasına dair SMS ile onam gönderme metodudur. Tüm parametreler doğru ve eksiksiz girilmelidir. Onam SMS'i buna göre gönderilecektir. </param>
        /// <param name="OnamIsteyenAdSoyad">Hasta bilgilerinin Ekip Projesi ile paylaşılmasına dair SMS ile onam gönderme metodudur. Tüm parametreler doğru ve eksiksiz girilmelidir. Onam SMS'i buna göre gönderilecektir. </param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda ilgili SMS_ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("smsonamkodugonder")]
        public IHttpActionResult SMSOnamKoduGonder(string HastaEkipNo, string Telefon, string KurumKodu, string OnamIsteyenAdSoyad)
        {
            int SMS_ID;
            return null;
        }

        /// <summary>
        /// SMS Onam Kodu Doğrulanır.
        /// </summary>
        /// <param name="SMS_ID">Onam için hastaya SMS ile gönderilen OnamKodu doğrulama metodudur. Doğrulama metodu başarılı işlem durumunda TRUE, hatalı işlem durumunda FALSE değerini geri döndürür.</param>     
        /// <param name="OnamKodu">Onam için hastaya SMS ile gönderilen OnamKodu doğrulama metodudur. Doğrulama metodu başarılı işlem durumunda TRUE, hatalı işlem durumunda FALSE değerini geri döndürür.</param> 
        /// <returns></returns>
        /// <response code="200">Doğrulama metodu başarılı işlem durumunda TRUE, hatalı işlem durumunda FALSE değerini geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("onamkodudogrula")]
        public IHttpActionResult OnamKoduDogrula(int SMS_ID, string OnamKodu)
        {
            return null;
        }

        /// <summary>
        /// Hasta onam durumu sorgulanır.
        /// </summary>
        /// <param name="HastaEkipNo">Hastaya ait onam durumu sorgulama metodudur. Hastadan veri paylaşımına dair onam alınmışsa metod, EkipOnamDurumu = TRUE değerini, onam alınmamışsa EkipOnamDurumu = FALSE değerini geri döndürür. HBYS, HastaEkipNo ve EkipOnamDurumu bilgilerini kendi bünyesinde kaydetmek zorundadır. İlgili HastaEkipNo'suna ait çalıştırılacak diğer tüm metodlar için öncelikle bu metod (HastaOnamSorgula) çalıştırılmalıdır. Eğer EkipOnamDurumu = TRUE ise hastadan tekrar onam sorgulaması yapılmamalı, onam durumu diğer tüm metodlar için aynı şekilde geçerli olacak şekilde tanımlanmalı ve diğer metodlarda gerekli olan bilgiler maskelenmemiş şekilde tarafımıza gönderilmelidir.</param>      
        /// <returns></returns>
        /// <response code="200">Hastadan veri paylaşımına dair onam alınmışsa metod, EkipOnamDurumu = TRUE değerini, onam alınmamışsa EkipOnamDurumu = FALSE değerini geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("hastaonamsorgula")]
        public IHttpActionResult HastaOnamSorgula(string HastaEkipNo)
        {
            return null;
        }

        /// <summary>
        ///  Hasta Kurum Randevu Kaydı Eklenir
        /// </summary>
        /// <param name="model">Hastaya ait kurum randevu bilgisi eklenir. Modelde yer alan alanlar zorunludur. Randevu bilgisi kaydı bu bilgilere göre oluşturulacaktır. Metod, başarılı işlem sonucunda yeni eklenen randevu kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda yeni eklenen randevuya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("kurumrandevudurumekle")]
        public IHttpActionResult KurumRandevuDurumEkle(KurumRandevuDurumEkleModel model)
        {
            try
            {
                string kurumRandevuID = "0";

                _db = new EKIPEntities();

                KurumRandevulari h = new KurumRandevulari();

                if (!string.IsNullOrEmpty(model.HastaEkipNo))
                    h.HastaEkipNo = model.HastaEkipNo;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "HastaEkipNo Boş Olamaz."
                    });
                }

                bool ekipNoKontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();
                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (ekipNoKontrol)
                    {
                        if (model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.RandevuTipi))
                            h.RandevuTipi = model.RandevuTipi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Randevu Tipi Boş Olamaz."
                            });
                        }

                        if (model.RandevuTarihSaati != null)
                            h.RandevuTarihSaati = model.RandevuTarihSaati;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "RandevuTarihSaati Boş Olamaz."
                            });
                        }

                        h.GeldiGelmedi = model.GeldiGelmedi;
                        h.KayitTarihi = DateTime.Now;

                        _db.KurumRandevulari.Add(h);
                        _db.SaveChanges();

                        kurumRandevuID = h.ID.ToString();
                        
                        return Ok(new DataResult<string>()
                        {
                            Data = kurumRandevuID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Kayıt Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu TC Kimlik Numarasına/Hasta Ekip Numarasına Ait Kayıt Bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Kayıt Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        ///  Hasta Kurum Randevu Kaydı Güncellenir
        /// </summary>
        /// <param name="model">Hastaya ait kurum randevu bilgisi güncellenir. Modelde yer alan alanlar zorunludur. Randevu bilgisi kaydı bu bilgilere göre güncellenecektir. Metod, başarılı işlem sonucunda güncellenen randevu kaydına ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</param>
        /// <returns></returns>
        /// <response code="200">Metod, başarılı işlem sonucunda güncellenen randevuya ait EKİP Projesi tarafındaki unique ID'yi geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("kurumrandevudurumguncelle")]
        public IHttpActionResult KurumRandevuDurumGuncelle(KurumRandevuDurumGuncelleModel model)
        {
            try
            {
                string kurumRandevuID = "0";

                _db = new EKIPEntities();

                KurumRandevulari h = _db.KurumRandevulari.Where(x => x.ID == model.RandevuID).FirstOrDefault();

                if (!string.IsNullOrEmpty(model.HastaEkipNo))
                    h.HastaEkipNo = model.HastaEkipNo;
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "HastaEkipNo Boş Olamaz."
                    });
                }

                bool ekipNoKontrol = _db.Hastalar.Where(x => x.HastaEkipNo.Equals(model.HastaEkipNo)).Any();
                bool kurumkontrol = _db.Kurumlar.Where(x => x.KurumKodu.Equals(model.KurumKodu)).Any();

                if (model.KurumKodu != null && kurumkontrol == true)
                {
                    if (ekipNoKontrol)
                    {
                        if (model.KurumKodu != null && model.KurumKodu != 0)
                            h.KurumKodu = model.KurumKodu;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Kurum Kodu Boş Olamaz veya Yanlış Değer Girdiniz."
                            });
                        }

                        if (!string.IsNullOrEmpty(model.RandevuTipi))
                            h.RandevuTipi = model.RandevuTipi;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "Randevu Tipi Boş Olamaz."
                            });
                        }

                        if (model.RandevuTarihSaati != null)
                            h.RandevuTarihSaati = model.RandevuTarihSaati;
                        else
                        {
                            return Ok(new DataResult<string>()
                            {
                                Data = "-1",
                                Basarili = false,
                                Mesaj = "RandevuTarihSaati Boş Olamaz."
                            });
                        }

                        h.GeldiGelmedi = model.GeldiGelmedi;

                        _db.SaveChanges();

                        kurumRandevuID = h.ID.ToString();

                        return Ok(new DataResult<string>()
                        {
                            Data = kurumRandevuID.ToString(),
                            Basarili = true,
                            Hata = null,
                            Mesaj = "Güncelleme Başarılı!"
                        });
                    }
                    else
                    {
                        return Ok(new DataResult<string>()
                        {
                            Data = "-1",
                            Basarili = false,
                            Mesaj = "Bu TC Kimlik Numarasına/Hasta Ekip Numarasına Ait Kayıt Bulunmamaktadır!"
                        });
                    }
                }
                else
                {
                    return Ok(new DataResult<string>()
                    {
                        Data = "-1",
                        Basarili = false,
                        Mesaj = "Lütfen Doğru Kurum Kodunu Girdiğinizden Emin Olunuz!"
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return Ok(new DataResult<string>()
                {
                    Data = "-1",
                    Basarili = false,
                    Hata = ex,
                    Mesaj = "Güncelleme Sırasında Hata Oluştu!" + ex.Message
                });
            }
        }

        /// <summary>
        /// KurumKodu'nuza göre kurumunuzdan EKİP yazılımına bildirebileceğiniz ICD kodların listesini döner.
        /// </summary>
        /// <param name="KurumKodu">"AcilServis" değeri TRUE/FALSE olarak sevis edilir, eğer TRUE ise acil servisten gelen hastaların gönderimini kabul eder. "Diger" değeri TRUE/FALSE olarak döner. TRUE değerinde Poliklinik vb.. alanlardan gelenleri kapsar.Eğer bir hastada size dönen ICD kodların haricinde bir tanı var ise kayıt metotları sırasında kabul edilmeyecektir. Bu metodu her gönderim öncesinde çalıştırmayınız. Günlük olarak geçici tabloda tutup hbys yazılımlarınız üzerinde sorgulayınız. Gün sonu tabloyu boşaltıp tekrardan yeni verileri çekiniz. Amaç; oluşabilecek network trafiğini azaltmaktır. NOT : Eğer dönen verilerde  ICDTaniKod paramtersinde "HEPSI" olarak dönerse (Şimdilik AMATEM ve CEMATEM) tüm tanı kodlarını kapsamaktadır.</param>      
        /// <returns></returns>
        /// <response code="200">Başarılı</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("icdkodusorgula")]
        public IHttpActionResult ICDKoduSorgula(string KurumKodu)
        {
            return null;
        }
    }
}