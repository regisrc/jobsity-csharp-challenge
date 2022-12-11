namespace Infrastructure.Entity
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Guid? LoggedToken { get; set; }

        public DateTime? TokenExpirationDate { get; set; }
    }
}
