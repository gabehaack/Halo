using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;
using Autofac;

namespace GHaack.Halo.Web.ModelMetadata
{
    public class ModelMetadataRegistrationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExtensibleModelMetadataProvider>()
                .As<ModelMetadataProvider>();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t is IModelMetadataFilter)
                .As<IModelMetadataFilter>();
        }
    }
}
