using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EkipProjesi.Web.Filter
{
    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;
            //string acceptEncoding = request.Headers["Accept-Encoding"];

            //if (string.IsNullOrEmpty(acceptEncoding)) return;

            //acceptEncoding = acceptEncoding.ToUpperInvariant();

            //try
            //{
            //    if (acceptEncoding.Contains("GZIP"))
            //    {
            //        response.AppendHeader("Content-encoding", "gzip");
            //        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            //    }
            //    else if (acceptEncoding.Contains("DEFLATE"))
            //    {
            //        response.AppendHeader("Content-encoding", "deflate");
            //        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            //    }
            //}
            //catch
            //{

            //}
            response.AppendHeader("Vary", "Content-Encoding");
        }
    }

    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string enviroment = ConfigurationSettings.AppSettings["Environment"].ToString();
            if (!string.IsNullOrEmpty(enviroment) && enviroment.ToLower() == "prod")
            {
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;

                response.Filter = new WhiteSpaceFilter(response.Filter, s =>
                {
                    s = Regex.Replace(s, @"\s+", " ");
                    s = Regex.Replace(s, @"\s*\n\s*", "\n");
                    s = Regex.Replace(s, @"\s*\>\s*\<\s*", "><");
                    s = Regex.Replace(s, @"<!--(.*?)-->", "");   //Remove comments

                    // single-line doctype must be preserved 
                    var firstEndBracketPosition = s.IndexOf(">");
                    if (firstEndBracketPosition >= 0)
                    {
                        s = s.Remove(firstEndBracketPosition, 1);
                        s = s.Insert(firstEndBracketPosition, ">");
                    }
                    return s;
                });
            }
        }

        public class WhiteSpaceFilter : Stream
        {

            private Stream _shrink;
            private Func<string, string> _filter;

            public WhiteSpaceFilter(Stream shrink, Func<string, string> filter)
            {
                _shrink = shrink;
                _filter = filter;
            }


            public override bool CanRead { get { return true; } }
            public override bool CanSeek { get { return true; } }
            public override bool CanWrite { get { return true; } }
            public override void Flush() { _shrink.Flush(); }
            public override long Length { get { return 0; } }
            public override long Position { get; set; }
            public override int Read(byte[] buffer, int offset, int count)
            {
                return _shrink.Read(buffer, offset, count);
            }
            public override long Seek(long offset, SeekOrigin origin)
            {
                return _shrink.Seek(offset, origin);
            }
            public override void SetLength(long value)
            {
                _shrink.SetLength(value);
            }
            public override void Close()
            {
                _shrink.Close();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                // capture the data and convert to string 
                byte[] data = new byte[count];
                Buffer.BlockCopy(buffer, offset, data, 0, count);
                string s = Encoding.UTF8.GetString(buffer);

                // filter the string
                s = _filter(s);

                // write the data to stream 
                byte[] outdata = Encoding.Default.GetBytes(s);
                _shrink.Write(outdata, 0, outdata.GetLength(0));
            }
        }
        //private class HelperClass : Stream
        //{
        //    private readonly Stream _base;
        //    StringBuilder _s = new StringBuilder();

        //    public HelperClass(Stream responseStream)
        //    {
        //        if (responseStream == null)
        //            throw new ArgumentNullException("responseStream");
        //        _base = responseStream;
        //    }

        //    public override void Write(byte[] buffer, int offset, int count)
        //    {
        //        var html = Encoding.UTF8.GetString(buffer, offset, count);
        //        var reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");
        //        html = reg.Replace(html, string.Empty);

        //        buffer = Encoding.UTF8.GetBytes(html);
        //        _base.Write(buffer, 0, buffer.Length);
        //    }

        //    #region Other Members

        //    public override int Read(byte[] buffer, int offset, int count)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    public override bool CanRead { get { return false; } }

        //    public override bool CanSeek { get { return false; } }

        //    public override bool CanWrite { get { return true; } }

        //    public override long Length { get { throw new NotSupportedException(); } }

        //    public override long Position
        //    {
        //        get { throw new NotSupportedException(); }
        //        set { throw new NotSupportedException(); }
        //    }

        //    public override void Flush()
        //    {
        //        _base.Flush();
        //    }

        //    public override long Seek(long offset, SeekOrigin origin)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    public override void SetLength(long value)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    #endregion
        //}
    }
}