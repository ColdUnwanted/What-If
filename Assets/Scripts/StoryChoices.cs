using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryChoices : MonoBehaviour
{
    public bool goodChildhood;
    public bool badChildhood;
    public bool study;
    public bool lazy;
    public bool stay;
    public bool leave;
    public bool continueStudies;
    public bool stopStudies;

    public bool pregnant;

    public int which;

    private void Start()
    {
        // Pregnant or not chance
        int random = Random.Range(0, 2);
        
        // 0 = pregnant; 1 = not
        if (random == 0)
        {
            pregnant = true;
        }
    }

    private void Update()
    {
        ChooseEnding();
    }

    void ChooseEnding()
    {
        if (badChildhood && lazy && leave && pregnant && stopStudies) // 00000
        {
            which = 1;
        }
        else if (badChildhood && lazy && leave && pregnant && continueStudies) // 00001
        {
            which = 5;
        }
        else if (badChildhood && lazy && leave && !pregnant && stopStudies) // 00010
        {
            which = 5;
        }
        else if (badChildhood && lazy && leave && !pregnant && continueStudies) // 00011
        {
            which = 5;
        }
        else if (badChildhood && lazy && stay && pregnant && stopStudies) // 00100
        {
            which = 4;
        }
        else if (badChildhood && lazy && stay && pregnant && continueStudies) // 00101
        {
            which = 2;
        }
        else if (badChildhood && lazy && stay && !pregnant && stopStudies) // 00110
        {
            which = 4;
        }
        else if (badChildhood && lazy && stay && !pregnant && continueStudies) // 00111
        {
            which = 4;
        }
        else if (badChildhood && study && leave && pregnant && stopStudies) // 01000
        {
            which = 1;
        }
        else if (badChildhood && study && leave && pregnant && continueStudies) // 01001
        {
            which = 5;
        }
        else if (badChildhood && study && leave && !pregnant && stopStudies) // 01010
        {
            which = 5;
        }
        else if (badChildhood && study && leave && !pregnant && continueStudies) // 01011
        {
            which = 5;
        }
        else if (badChildhood && study && stay && pregnant && stopStudies) // 01100
        {
            which = 4;
        }
        else if (badChildhood && study && stay && pregnant && continueStudies) // 01101
        {
            which = 2;
        }
        else if (badChildhood && study && stay && !pregnant && stopStudies) // 01110
        {
            which = 4;
        }
        else if (badChildhood && lazy && leave && pregnant && stopStudies) // 01111
        {
            which = 1;
        }
        else if (goodChildhood && lazy && leave && pregnant && stopStudies) // 10000
        {
            which = 1;
        }
        else if (goodChildhood && lazy && leave && pregnant && continueStudies) // 10001
        {
            which = 5;
        }
        else if (goodChildhood && lazy && leave && !pregnant && stopStudies) // 10010
        {
            which = 5;
        }
        else if (goodChildhood && lazy && leave && pregnant && continueStudies) // 10011
        {
            which = 5;
        }
        else if (goodChildhood && lazy && stay && pregnant && stopStudies) // 10100
        {
            which = 4;
        }
        else if (goodChildhood && lazy && stay && pregnant && continueStudies) // 10101
        {
            which = 4;
        }
        else if (goodChildhood && lazy && stay && !pregnant && stopStudies) // 10110
        {
            which = 4;
        }
        else if (goodChildhood && lazy && stay && !pregnant && continueStudies) // 10111
        {
            which = 4;
        }
        else if (goodChildhood && study && leave && pregnant && stopStudies) // 11000
        {
            which = 1;
        }
        else if (goodChildhood && study && leave && pregnant && continueStudies) // 11001
        {
            which = 5;
        }
        else if (goodChildhood && study && leave && !pregnant && stopStudies) // 11010
        {
            which = 5;
        }
        else if (goodChildhood && study && leave && !pregnant && continueStudies) // 11011
        {
            which = 5;
        }
        else if (goodChildhood && study && stay && pregnant && stopStudies) // 11100
        {
            which = 4;
        }
        else if (goodChildhood && study && stay && pregnant && continueStudies) // 11101
        {
            which = 3;
        }
        else if (goodChildhood && study && stay && !pregnant && stopStudies) // 11110
        {
            which = 4;
        }
        else if (goodChildhood && study && stay && !pregnant && continueStudies) // 11111
        {
            which = 3;
        }
    }
}
