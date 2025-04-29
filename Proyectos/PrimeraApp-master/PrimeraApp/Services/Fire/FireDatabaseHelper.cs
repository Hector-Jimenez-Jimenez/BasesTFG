using Firebase.Database;

namespace PrimeraApp.Services.Fire
{
    public class FireDatabaseHelper
    {
        public static FirebaseClient firebaseClient { get; } = new FirebaseClient(
          "https://ies-tubalcain-di-login-default-rtdb.europe-west1.firebasedatabase.app/",
          new FirebaseOptions
          {
              AuthTokenAsyncFactory = () => FireAuthHelper.client.User.GetIdTokenAsync()
          });
    }
}
