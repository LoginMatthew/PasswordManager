
namespace PasswordManager.Services
{
    /// <summary>
    /// This class is used for future development, e.g.: when adding a login page!
    /// </summary>
    public class CompositeNavigationService : INavigationService 
    {
        //multiple navagtion service defined here, a collection of them.
        private readonly IEnumerable<INavigationService> _navigationServices;

        public CompositeNavigationService(params INavigationService[] navigationServices)
        {
            _navigationServices = navigationServices;
        }

        public void Navigate()
        {
            foreach (INavigationService nagivationService in _navigationServices)
            {
                nagivationService.Navigate();
            }
        }
    }
}
