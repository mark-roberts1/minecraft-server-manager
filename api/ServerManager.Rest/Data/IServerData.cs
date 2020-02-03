using ServerManager.Rest.Dto;
using ServerManager.Rest.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public interface IServerData : IServerOperator
    {
        Task<CreateServerResponse> CreateAsync(CreateServerRequest createRequest, CancellationToken cancellationToken);
        Task<IEnumerable<Template>> ListTemplatesAsync(CancellationToken cancellationToken);
        Task<AddTemplateResponse> AddTemplateAsync(AddTemplateRequest addTemplateRequest, CancellationToken cancellationToken);
        Task<Template> GetTemplateAsync(int templateId, CancellationToken cancellationToken);
        Task<UpdateTemplateResponse> UpdateTemplateAsync(UpdateTemplateRequest updateTemplateRequest, CancellationToken cancellationToken);
    }
}
