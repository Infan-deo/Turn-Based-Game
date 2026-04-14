using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private PopupService _popupService;
    [SerializeField] private MainMenuService _mainMenuService;
    [SerializeField] private LevelGrid _levelgrid;

    private ObjectResolver _resolver;

    void Awake()
    {
        _resolver = new ObjectResolver();
    }

    void OnEnable()
    {
        _resolver.RegisterInstance(_popupService);
        _resolver.RegisterInstance(_mainMenuService);
        _resolver.RegisterInstance(_levelgrid);

        _resolver.InjectInto(_popupService);
        _resolver.InjectInto(_mainMenuService);
        _resolver.InjectInto(_levelgrid);
    }

    void OnDisable()
    {
        _resolver.UnregisterInstance<PopupService>();
        _resolver.UnregisterInstance<MainMenuService>();
        _resolver.UnregisterInstance<LevelGrid>();
    }

    //example:
    // [Inject]
    // public void Inject(MainMenuService mainMenuService)
    // {
    //     _mainMenuService = mainMenuService;
    // }
}

