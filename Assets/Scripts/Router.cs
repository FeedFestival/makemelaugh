using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Router : MonoBehaviour
{
    private static Router _router;
    public static Router Instance
    {
        get { return _router; }
    }

    public GameObject Drawer;
    [MyBox.Separator("Routes")]
    public GameObject AddJokeRoute;
    public GameObject YourJokesRoute;

    public string StartRoute;

    public Dictionary<string, GameObject> Routes;

    private void Awake()
    {
        _router = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Routes = new Dictionary<string, GameObject>() {
            { "AddJoke", AddJokeRoute },
            { "YourJokes", YourJokesRoute }
        };

        Init();
    }

    private void Init()
    {
        Drawer.SetActive(true);

        if (string.IsNullOrEmpty(StartRoute))
        {
            StartRoute = "AddJoke";
        }
        ChangeRoute(StartRoute);
    }

    public void ChangeRoute(string routeName)
    {
        foreach (KeyValuePair<string, GameObject> route in Routes)
        {
            if (route.Key == routeName)
            {
                route.Value.SetActive(true);
            }
            else
            {
                route.Value.SetActive(false);
            }
        }

        switch (StartRoute)
        {
            case "YourJokes":
                YourJokesRoute.GetComponent<YourJokesController>().Refresh();
                break;
            default:
                break;
        }
    }
}
