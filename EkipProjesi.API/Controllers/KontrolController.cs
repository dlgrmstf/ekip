using EkipProjesi.Data;
using EkipProjesi.API.Utils;
using System;
using System.Linq;
using System.Web.Http;
using static EkipProjesi.API.Models.Models;

namespace EkipProjesi.API.Controllers
{
    [RoutePrefix("api/kontrol")]
    public class KontrolController : ApiController
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        #endregion

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
        /// Kurum Kodu Kontrol
        /// </summary>
        /// <param name="KurumKodu">KurumKodu zorunludur ve hatasız girilmelidir. Kurum bu bilgilere göre kontrol edilecektir.</param>
        /// <returns></returns>
        /// <response code="200">Metod başarılı ise işlem kuruma ait EKİP Projesi tarafındaki kuruma ait unique ID'yi ve kurum bilgilerini geri döndürür.</response>
        /// <response code="400">Geçersiz istek</response>
        /// <response code="500">Sunucuya bağlanılamadı</response>
        [Authorize]
        [HttpPost, Route("kurumkodukontrol")]
        public IHttpActionResult KurumKoduKontrol(int KurumKodu)
        {
            string kurumID = "0";
            string kurumAdi = "";
            try
            {
                _db = new EKIPEntities();

                if (_db.Kurumlar.Where(x => x.KurumKodu == KurumKodu).Count() != 0)
                {
                    kurumID = (from x in _db.Kurumlar where x.KurumKodu == KurumKodu select x.KurumID).FirstOrDefault().ToString();
                    kurumAdi = (from x in _db.Kurumlar where x.KurumKodu == KurumKodu select x.KurumAdi).FirstOrDefault().ToString();

                    return Ok(new DataResult<string>()
                    {
                        Data = "Kurum ID:" + kurumID.ToString() + " " + "Kurum Adı:" + kurumAdi,
                        Basarili = true,
                        Hata = null,
                        Mesaj = "True. İlgili kurum kodu sistemimizde bulunmaktadır."
                    });
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
    }
}