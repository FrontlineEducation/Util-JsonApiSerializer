﻿using System;
using System.Web;

namespace UtilJsonApiSerializer.Serialization
{
    public class UrlBuilder : IUrlBuilder
    {
        private string routePrefix = string.Empty;
        private string root = string.Empty;

        public string RoutePrefix
        {
            set { routePrefix = value; }
            get
            {
                if (!string.IsNullOrWhiteSpace(routePrefix))
                {
                    return routePrefix;
                }

                return string.Empty;
            }
        }

        public string Url
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    Uri url = HttpContext.Current.Request.Url;
                    root = url.Scheme + "://" + url.Authority + HttpContext.Current.Request.ApplicationPath;
                    if (!root.EndsWith("/")) root += "/";
                }
                return root;
            }
        }

        public string GetFullyQualifiedUrl(string urlTemplate)
        {
            try
            {
                if (String.IsNullOrEmpty(Url))
                {
                    if (urlTemplate.StartsWith("//"))
                        return new Uri(RoutePrefix + urlTemplate.TrimStart('/')).ToString();

                    return new Uri(RoutePrefix + urlTemplate).ToString();
                }
            }
            catch(Exception debugException)
            {
                int test = 1;
                throw;
            }

            Uri fullyQualiffiedUrl;

            if (Uri.TryCreate(urlTemplate, UriKind.Absolute, out fullyQualiffiedUrl))
                return fullyQualiffiedUrl.ToString();

            if (!Uri.TryCreate(new Uri(Url), new Uri(RoutePrefix + urlTemplate, UriKind.Relative), out fullyQualiffiedUrl))
                throw new ArgumentException(string.Format("Unable to create fully qualified url for urltemplate = '{0}'", urlTemplate));

            return fullyQualiffiedUrl.ToString();
        }
    }
}