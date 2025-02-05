using System;

namespace StarterApi.Application.Common.Models;

public class LookupRequestDto
{
    public string SearchTerm { get; set; } = string.Empty;
    public int MaxResults { get; set; } = 10;
}

public class IndividualLookupDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class UnitLookupDto
{
    public Guid Id { get; set; }
    public string UnitNumber { get; set; }
    public string FloorName { get; set; }
    public string BlockName { get; set; }
}

public class BlockLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string SocietyName { get; set; }
}

public class FloorLookupDto
{
    public Guid Id { get; set; }
    public string FloorName { get; set; }
    public int FloorNumber { get; set; }
    public string BlockName { get; set; }
}

public class ResidentLookupDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string UnitNumber { get; set; }
    public string ResidentType { get; set; }
    public string Status { get; set; }
}

public class UserLookupDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string RoleName { get; set; }
}

public class SocietyLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string RegistrationNumber { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}

public class FacilityLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }
}
