﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace memepowermachine.Controllers
{
    public class HomeController : Controller
    {
        public string paginaSiguiente;
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
			
            WebClient client = new WebClient();
			String downloadString = client.DownloadString("https://9gag.com/");
            int inicio_del_data_entry_url = downloadString.IndexOf("data-entry-url");

            int indice = inicio_del_data_entry_url;
            while(downloadString[indice] != ' ') {
                indice++;
            }
            int fin_del_data_entre_url = indice;
            string url_del_siguiente_post = downloadString.Substring(inicio_del_data_entry_url, fin_del_data_entre_url - inicio_del_data_entry_url + 1);
            url_del_siguiente_post = url_del_siguiente_post.Replace("data-entry-url", "");
            url_del_siguiente_post = url_del_siguiente_post.Replace("\"", "");
            url_del_siguiente_post = url_del_siguiente_post.Replace("=", "");
            url_del_siguiente_post = url_del_siguiente_post.Replace("\n", "");
            ViewData["url_del_siguiente_post"] = url_del_siguiente_post;

			ViewData["contenido_mostrado"] = client.DownloadString(url_del_siguiente_post);


			return View();
        }

        public ActionResult SiguienteMeme(string paginaSiguiente) {
			WebClient client = new WebClient();
            paginaSiguiente = paginaSiguiente.Replace("https://9gag.com/gag/", "");
            String downloadString = client.DownloadString("https://9gag.com/gag/" + paginaSiguiente);
			int inicio_del_data_entry_url = downloadString.IndexOf("data-entry-key");
            int indice = inicio_del_data_entry_url;
			while (downloadString[indice] != ' ')
			{
				indice++;
			}
			int fin_del_data_entre_url = indice;
			string url_del_siguiente_post = downloadString.Substring(inicio_del_data_entry_url, fin_del_data_entre_url - inicio_del_data_entry_url + 1);
			url_del_siguiente_post = url_del_siguiente_post.Replace("data-entry-key", "");
			url_del_siguiente_post = url_del_siguiente_post.Replace("\"", "");
			url_del_siguiente_post = url_del_siguiente_post.Replace("=", "");
			url_del_siguiente_post = url_del_siguiente_post.Replace("\n", "");
			ViewData["url_del_siguiente_post"] = url_del_siguiente_post;
			ViewData["contenido_mostrado"] = client.DownloadString("https://9gag.com/gag/" + url_del_siguiente_post);
			return View("Index");

		}
    }
}
