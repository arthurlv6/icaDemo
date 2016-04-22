using ica.website.Domain;
using ica.website.Infrastructure.Mapping;
using ica.website.Infrastructure.Tasks;
using ica.website.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ica.website.Models.Operator;

namespace ica.website.App_Start
{
    public class AutoMapperConfig : IRunAtInit
    {
        public void Execute()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            
            LoadStandardMappings(types);

            LoadCustomMappins(types);
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                        !t.IsAbstract &&
                        !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
            {
                Mapper.CreateMap(map.Source, map.Destination);
                Mapper.CreateMap(map.Destination, map.Source);
            }
            
        }

        private void LoadCustomMappins(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                        !t.IsAbstract &&
                        !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(Mapper.Configuration);
            }
            Mapper.CreateMap(typeof(ConditionViewModel<ApplicationUser>), typeof(ConditionViewModelTarget<OperatorViewModel>));
            Mapper.CreateMap( typeof(ConditionViewModelTarget<OperatorViewModel>), typeof(ConditionViewModel<ApplicationUser>)).ForMember("Data",d=>d.Ignore());
        }
    }
}