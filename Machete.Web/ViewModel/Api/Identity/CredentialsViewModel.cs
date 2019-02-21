namespace Machete.Web.ViewModel.Api.Identity {
    public class CredentialsViewModel {
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool Remember { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Code { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
    }
}
