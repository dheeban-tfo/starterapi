namespace StarterApi.Domain.Constants
{
    public static class Permissions
    {
        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
            public const string ManageRoles = "Users.ManageRoles";
            public const string InviteUser = "Users.Invite";
        }

        public static class Roles
        {
            public const string View = "Roles.View";
            public const string Create = "Roles.Create";
            public const string Edit = "Roles.Edit";
            public const string Delete = "Roles.Delete";
            public const string ManagePermissions = "Roles.ManagePermissions";
        }

        public static class Tenants
        {
            public const string View = "Tenants.View";
            public const string Create = "Tenants.Create";
            public const string Edit = "Tenants.Edit";
            public const string Delete = "Tenants.Delete";
            public const string ManageUsers = "Tenants.ManageUsers";
        }

        public static class Societies
        {
            public const string View = "Societies.View";
            public const string Create = "Societies.Create";
            public const string Edit = "Societies.Edit";
            public const string Delete = "Societies.Delete";
            public const string ManageBlocks = "Societies.ManageBlocks";
        }

        public static class Blocks
        {
            public const string View = "Blocks.View";
            public const string Create = "Blocks.Create";
            public const string Edit = "Blocks.Edit";
            public const string Delete = "Blocks.Delete";
            public const string ManageFloors = "Blocks.ManageFloors";
        }

        public static class Floors
        {
            public const string View = "Floors.View";
            public const string Create = "Floors.Create";
            public const string Edit = "Floors.Edit";
            public const string Delete = "Floors.Delete";
            public const string ManageUnits = "Floors.ManageUnits";
        }

        public static class Units
        {
            public const string View = "Units.View";
            public const string Create = "Units.Create";
            public const string Edit = "Units.Edit";
            public const string Delete = "Units.Delete";
        }

        public static class Individuals
        {
            public const string View = "Individuals.View";
            public const string Create = "Individuals.Create";
            public const string Edit = "Individuals.Edit";
            public const string Delete = "Individuals.Delete";
            public const string Verify = "Individuals.Verify";
        }

        public static class Residents
        {
            public const string View = "Residents.View";
            public const string Create = "Residents.Create";
            public const string Edit = "Residents.Edit";
            public const string Delete = "Residents.Delete";
            public const string ManageFamily = "Residents.ManageFamily";
            public const string Verify = "Residents.Verify";
        }

        public static class Documents
        {
            public const string View = "Documents.View";
            public const string Create = "Documents.Create";
            public const string Edit = "Documents.Edit";
            public const string Delete = "Documents.Delete";
            public const string ManageCategories = "Documents.ManageCategories";
            public const string ManageAccess = "Documents.ManageAccess";
            public const string ManageVersions = "Documents.ManageVersions";
        }

        public static IEnumerable<string> GetAllPermissions()
        {
            return typeof(Permissions)
                .GetNestedTypes()
                .SelectMany(t => t.GetFields())
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetValue(null).ToString());
        }
    }
} 