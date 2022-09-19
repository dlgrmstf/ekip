using EkipProjesi.Core.Kullanici;
using EkipProjesi.Data;
using EkipProjesi.Data.Repositories;
using EkipProjesi.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EkipProjesi.Core.LLog;
using SmsApiNode;

namespace EkipProjesi.Web.Controllers
{
    [AllowAnonymous]
    public class HesapController : Controller
    {
        #region Const
        private HesapRepository _hesapRepo;
        private KullaniciRepository _kullaniciRepo;
        private LogRepository _logRepo;
        private SMSRepository _smsRepo;
        private readonly EKIPEntities _db;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HesapController()
        {
            _hesapRepo = new HesapRepository();
            _kullaniciRepo = new KullaniciRepository();
            _logRepo = new LogRepository();
            _smsRepo = new SMSRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            if (TempData["Mesaj"] != null)
            {
                ViewBag.Msg = TempData["Mesaj"].ToString();
                TempData["Mesaj"] = null;
            }
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IKullanici model)
        {            
            Session["Cikis"] = false;
            if (model.Log == null) model.Log = new ILogLogin();
            Session["SessionID"] = Guid.NewGuid().ToString();
            model.Log.SessionID = Session["SessionID"].ToString();
            string ip = "";
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            if (!string.IsNullOrEmpty(ip))
            {
                model.Log.IP = ip;
            }
            else
            {
                if (Request.ServerVariables["HTTP_CLIENT_ADDRESS"] != null)
                {
                    ip = Request.ServerVariables["HTTP_CLIENT_ADDRESS"].ToString();
                }
                if (!string.IsNullOrEmpty(ip))
                {
                    model.Log.IP = ip;
                }
                else
                {
                    if (Request.ServerVariables["REMOTE_ADDR"] != null)
                    {
                        model.Log.IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
            }

            model.Log.IsletimSistemi = Request.UserAgent;
            try
            {
                if (Request.Browser != null)
                {
                    model.Log.TarayiciAdi = Request.Browser.Browser + " " + Request.Browser.Version;
                    if (Request.Browser.IsMobileDevice)
                    {
                        model.Log.MobilMi = "Evet - " + Request.Browser.MobileDeviceManufacturer + " - " + Request.Browser.MobileDeviceModel;
                    }
                    else
                    {
                        model.Log.MobilMi = "Hayır";
                    }
                }
            }
            catch   { }

            string mesaj = "";
            if (Request.ServerVariables["LOCAL_ADDR"] != null)
            {
                model.Log.MachineIP = Request.ServerVariables["LOCAL_ADDR"];
            }
            model.Log.MachineName = Environment.MachineName;
            IKullanici outuser = new IKullanici();
            if (_kullaniciRepo.KullaniciGiris(model, out outuser, out mesaj))
            {
                try
                {
                    UserCookie co = new UserCookie();
                    co.Email = outuser.KullaniciAdi;
                    co.ID = outuser.ID;
                    co.Name = outuser.Ad;
                    co.PersonelID = outuser.PersonelId.GetValueOrDefault();
                    co.SessionID = Session["SessionID"].ToString();
                    co.MachineIP = model.Log.MachineIP;
                    string userData = JsonConvert.SerializeObject(co);

                    FormsAuthentication.SetAuthCookie(outuser.KullaniciAdi, true);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        outuser.KullaniciAdi,
                        DateTime.Now,
                        DateTime.Now.AddDays(1),
                        false,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);


                    HttpCookie authCookie4 = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Set(authCookie4);
                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                    HttpCookie httpCookieDateTime = new HttpCookie("Date", string.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now));
                    HttpCookie serverip = new HttpCookie("info", "IP: " + co.MachineIP);
                    Response.Cookies.Add(httpCookieDateTime);
                    Response.Cookies.Add(serverip);
                    Request.Cookies.Add(serverip);
                }
                catch (Exception)
                {

                }
                Session["UserID"] = outuser.ID;
                log4net.ThreadContext.Properties["userid"] = Session["UserID"].ToString();
                Session["SifreKontrol"] = outuser.SifreKontrol;
                string foto = "";
                try
                {
                    foto = Convert.ToBase64String(outuser.FotoByte);
                }
                catch (Exception)
                {
                    foto = "";
                }
                if (outuser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, outuser.ID.ToString()),
                        new Claim(ClaimTypes.Name, outuser.Ad),
                        //new Claim("ContentType", outuser.FotoContentType),
                        new Claim(ClaimTypes.Email, outuser.KullaniciAdi),
                        new Claim("TC", outuser.TC),
                        new Claim("PersonelId",outuser.PersonelId.ToString()),
                    };

                    foreach (var r in outuser.Roller)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, r.RolEnum.ToString(), ClaimValueTypes.String));
                    }

                    try
                    {
                        var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(5)
                        }, identity);
                    }
                    catch { }

                    var user = User as ClaimsPrincipal;
                    Session["Roller"] = outuser.Roller;
                    Session["PhotoByte"] = foto;
                    Session["Photo"] = outuser.FotoByte;
                    Session["PhotoContent"] = outuser.FotoContentType;
                    Session["PersonelID"] = outuser.PersonelId;
                    Session["PersonelSlug"] = outuser.PersonelSlug;
                    Session["Isletmeler"] =null;
                    Session["PPPID"] = null; 
                    Session["IsletmeID"] = null;
                    Session["UserID"] = outuser.ID;
                    Session["TipID"] = outuser.RolID;
                    Session["UserName"] = outuser.TC;
                    Session["KurumID"] = model.KurumID;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Cikis", "Hesap");
                }
            }
            else
            {
                model.error = "Error";
                model.error_description = mesaj;
                model.Sifre = "";
                return View(model);
            }
        }
        public ActionResult Cikis()
        {

            HttpCookie cmachineName = new HttpCookie("MachineName");
            HttpCookie cmachineIP = new HttpCookie("MachineIP");
            int sessionCount = -1;
            string machineName = Environment.MachineName;
            string ipAddress = "";
            string UserData = "";
            UserCookie co = new UserCookie();
            try
            {
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                if (Request.Cookies["MachineName"] != null)
                {
                    cmachineName = Request.Cookies["MachineName"];
                }
                if (Request.Cookies["MachineIP"] != null)
                {
                    cmachineIP = Request.Cookies["MachineIP"];
                }

                if (Session != null)
                {
                    sessionCount = Session.Count;
                }

                if (Request.ServerVariables["LOCAL_ADDR"] != null)
                {
                    ipAddress = Request.ServerVariables["LOCAL_ADDR"];
                }
                if (cmachineName != null && !string.IsNullOrEmpty(cmachineName.Value))
                {
                    machineName = cmachineName.Value;
                }
                if (cmachineIP != null && !string.IsNullOrEmpty(cmachineIP.Value))
                {
                    ipAddress = cmachineIP.Value;
                }

                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    UserData = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData;
                    log.Info(UserData);

                    co = JsonConvert.DeserializeObject<UserCookie>(UserData);
                    if (!string.IsNullOrEmpty(UserData) && co.ID > 0)
                    {
                        _logRepo.LogCikis(co.ID, co.SessionID, sessionCount, machineName, ipAddress);
                    }
                    else
                    {
                        log.Info("Request Auth Cookie Tanımsız.");
                        _logRepo.LogCikis(0, "", sessionCount, machineName, ipAddress);
                    }
                }
                else
                {
                    if (Response.Cookies[FormsAuthentication.FormsCookieName] != null && !string.IsNullOrEmpty(Response.Cookies[FormsAuthentication.FormsCookieName].Value))
                    {
                        authCookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        UserData = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData;
                        log.Info(UserData);

                        co = JsonConvert.DeserializeObject<UserCookie>(UserData);
                        if (!string.IsNullOrEmpty(UserData) && co.ID > 0)
                        {
                            _logRepo.LogCikis(co.ID, co.SessionID, sessionCount, machineName, ipAddress);
                        }
                        else
                        {
                            log.Info("Response Auth Cookie Tanımsız.");
                            _logRepo.LogCikis(0, "", sessionCount, machineName, ipAddress);
                        }
                    }
                    if (Session != null && sessionCount > 0 && Session["UserID"] != null && Session["SessionID"].ToString() != null)
                    {
                        _logRepo.LogCikis((int)Session["UserID"], Session["SessionID"].ToString(), sessionCount, machineName, ipAddress);
                    }
                    else
                    {
                        log.Info("Request ve Response Auth Cookie Tanımsız. Session Count: 0");
                        _logRepo.LogCikis(0, "", sessionCount, machineName, ipAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepo.LogCikis(0, "", sessionCount, machineName, ipAddress);
                log.Error(ex);
            }

            try
            {
                //HttpCookie c = Request.Cookies[FormsAuthentication.FormsCookieName];
                //if (c != null)
                //{
                //    c.Expires = DateTime.Now.AddDays(-1);
                //}
                foreach (DictionaryEntry entry in HttpContext.Cache)
                {
                    HttpContext.Cache.Remove((string)entry.Key);
                }
                Request.Cookies.Clear();
                Response.Cookies.Clear();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }
            catch { }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            Session["Cikis"] = true;
            return RedirectToAction("Index");
        }
        public ActionResult SifreYenile(string kullaniciAdi)
        {
            ViewBag.KullaniciAdi = kullaniciAdi;
            return View();
        }
        [HttpPost]
        public JsonResult SMSOnayKoduGonder(string KullaniciAdi)
        {
            if (!string.IsNullOrEmpty(KullaniciAdi))
            {
                if (_smsRepo.SMSOnayKoduGonder(KullaniciAdi))
                    return Json(true);
                else
                    return Json(false);
            }
            else
                return Json(false);
        }
        [HttpPost]
        public JsonResult SifreGuncelle(IKullanici model)
        {
            if (model != null)
            {
                if (_kullaniciRepo.SifreYenile(model))
                {
                    ILogLogin log = new ILogLogin();
                    if (Request.ServerVariables["LOCAL_ADDR"] != null)
                    {
                        log.MachineIP = Request.ServerVariables["LOCAL_ADDR"];
                    }
                    log.MachineName = Environment.MachineName;
                    log.KullaniciId = _db.Kullanicilar.Where(x => x.KullaniciAdi == model.KullaniciAdi).Select(x => x.ID).FirstOrDefault();
                    log.LogTipi = Core.LogTipi.YeniSifre;
                    log.SessionID = "-";
                    string ip = "";
                    if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ip))
                    {
                        log.IP = ip;
                    }
                    else
                    {
                        if (Request.ServerVariables["HTTP_CLIENT_ADDRESS"] != null)
                        {
                            ip = Request.ServerVariables["HTTP_CLIENT_ADDRESS"].ToString();
                        }
                        if (!string.IsNullOrEmpty(ip))
                        {
                            log.IP = ip;
                        }
                        else
                        {
                            if (Request.ServerVariables["REMOTE_ADDR"] != null)
                            {
                                log.IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
                            }
                        }
                    }
                    log.IsletimSistemi = Request.UserAgent;
                    try
                    {
                        if (Request.Browser != null)
                        {
                            log.TarayiciAdi = Request.Browser.Browser + " " + Request.Browser.Version;
                            if (Request.Browser.IsMobileDevice)
                            {
                                log.MobilMi = "Evet - " + Request.Browser.MobileDeviceManufacturer + " - " + Request.Browser.MobileDeviceModel;
                            }
                            else
                            {
                                log.MobilMi = "Hayır";
                            }
                        }
                    }
                    catch { }
                    _logRepo.LogKayit(log);
                    LogSifreDegistirmeModel sd = new LogSifreDegistirmeModel();
                    sd.IP = log.IP;
                    sd.SessionID = log.SessionID;
                    sd.Tarih = DateTime.Now;
                    sd.UserID = _db.Kullanicilar.Where(x=>x.KullaniciAdi == model.KullaniciAdi).Select(x=>x.ID).FirstOrDefault();
                    _logRepo.SifreDegistirme(sd);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}