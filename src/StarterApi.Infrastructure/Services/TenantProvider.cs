using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TenantIdHeader = "X-TenantId";

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentTenantId()
    {
        var tenantIdString = _httpContextAccessor.HttpContext?.Request.Headers[TenantIdHeader].FirstOrDefault();
        return Guid.TryParse(tenantIdString, out Guid tenantId) ? tenantId : null;
    }

    public void SetCurrentTenantId(Guid tenantId)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.HttpContext.Request.Headers[TenantIdHeader] = tenantId.ToString();
        }
    }
} 