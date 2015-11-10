﻿using NJsonApi.HelloWorld.Common.Models;

namespace NJsonApi.HelloWorld.Common
{
    public static class NJsonApiConfiguration
    {
        public static Configuration BuildConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder
                .Resource<World>()
                .WithAllProperties()
                .WithLinkTemplate("/worlds/{id}");

            configBuilder
                .Resource<Continent>()
                .WithAllProperties()
                .WithLinkTemplate("/continents/{id}");

            var nJsonApiConfig = configBuilder.Build();
            return nJsonApiConfig;
        }
    }
}
