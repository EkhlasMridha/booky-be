namespace HelperServices
{
    public static class ConfirmMail
    {
        public const string Subject = "Verify Your Email";
        public const string Title = "Welcome to Root Line Zero Service";
        public const string Description = @"You are now registered to Root Line service. Just one step ahead to complete
                                            your registration. Click the below button to confirm your mail address.";
        public const string Button = "Verify Email";
    }

    public static class ResetPasswordMail
    {
        public const string Subject = "Password reset";
        public const string Title = "Reset your password";
        public const string Description = @"A password reset request has been made from your account. Click the below button to reset your password";
        public const string Button = "Reset password";
        public const string clientPath = "/reset-password";
    }

    public class Permissions
    {

    }

    public static class ClientConstant
    {
        public const string ClientId = "12ad67fedd362dbab60bcde8aff7e8ac7ddd9f15e77f2e4685";
    }
}
