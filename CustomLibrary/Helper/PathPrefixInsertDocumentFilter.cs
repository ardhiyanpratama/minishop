﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace CustomLibrary.Helper
{
    public class PathPrefixInsertDocumentFilter : IDocumentFilter
    {
        private readonly string _pathPrefix;

        public PathPrefixInsertDocumentFilter(string prefix)
        {
            this._pathPrefix = prefix;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.Keys.ToList();
            foreach (var path in paths)
            {
                var pathToChange = swaggerDoc.Paths[path];
                swaggerDoc.Paths.Remove(path);
                swaggerDoc.Paths.Add($"{_pathPrefix}{path}", pathToChange);
            }
        }
    }
}
