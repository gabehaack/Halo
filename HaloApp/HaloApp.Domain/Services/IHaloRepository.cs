using HaloApp.Domain.Models.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloApp.Domain.Services
{
    public interface IHaloRepository
    {
        Task ReplaceMetadata<TMetadata>(IList<TMetadata> metadata);
    }
}
