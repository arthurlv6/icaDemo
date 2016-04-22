using StructureMap.Configuration.DSL;
using System.Web.Mvc;

namespace ica.website.Infrastructure.ModelMetadata
{
    public class ModelMetadataRegidrty : Registry
    {
        public ModelMetadataRegidrty()
        {
            For<ModelMetadataProvider>().Use<ExtensibleModelMetadataProvider>();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AddAllTypesOf<IModelMetadataFilter>();
            });
        }
    }
}