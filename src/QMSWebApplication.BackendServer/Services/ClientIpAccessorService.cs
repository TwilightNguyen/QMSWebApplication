using System.Net;

namespace QMSWebApplication.BackendServer.Services
{

    public interface IClientIpAccessorService
    {
        /// <summary>Lấy IP client dạng chuỗi (ưu tiên IPv4 nếu mapped), trả null nếu không có.</summary>
        string? GetClientIp(); 
    }


    public class ClientIpAccessorService : IClientIpAccessorService
    {


        private readonly IHttpContextAccessor _http;
        private readonly bool _preferXff;

        public ClientIpAccessorService(IHttpContextAccessor http, bool preferXff = false)
        {
            _http = http;
            _preferXff = preferXff;
        }

        public string? GetClientIp()
        {
            var ctx = _http.HttpContext;
            if (ctx == null) return null;

            // 1) Ưu tiên Cloudflare nếu có (đáng tin)
            var cf = ctx.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(cf) && IPAddress.TryParse(cf, out var cfAddr))
                return Normalize(cfAddr).ToString();

            // 2) Nếu được cấu hình, ưu tiên X-Forwarded-For (chỉ dùng khi proxy đáng tin)
            if (_preferXff)
            {
                var xff = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(xff))
                {
                    var first = xff.Split(',').Select(s => s.Trim()).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(first) && IPAddress.TryParse(first, out var xffAddr))
                        return Normalize(xffAddr).ToString();
                }
            }

            // 3) Mặc định: RemoteIpAddress (đã đúng nếu UseForwardedHeaders() hoạt động)
            var addr = ctx.Connection.RemoteIpAddress;
            if (addr != null) return Normalize(addr).ToString();

            // 4) Fallback cuối cùng: nếu chưa có gì, mới thử XFF
            var xff2 = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(xff2))
            {
                var first = xff2.Split(',').Select(s => s.Trim()).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(first) && IPAddress.TryParse(first, out var xffAddr2))
                    return Normalize(xffAddr2).ToString();
            }

            return null;
        }

        private static IPAddress Normalize(IPAddress ip)
        {
            if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
            if (IPAddress.IsLoopback(ip)) return IPAddress.Parse("127.0.0.1");
            return ip;
        }

    }
}
