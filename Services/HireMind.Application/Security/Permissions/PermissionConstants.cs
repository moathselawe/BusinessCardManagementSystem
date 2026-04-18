namespace HireMind.Application.Security.Permissions;
public static class PermissionConstants
{
    public static class Users
    {
        public const string View = "users.view";
        public const string Create = "users.create";
        public const string Update = "users.update";
        public const string Delete = "users.delete";
    }
    public static class Jobs
    {
        public const string View = "jobs.view";
        public const string Create = "jobs.create";
        public const string Update = "jobs.update";
        public const string Delete = "jobs.delete";
    }
    public static class Applications
    {
        public const string Apply = "applications.apply";
        public const string View = "applications.view";
    }

    public static class ApplicationStages
    {
        public const string Update = "applicationstages.update";
        public const string View = "applicationstages.view";
    }

    public static class Lookups
    {
        public const string View = "lookups.view";
        public const string Create = "lookups.create";
        public const string Update = "lookups.update";
        public const string Delete = "lookups.delete";
    }

    public static class AI
    {
        public const string Chat = "ai.chat";
        public const string Suggest = "ai.suggest";
    }

    public static class JobsAdmin
    {
        public const string Manage = "jobs.manage";
    }

    public static class HiringStages
    {
        public const string View = "hiringstages.view";
    }

    public static class BusinessCards
    {
        public const string View = "businesscards.view";
        public const string Create = "businesscards.create";
        public const string Update = "businesscards.update";
        public const string Delete = "businesscards.delete";
    }

    public static class Auth
    {
        public const string Manage = "auth.manage";
    }
}
