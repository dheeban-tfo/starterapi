using System;
using System.Collections.Generic;

namespace StarterApi.Application.Common.Models
{
    public class LookupDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class IndividualDetailDto : LookupDetailDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UnitDetailDto : LookupDetailDto
    {
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string Status { get; set; }
        public LookupDetailDto SelectedFloor { get; set; }
        public LookupDetailDto SelectedBlock { get; set; }
        public LookupDetailDto SelectedSociety { get; set; }
        public LookupDetailDto SelectedCurrentOwner { get; set; }
    }

    public class BlockDetailDto : LookupDetailDto
    {
        public string Code { get; set; }
        public LookupDetailDto SelectedSociety { get; set; }
        public int FloorCount { get; set; }
        public int UnitCount { get; set; }
    }

    public class FloorDetailDto : LookupDetailDto
    {
        public int FloorNumber { get; set; }
        public LookupDetailDto SelectedBlock { get; set; }
        public int UnitCount { get; set; }
    }

    public class ResidentDetailDto : LookupDetailDto
    {
        public string ResidentType { get; set; }
        public string Status { get; set; }
        public bool IsVerified { get; set; }
        public LookupDetailDto SelectedIndividual { get; set; }
        public LookupDetailDto SelectedUnit { get; set; }
    }

    public class UserDetailDto : LookupDetailDto
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
        public LookupDetailDto SelectedRole { get; set; }
    }

    public class RoleDetailDto : LookupDetailDto
    {
        public bool IsDefault { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class SocietyDetailDto : LookupDetailDto
    {
        public string RegistrationNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int BlockCount { get; set; }
        public int UnitCount { get; set; }
    }

    public class OwnerDetailDto : LookupDetailDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnershipType { get; set; }
        public LookupDetailDto SelectedIndividual { get; set; }
    }
}
