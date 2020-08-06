using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ReadlinesFromText : MonoBehaviour
{
    public Text JokeText;

    // Start is called before the first frame update
    void Start()
    {
        var path = Application.streamingAssetsPath + "\\Premises_v2.txt";
        string line = null;

        try
        {
            int lineNumber = 0;

            using (Stream source = File.OpenRead(path))
            {
                var lineCount = CountLinesSmarter(source);

                Debug.Log(lineCount);
                lineNumber = Convert.ToInt32(UnityEngine.Random.Range(0.0f, lineCount));

                //using (StreamReader reader = new StreamReader(source))
                //{
                //    if (lineNumber == 0)
                //    {
                //        line = reader.ReadLine();
                //    }
                //    else
                //    {
                //        //File.ReadLines(FileName).Skip(14).Take(1).First();
                //        for (int i = 0; i < lineNumber; ++i)
                //        {
                //            line = reader.ReadLine();
                //        }
                //    }
                //}
            }

            using (StreamReader reader = new StreamReader(path))
            {
                if (lineNumber == 0)
                {
                    line = reader.ReadLine();
                }
                else
                {
                    //File.ReadLines(FileName).Skip(14).Take(1).First();
                    for (int i = 0; i < lineNumber; ++i)
                    {
                        line = reader.ReadLine();
                    }
                }
            }

            JokeText.text = line;
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            print("The file could not be read:");
            print(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Premise premise = new Premise();
            premise.Text = "Maicăta e așa proastă…";

            DomainLogic.DB.SavePremise(premise);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            DomainLogic.DB.GetPremise();
        }
    }

    private const char CR = '\r';
    private const char LF = '\n';
    private const char NULL = (char)0;

    public static long CountLinesSmarter(Stream stream)
    {
        if (stream == null)
            return 0;
        //Ensure.NotNull(stream, nameof(stream));

        var lineCount = 0L;

        var byteBuffer = new byte[1024 * 1024];
        var prevChar = NULL;
        var pendingTermination = false;

        int bytesRead;
        while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
        {
            for (var i = 0; i < bytesRead; i++)
            {
                var currentChar = (char)byteBuffer[i];
                switch (currentChar)
                {
                    case NULL:
                    case LF when prevChar == CR:
                        continue;
                    case CR:
                    case LF when prevChar != CR:
                        lineCount++;
                        pendingTermination = false;
                        break;
                    default:
                        if (!pendingTermination)
                        {
                            pendingTermination = true;
                        }
                        break;
                }
                prevChar = currentChar;
            }
        }

        if (pendingTermination) { lineCount++; }
        return lineCount;
    }
}
