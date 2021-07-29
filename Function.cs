using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.IO;

namespace nozctf_csharp
{
	public class Function : IHttpFunction
	{

		static string QTEXT = "X CAN YOU SEE ANYTHING Q";
		static string SECRET_PARTS = Environment.GetEnvironmentVariable("KEY_Q7") ?? "";
		public static string search(string pattern)
		{
			if (pattern.Length > 10) return "Error! too long";

			try {
				Match m = Regex.Match(QTEXT, pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
				if (m.Success) {
					return $"Find! {m.Value}";
				} else {
					return "No Hit";
				}
			}
			catch (RegexMatchTimeoutException) {
				return $"TIMEOUT! 'FLAG_{SECRET_PARTS}'";
			}
		}

		/// <summary>
		/// Logic for your function goes here.
		/// </summary>
		/// <param name="context">The HTTP context, containing the request and the response.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task HandleAsync(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			using TextReader reader = new StreamReader(request.Body);


			string pattern = await reader.ReadLineAsync();
			await response.WriteAsync(search(pattern));


		}
	}
}