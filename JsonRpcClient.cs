using System;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace ginger
{
  public class JsonRpcClient
  {
    readonly HttpClient _cli;
    string _endpoint;

    public Tuple<string,string> Authorization { get; set; }

    public JsonRpcClient(string endpoint)
    {
      Debug.Assert(endpoint != null);
      _endpoint = endpoint;
      // 一番応答の遅いAPIだろうペカステの帯域測定が10秒なので、15秒あれば十分じゃないかと思う。
      _cli = new HttpClient() { Timeout = TimeSpan.FromSeconds(15.0) };
    }

    string CreateRequestBody(string method, JObject args)
    {
      JObject jobj = new JObject();

      jobj["jsonrpc"] = "2.0";
      jobj["method"] = method;
      if (args != null)
        jobj["params"] = args;
      jobj["id"] = 1;

      Debug.Print(jobj.ToString());
      
      return jobj.ToString();
    }

    HttpRequestMessage CreateRequestMessage(string method, JObject args)
    {
      var req = new HttpRequestMessage(HttpMethod.Post, _endpoint);
      req.Content = new StringContent(CreateRequestBody(method, args));
      if (Authorization != null) {
        req.Headers.Add("Authorization", CreateBasicAuthorizationHeader(Authorization.Item1, Authorization.Item2));
      }
      req.Headers.Add("X-Requested-With", "XMLHttpRequest");
      return req;
    }

    string CreateBasicAuthorizationHeader(string id, string pwd)
    {
      return "BASIC " + Convert.ToBase64String(Encoding.ASCII.GetBytes(id + ":" + pwd));
    }

    public async Task<JToken> InvokeAsync(string method, JObject args = null)
    {
      var req = CreateRequestMessage(method, args);
      var res = await _cli.SendAsync(req);
      if (res.StatusCode == HttpStatusCode.Unauthorized)
        throw new UnauthorizedException(res.Headers.WwwAuthenticate.ToString());
      else if (res.StatusCode != HttpStatusCode.OK)
        throw new Exception($"Server responded {res.StatusCode}");
      var json = await res.Content.ReadAsStringAsync();
      Debug.Print(json);
      JObject obj = JObject.Parse(json);
      if (obj["error"] != null) {
        throw new Exception(obj["error"]["message"] + " (Error code: " + obj["error"]["code"] + ")");
      }
      else {
        return obj["result"];
      }
    }

    public async Task<T> InvokeAsync<T>(string method, JObject args = null)
    {
      var result = await InvokeAsync(method, args);
      return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(result));
    }

    public class UnauthorizedException : Exception
    {
      public UnauthorizedException (string message) : base(message) {}
    }
  }

}
