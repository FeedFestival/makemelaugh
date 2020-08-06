using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AddJokeController : MonoBehaviour
{
    public Text PremiseText;
    public InputField JokeInput;

    private Premise _currentPremise;

    void Start()
    {
        GetJoke();
    }


    public void GetJoke()
    {
        _currentPremise = DomainLogic.DataService.GetRandomPremise();
        PremiseText.text = _currentPremise.Text;
    }

    public void SaveJoke()
    {
        var joke = new Joke();
        joke.Text = JokeInput.text;
        joke.PremiseId = _currentPremise.Id;
        DomainLogic.DataService.SaveJoke(joke);
    }
}
