﻿using System;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ginger.Rpc
{
  public class JsonRpcClient
  {
    HttpClient _cli;
    string _endpoint;

    public Tuple<string,string> Authorization { get; set; }

    public JsonRpcClient(string endpoint)
    {
      Debug.Assert(endpoint != null);
      _endpoint = endpoint;
      _cli = new HttpClient();
    }

    string CreateRequestBody(string method, JObject args)
    {
      JObject jobj = new JObject();

      jobj["jsonrpc"] = "2.0";
      jobj["method"] = method;
      if (args != null)
        jobj["params"] = args;
      jobj["id"] = 1;

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
      var json = await res.Content.ReadAsStringAsync();
      JObject obj = JObject.Parse(json);
      // TODO: エラー処理
      return obj["result"];
    }

    public async Task<T> InvokeAsync<T>(string method, JObject args = null)
    {
      var req = CreateRequestMessage(method, args);
      var res = await _cli.SendAsync(req);
      var json = await res.Content.ReadAsStringAsync();
      JObject obj = JObject.Parse(json);
      return JsonConvert.DeserializeObject<T>(obj["result"].ToString());
    }

  }

}
