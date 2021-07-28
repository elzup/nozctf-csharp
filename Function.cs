using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

namespace nozctf_csharp {
  public class Function: IHttpFunction {
    /// <summary>
    /// Logic for your function goes here.
    /// </summary>
    /// <param name="context">The HTTP context, containing the request and the response.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(HttpContext context) {
      HttpRequest request = context.Request;
      HttpResponse response = context.Response;

      string pattern = context.Request.Body.AsString();
      string sentence = "X CAN YOU SEE ANYTHING Q";

      try {
        foreach(Match match in Regex.Matches(sentence, pattern,
          RegexOptions.None,
          TimeSpan.FromSeconds(1))) {
          await response.WriteAsync(match.Value);
        }
      } catch (RegexMatchTimeoutException) {
        string secret = Environment.GetEnvironmentVariable("KEY_Q7") ?? "";

        await response.WriteAsync($"Found 'FLAG_{secret}'");
      }
    }
  }
}