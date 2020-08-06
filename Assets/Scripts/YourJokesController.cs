using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YourJokesController : MonoBehaviour
{
    public RectTransform Content;
    public GameObject JokePanelPrefab;

    private List<int> _jokesShown;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var allJokes = DomainLogic.DataService.GetJokeHistory();

        if (_jokesShown == null)
        {
            _jokesShown = new List<int>();
        }

        foreach (Joke joke in allJokes)
        {
            var index = _jokesShown.IndexOf(joke.Id);
            var exists = index >= 0;
            JokePanel jokePanel;
            if (exists == false)
            {
                jokePanel = AddJokeInHistory();
                jokePanel.PremiseText.text = joke.Premise.Text;
                jokePanel.JokeText.text = joke.Text;
                _jokesShown.Add(joke.Id);
            }
        }
    }

    private JokePanel AddJokeInHistory()
    {
        var go = Instantiate(JokePanelPrefab);
        go.name = "Joke Panel";
        go.transform.SetParent(Content);
        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        return go.GetComponent<JokePanel>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
