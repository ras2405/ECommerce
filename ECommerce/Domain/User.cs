using Exceptions.LogicExceptions;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public Roles Rol { get; set; }
        public enum Roles
        {
            Admin,
            Buyer,
            Both
        }

        public override bool Equals(Object obj)
        {
            bool result = false;
            User user = (User)obj;
            if (this.Address == user.Address
                && this.Email == user.Email
                && this.Password == user.Password
                && this.Rol == user.Rol)
            {
                result = true;
            }
            return result;
        }

        public void SelfValidation()
        {
            if (!IsValidEmail(this.Email)) 
                throw new BadRequestException("Invalid Email format");
            if (string.IsNullOrWhiteSpace(this.Address))
                throw new BadRequestException("Invalid address: User address can not be blank");
            if (string.IsNullOrWhiteSpace(this.Password))
                throw new BadRequestException("Invalid password: User password can not be blank");
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }
    }
}
