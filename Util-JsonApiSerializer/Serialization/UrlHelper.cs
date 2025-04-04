﻿#if NETCOREAPP
using Microsoft.AspNetCore.Http;
#else
#endif
using System;
using System.Linq;
using System.Web;

namespace UtilJsonApiSerializer.Serialization
{
    public class UrlBuilder : IUrlBuilder
    {
        private string routePrefix = string.Empty;
        private string root = string.Empty;


        public UrlBuilder()
        {
        }
#if NETCOREAPP
        private readonly IHttpContextAccessor _accessor;
        public UrlBuilder(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
#else
#endif

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


#if NETCOREAPP
        public string Url
        {
            get
            {
                if (!string.IsNullOrEmpty(routePrefix))
                {
                    root = routePrefix;
                }
                else
                {
                    var context = _accessor.HttpContext;
                    if (context != null)
                    {
                        if (routePrefix == string.Empty)
                        {
                            Uri url =new Uri(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request));
                            var scheme = url.Scheme;
                            if (context.Request.Headers["X-Forwarded-Proto"].Any())
                            {
                                scheme = context.Request.Headers["X-Forwarded-Proto"];
                            }
                            root = scheme + "://" + url.Authority + context.Request.PathBase;
                        }
                    }

                }

                if (!root.EndsWith("/")) root += "/";
                return root;
            }
        }
#else
        public string Url
        {
            get
            {
                if (!string.IsNullOrEmpty(routePrefix))
                {
                    root = routePrefix;
                }
                else
                {
                    
                    if (HttpContext.Current != null)
                    {
                        if (routePrefix == string.Empty)
                        {
                            Uri url = HttpContext.Current.Request.Url;
                            var scheme = url.Scheme;
                            if (HttpContext.Current.Request.Headers["X-Forwarded-Proto"] != null)
                            {
                                scheme = HttpContext.Current.Request.Headers["X-Forwarded-Proto"];
                            }
                            root = scheme + "://" + url.Authority + HttpContext.Current.Request.ApplicationPath;
                        }
                    }
                }
             
                if (!root.EndsWith("/")) root += "/";
                return root;
            }
        }
#endif
        public string GetFullyQualifiedUrl(string urlTemplate)
        {
            if (String.IsNullOrEmpty(Url))
            {
                if (urlTemplate.StartsWith("//"))
                    return new Uri(RoutePrefix + urlTemplate.TrimStart('/')).ToString();

                return new Uri(RoutePrefix + urlTemplate).ToString();
            }

            Uri fullyQualiffiedUrl;

            //try to create an absolute URL from the urlTemplate
            if (Uri.TryCreate(urlTemplate, UriKind.Absolute, out fullyQualiffiedUrl) && (fullyQualiffiedUrl.Scheme == Uri.UriSchemeHttp || fullyQualiffiedUrl.Scheme == Uri.UriSchemeHttps))
                return fullyQualiffiedUrl.ToString();

            //try to create an absolute url from the routeprefix + urltemplate
            if (Uri.TryCreate(RoutePrefix + urlTemplate, UriKind.Absolute, out fullyQualiffiedUrl))
                return fullyQualiffiedUrl.ToString();

            if (!Uri.TryCreate(new Uri(Url), new Uri(RoutePrefix + urlTemplate, UriKind.Relative), out fullyQualiffiedUrl))
                throw new ArgumentException(string.Format("Unable to create fully qualified url for urltemplate = '{0}'", urlTemplate));

            return fullyQualiffiedUrl.ToString();
        }
    }
}