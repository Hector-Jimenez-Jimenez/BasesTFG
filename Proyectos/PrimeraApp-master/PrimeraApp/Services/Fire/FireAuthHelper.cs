using Firebase.Auth.Providers;
using Firebase.Auth;

namespace PrimeraApp.Services.Fire
{
    public class FireAuthHelper
    {
        public static FirebaseAuthClient client = new
            (
                new FirebaseAuthConfig()
                {
                    ApiKey = "AIzaSyCVX___VLGWINyhiU25bil1RGo1CCuarKQ",
                    AuthDomain = "ies-tubalcain-di-login.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
                }
            );
        public FireAuthHelper() { }
    }
}
