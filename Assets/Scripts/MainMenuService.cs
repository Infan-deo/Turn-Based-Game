using UnityEngine;

public class MainMenuService : MonoBehaviour
{
    public int number;
    private PopupService _popUpService;

    private LevelGrid levelGrid;

    // public void InjectResolver(ObjectResolver resolver)
    // {
    //     _popUpService = resolver.Resolve<PopupService>();
    //     levelGrid = resolver.Resolve<LevelGrid>();
    // }

    // public MainMenuService(PopupService popupService)
    // {
    //     _popUpService = popupService;
    // }

    [Inject]
    public void Inject(PopupService popUpService,LevelGrid levelGrid)
    {
        _popUpService = popUpService;
        this.levelGrid = levelGrid; 
    }

    public void Getinfo()
    {
        Debug.Log("message: " + _popUpService.message);
        Debug.Log("message: " + levelGrid.GetHeight());
    }


}
