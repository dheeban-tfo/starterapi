using System;

namespace StarterApi.Application.Common.Models;

public class LookupRequestDto
{
    public string SearchTerm { get; set; }
    public int MaxResults { get; set; } = 10;
}

public class IndividualLookupDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
