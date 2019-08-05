namespace Machete.Web.ViewModel.Api.Identity {
    public class RegistrationViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } //?
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
