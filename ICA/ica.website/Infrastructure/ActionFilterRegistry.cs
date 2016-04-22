using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.TypeRules;
using System;
using System.Web.Mvc;

namespace ica.website.Infrastructure
{
    public class ActionFilterRegistry : Registry
    {
        public ActionFilterRegistry(Func<IContainer> containerFactory)
        {
            For<IFilterProvider>().Use(new StructureMapFilterProvider(containerFactory));

            SetAllProperties(x =>
                x.Matching(p =>
                    p.DeclaringType.CanBeCastTo(typeof(ActionFilterAttribute)) &&
                    p.DeclaringType.Namespace.StartsWith("ica.website") &&
                    !p.PropertyType.IsPrimitive &&
                    p.PropertyType != typeof(string)
                    ));
        }
    }
}