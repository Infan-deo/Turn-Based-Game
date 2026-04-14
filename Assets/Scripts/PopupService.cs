using UnityEngine;

public class PopupService : MonoBehaviour
{
    private MainMenuService _mainMenuService;
    public string message;


    // public void InjectResolver(ObjectResolver resolver)
    // {
    //     _mainMenuService = resolver.Resolve<MainMenuService>();
    // }
    [Inject]
    public void Inject(MainMenuService mainMenuService)
    {
        _mainMenuService = mainMenuService;
    }

    // public PopupService(MainMenuService mainMenuService)
    // {
    //     _mainMenuService = mainMenuService;  
    // }
    public void Getinfo()
    {
        Debug.Log("message: " + _mainMenuService.number);
    }

}
