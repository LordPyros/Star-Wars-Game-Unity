using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] mines;
    public GameObject[] dogFighters;
    public GameObject[] flyThruFighters;
    public GameObject[] friendlyDogFighters;
    public GameObject[] capitalShips;
    public GameObject[] transports;
    public GameObject[] boss;

    public GameObject[] prePositionedTweensTF;
    public GameObject[] prePositionedTweensTA;
    public GameObject[] prePositionedTweensTI;
    public GameObject[] prePositionedTweensTB;

    private Vector2 topRightOfScreen;

    private void Start()
    {
        // top right screen corner
        topRightOfScreen = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    public void StartSpawner()
    {
        // Check what is the current level and if the game is one player or 2
        // start correct spawning sequence
        switch (GameManager.Level)
        {
            case 1:
                Invoke("Level1Players1", 2f);
                break;
            case 2:
                Invoke("Level2Players1", 2f);
                break;
            case 3:
                Invoke("Level3Players1", 2f);
                break;
            case 4:
                Invoke("Level4Players1", 2f);
                break;
            case 5:
                Invoke("Level5Players1", 2f);
                break;
            case 6:
                Invoke("Level6Players1", 2f);
                break;
            case 7:
                Invoke("Level7Players1", 2f);
                break;
            case 8:
                Invoke("Level8Players1", 2f);
                break;
            //case 11:
            //    //Invoke("Level1Players2", 1f);
            //    InvokeRepeating("TestLevel", 2f, 30f);
            //    break;
            case 11:
                Invoke("Level1Players2", 2f);
                break;
            case 12:
                Invoke("Level2Players2", 2f);
                break;
            case 13:
                Invoke("Level3Players2", 2f);
                break;
            case 14:
                Invoke("Level4Players2", 2f);
                break;
            case 15:
                Invoke("Level5Players2", 2f);
                break;
            case 16:
                Invoke("Level6Players2", 2f);
                break;
            case 17:
                Invoke("Level7Players2", 2f);
                break;
            case 18:
                Invoke("Level8Players2", 2f);
                break;
        }
    }

    // start enemy spawner
    public void ScheduleEnemySpawner()
    {
        Invoke("StartSpawner", 0f);
    }

    //stop enemy spawner
    public void UnscheduleEnemySpawner()
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    IEnumerator SpawnMine(float posFromnLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject mine = Instantiate(mines[id]);
        mine.transform.position = new Vector2(posFromnLeft, topRightOfScreen.y + (mine.GetComponent<SpriteRenderer>().bounds.size.y / 2));

        // 0 = laser mine
        // 1 = torpedo mine
    }
    private IEnumerator SpawnLaserMineField(float lengthOfTime, float spawnFrequency)
    {
        StartCoroutine(SpawnMine(SetPos(Random.Range(5, 95)), 0, spawnFrequency));
        yield return new WaitForSeconds(spawnFrequency);
        lengthOfTime -= spawnFrequency;
        if (lengthOfTime > 0)
            StartCoroutine(SpawnLaserMineField(lengthOfTime, spawnFrequency));
    }
    private IEnumerator SpawnMixedMineField(float lengthOfTime, float spawnFrequency)
    {
        // spawn frequency of 1 makes thick minefield
        // spawn frequency of 4 make a thin field

        // randomly select type of mine to spawn
        int ran = Random.Range(0, 10);
        if (ran < 8)
            StartCoroutine(SpawnMine(SetPos(Random.Range(5, 95)), 0, spawnFrequency));
        else if (ran == 8)
            StartCoroutine(SpawnMine(SetPos(Random.Range(5, 95)), 1, spawnFrequency));
        else
            StartCoroutine(SpawnMine(SetPos(Random.Range(5, 95)), 2, spawnFrequency));
        yield return new WaitForSeconds(spawnFrequency);
        lengthOfTime -= spawnFrequency;
        if (lengthOfTime > 0)
            StartCoroutine(SpawnMixedMineField(lengthOfTime, spawnFrequency));
    }
    private IEnumerator SpawnMineField(float lengthOfTime, float spawnFrequency, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (id == 0)
            StartCoroutine(SpawnLaserMineField(lengthOfTime, spawnFrequency));
        else
            StartCoroutine(SpawnMixedMineField(lengthOfTime, spawnFrequency));
    }


    IEnumerator SpawnPrePositionedTweensTF(int id, bool reverse, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // instantiate enemy in location provided
        GameObject t = Instantiate(prePositionedTweensTF[id]);
        if (reverse)
            t.transform.rotation = Quaternion.Euler(180, 0, 180);
    }
    IEnumerator SpawnPrePositionedTweensTB(int id, bool reverse, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // instantiate enemy in location provided
        GameObject t = Instantiate(prePositionedTweensTB[id]);
        if (reverse)
            t.transform.rotation = Quaternion.Euler(180, 0, 180);
    }
    IEnumerator SpawnPrePositionedTweensTI(int id, bool reverse, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // instantiate enemy in location provided
        GameObject t = Instantiate(prePositionedTweensTI[id]);
        if (reverse)
            t.transform.rotation = Quaternion.Euler(180, 0, 180);
    }
    IEnumerator SpawnPrePositionedTweensTA(int id, bool reverse, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // instantiate enemy in location provided
        GameObject t = Instantiate(prePositionedTweensTA[id]);
        if (reverse)
            t.transform.rotation = Quaternion.Euler(180, 0, 180);
    }

    IEnumerator SpawnDogFighter(float posFromLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject df = Instantiate(dogFighters[id]);
        df.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (df.GetComponent<SpriteRenderer>().bounds.size.y / 2));

        // 0 = tie fighter
        // 1 = tie fighter suicide
        // 2 = tie interceptor
        // 3 = tie interceptor suicide
        // 4 = tie bomber
        // 5 = tie bomber suicide
        // 6 = tie bomber torpedo
        // 7 = tie advanced
        // 8 = tie advanced missile
    }

    IEnumerator SpawnFlyThruFighter(float posFromLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject fighter = Instantiate(flyThruFighters[id]);
        fighter.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (fighter.GetComponent<SpriteRenderer>().bounds.size.y / 2));

        // 0 = tie fighter
        // 1 = tie interceptor
        // 2 = tie bomber
        // 3 = tie advanced
    }
    IEnumerator SpawnFlyThruAngleFighter(float posFromLeft, int angle, int id, float delayTime)
    {
        //*** need to destroy fighter if it leaves the screen (fly thru script insufficient)

        // DOES NOT USE SETPOS METHOD!

        yield return new WaitForSeconds(delayTime);
        //posFromLeft - can spawn fighters anywhere outside the top half of the screen
        // Starts froms mid screen on the left and goes in a U shape around the top of the screen and ends mid screen on the right.
        // 0 = middle left of screen
        // 25 = top left of screen
        // 50 = center top of screen
        // 75 = top right of screen
        // 100 = middle right of screen

        // angle degrees (10 - 80 recommended  - ( 20 almost vertical, 70 almost horizontal)
        // if posFromLeft < 50 - ships will face toward the right
        // posFromLeft >= 50 - ships will face toward the left

        // If posFromLeft is between 25 - 75 you can use a minus to point the other way



        // Make sure posFromLeft is a compatible number
        if (posFromLeft <= 0)
            posFromLeft = 1;
        else if (posFromLeft >= 100)
            posFromLeft = 99;

        // make sure angle is a compatible number
        if (posFromLeft > 24 && posFromLeft < 76)
        {
            if (angle > 85 || angle < -85)
            {
                Debug.Log("Bad Angle, Auto adjusting to 45 degrees");
                angle = 45;
            }
        }
        else if (posFromLeft > 75 || posFromLeft < 25)
        {
            if (angle < 5 || angle > 85)
            {
                Debug.Log("Bad Angle, Auto adjusting to 45 degrees");
                angle = 45;
            }
        }

        float newPos;
        // spawn just outside top half of the screen on the left side
        if (posFromLeft < 25)
        {
            // 0 = center (0)
            // 24 = top corner
            if (posFromLeft == 0)
                newPos = 0f;
            else
            {
                angle -= 90;
                angle *= -1;

                posFromLeft *= 4;
                posFromLeft = posFromLeft * topRightOfScreen.y;
                newPos = posFromLeft / 100;
                GameObject fighter = Instantiate(flyThruFighters[id]);
                fighter.transform.position = new Vector2(-topRightOfScreen.x - (fighter.GetComponent<SpriteRenderer>().bounds.size.x / 2), newPos);
                fighter.transform.rotation = Quaternion.Euler(0, 0, -angle);
            }
        }
        // spawn just outside top half of the screen on the left side
        else if  (posFromLeft > 75)
        {
            // 0 = center (0)
            // 24 = top corner
            if (posFromLeft == 100)
                newPos = 0f;
            else
            {
                angle -= 90;
                angle *= -1;

                posFromLeft -= 100;
                posFromLeft *= -1;
                posFromLeft *= 4;
                posFromLeft = posFromLeft * topRightOfScreen.y;
                newPos = posFromLeft / 100;
                                
                GameObject fighter = Instantiate(flyThruFighters[id]);
                fighter.transform.position = new Vector2(topRightOfScreen.x - (fighter.GetComponent<SpriteRenderer>().bounds.size.x / 2), newPos);
                fighter.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            }
        }
        // spawn just above the top of the screen
        else
        {
            
            if (posFromLeft < 50)
            {
                angle -= 90;
                angle *= -1;

                posFromLeft -= 25;
                posFromLeft *= 2;
                posFromLeft = 100 - posFromLeft;
                posFromLeft *= topRightOfScreen.x;
                posFromLeft /= 100;
                posFromLeft *= -1;

                GameObject fighter = Instantiate(flyThruFighters[id]);
                fighter.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (fighter.GetComponent<SpriteRenderer>().bounds.size.y / 2));
                fighter.transform.rotation = Quaternion.Euler(0, 0, -angle);

            }
            else
            {
                if (posFromLeft == 50)
                    posFromLeft = 0;
                else
                {
                    posFromLeft -= 50;
                    posFromLeft *= 4;
                    posFromLeft *= topRightOfScreen.x;
                    posFromLeft /= 100;
                }

                angle -= 90;
                angle *= -1;

                GameObject fighter = Instantiate(flyThruFighters[id]);
                fighter.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (fighter.GetComponent<SpriteRenderer>().bounds.size.y / 2));
                fighter.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            }
        }
    }
    public float SetPos(int num)
    {
        if (num == 50)
            return 0f;
        else if (num > 50)
        {
            num -= 50;
            num *= 2;
            float newNum = num * topRightOfScreen.x;
            newNum /= 100;
            return newNum;
        }
        else
        {
            num *= 2;
            num = 100 - num;
            float newNum = num * topRightOfScreen.x;
            newNum /= 100;
            newNum = newNum * -1;
            return newNum;
        }
    }

    IEnumerator SpawnFriendlyDogFighter(float leftOrRight, int id, float delayTime)
    {
        // leftOrRight reprents where the fighter spawns, the fighter will spawn just off the side of the screen
        // 0 is middle of screen on the left
        // 50 is top of screen on the left
        // 51 is middle of the screen on the right
        // 100 is top of the screen on the right

        yield return new WaitForSeconds(delayTime);
        GameObject ff = Instantiate(friendlyDogFighters[id]);
        // if the number is 50 or less, spawn on left side
        if (leftOrRight < 51)
        {
            leftOrRight *= 2;
            leftOrRight = 100 - leftOrRight;
            float newNum = leftOrRight * topRightOfScreen.y;
            newNum /= 100;


            ff.transform.position = new Vector2(-topRightOfScreen.x - (ff.GetComponent<SpriteRenderer>().bounds.size.x / 2), newNum);
        }
        // *** this spawns the fighter facing the wrong direction...
        // spawn on right side of screen
        else
        {
            leftOrRight -= 50;
            leftOrRight *= 2;
            float newNum = leftOrRight * topRightOfScreen.y;
            newNum /= 100;



            ff.transform.position = new Vector2(topRightOfScreen.x + (ff.GetComponent<SpriteRenderer>().bounds.size.x / 2), newNum);
        }



        // 0 = X-Wing
        // 1 = A-Wing
        // 2 = Falcon

    }

    IEnumerator SpawnTransport(float posFromLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // spawn ship
        GameObject t = Instantiate(transports[id]);
        // set position to "posFromLeft" on x axis and to half of the ships height, above the top of the screen
        t.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (t.GetComponent<SpriteRenderer>().bounds.size.y / 2));

        // 0 = ATR-6
        // 1 = ATR-6 Fly Thru
        // 2 = ATR-6 Torpedo
    }

    IEnumerator SpawnCapitalShip(float posFromLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // spawn ship
        GameObject cs = Instantiate(capitalShips[id]);
        // set position to "posFromLeft" on x axis and to half of the ships height, above the top of the screen
        cs.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (cs.GetComponent<SpriteRenderer>().bounds.size.y / 2));

        // 0 = Corvette Stay And Fight
        // 1 = Corvette Fly Thru
    }

    IEnumerator SpawnBoss(float posFromLeft, int id, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // check if all enemies are dead
        GameObject[] go = GameObject.FindGameObjectsWithTag("EnemyShipTag");
        bool enemiesExist = false;
        if (go.Length == 0)
        {
            go = GameObject.FindGameObjectsWithTag("EnemyCapitalShipTag");
            if (go.Length == 0)
            {
                // spawn ship
                GameObject b = Instantiate(boss[id]);
                // set position to "posFromLeft" on x axis and to half of the ships height, above the top of the screen
                b.transform.position = new Vector2(posFromLeft, topRightOfScreen.y + (b.GetComponent<SpriteRenderer>().bounds.size.y / 2));
                // 0 = Star Destroyer Imp 1
            }
            else
                enemiesExist = true;
        }
        else
            enemiesExist = true;
        if (enemiesExist)
            StartCoroutine(SpawnBoss(posFromLeft, id, 2f));



    }

    // takes a percentage and returns a point on the x axis. (left screen edge = 0%, right = 100%)
    

   
    private void Level1Players1()
    {

    }
    private void Level2Players1()
    {

    }
    private void Level3Players1()
    {

    }
    private void Level4Players1()
    {

    }
    private void Level5Players1()
    {

    }
    private void Level6Players1()
    {

    }
    private void Level7Players1()
    {

    }
    private void Level8Players1()
    {

    }


    private void Level1Players2()
    {

        StartCoroutine(SpawnPrePositionedTweensTF(0, false, 2f));
        StartCoroutine(SpawnPrePositionedTweensTF(3, false, 4.1f));
        StartCoroutine(SpawnPrePositionedTweensTF(5, true, 5.6f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(25), 0, 7.2f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(22), 0, 7.3f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(75), 0, 8.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(72), 0, 8.6f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(50), 0, 10f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(47), 0, 10.1f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(53), 0, 10.1f));
        StartCoroutine(SpawnPrePositionedTweensTF(14, false, 12f));
        StartCoroutine(SpawnPrePositionedTweensTF(17, true, 16f));
        StartCoroutine(SpawnPrePositionedTweensTF(20, false, 18f));
        StartCoroutine(SpawnPrePositionedTweensTF(8, true, 21f));
        StartCoroutine(SpawnFriendlyDogFighter(25, 0, 23.8f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(35), 1, 23.8f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(32), 1, 23.95f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(65), 1, 25.1f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(68), 1, 25.25f));
        StartCoroutine(SpawnDogFighter(SetPos(90), 0, 27.5f));
        StartCoroutine(SpawnDogFighter(SetPos(20), 0, 28.5f));
        StartCoroutine(SpawnDogFighter(SetPos(60), 0, 29.5f));
        StartCoroutine(SpawnPrePositionedTweensTI(16, false, 34f));
        StartCoroutine(SpawnPrePositionedTweensTF(21, false, 36.2f));
        StartCoroutine(SpawnPrePositionedTweensTF(9, true, 38f));
        StartCoroutine(SpawnTransport(SetPos(60), 1, 41f));
        StartCoroutine(SpawnMine(SetPos(20), 0, 43f));
        StartCoroutine(SpawnMine(SetPos(75), 0, 45f));
        StartCoroutine(SpawnPrePositionedTweensTF(1, true, 45.5f));
        StartCoroutine(SpawnMine(SetPos(45), 0, 46f));
        StartCoroutine(SpawnMine(SetPos(85), 0, 48f));
        StartCoroutine(SpawnPrePositionedTweensTF(7, true, 48.5f));
        StartCoroutine(SpawnMine(SetPos(30), 0, 50f));
        StartCoroutine(SpawnFriendlyDogFighter(25, 1, 51.5f));
        StartCoroutine(SpawnPrePositionedTweensTB(9, false, 51.5f));
        StartCoroutine(SpawnPrePositionedTweensTF(21, true, 51.5f));
        StartCoroutine(SpawnPrePositionedTweensTB(5, false, 53.5f));
        StartCoroutine(SpawnPrePositionedTweensTB(8, false, 55f));
        StartCoroutine(SpawnCapitalShip(SetPos(30), 1, 58f));
        StartCoroutine(SpawnDogFighter(SetPos(90), 2, 59f));
        StartCoroutine(SpawnDogFighter(SetPos(60), 2, 60f));
        StartCoroutine(SpawnDogFighter(SetPos(99), 2, 62f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(20), 0, 65f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(80), 0, 65f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(50), 0, 66.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(40), 0, 68f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(60), 0, 68f));
        StartCoroutine(SpawnFriendlyDogFighter(25, 2, 68f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(30), 0, 69.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(70), 0, 69.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(50), 0, 69.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(10), 0, 69.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(90), 0, 69.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(20), 0, 71f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(80), 0, 71f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(30), 0, 72.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(50), 0, 72.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(70), 0, 72.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(20), 0, 74f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(40), 0, 74f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(60), 0, 74f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(80), 0, 74f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(35), 0, 75.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(65), 0, 75.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(15), 0, 75.5f));
        StartCoroutine(SpawnFlyThruFighter(SetPos(85), 0, 75.5f));
        StartCoroutine(SpawnTransport(SetPos(25), 0, 77f));
        StartCoroutine(SpawnPrePositionedTweensTI(15, false, 78f));
        StartCoroutine(SpawnDogFighter(SetPos(10), 0, 80f));
        StartCoroutine(SpawnDogFighter(SetPos(70), 2, 81f));
        StartCoroutine(SpawnDogFighter(SetPos(30), 1, 83f));
        StartCoroutine(SpawnCapitalShip(SetPos(65), 0, 85f));
        StartCoroutine(SpawnPrePositionedTweensTI(19, false, 90f));
        StartCoroutine(SpawnMineField(20f, 2.5f, 1, 91f));
        StartCoroutine(SpawnFlyThruAngleFighter(13, 45, 0, 100));
        StartCoroutine(SpawnFlyThruAngleFighter(25, 45, 0, 100));
        StartCoroutine(SpawnFlyThruAngleFighter(37, 45, 0, 100));
        StartCoroutine(SpawnFlyThruAngleFighter(1, 45, 1, 102));
        StartCoroutine(SpawnFlyThruAngleFighter(17, 45, 1, 102));
        StartCoroutine(SpawnFlyThruAngleFighter(33, 45, 1, 102));
        StartCoroutine(SpawnFlyThruAngleFighter(49, 45, 1, 102));
        StartCoroutine(SpawnDogFighter(SetPos(90), 6, 107f));
        StartCoroutine(SpawnDogFighter(SetPos(60), 6, 1087f));
        StartCoroutine(SpawnDogFighter(SetPos(75), 6, 109f));
        StartCoroutine(SpawnFlyThruAngleFighter(99, 30, 2, 114));
        StartCoroutine(SpawnFlyThruAngleFighter(89, 30, 2, 114));
        StartCoroutine(SpawnFlyThruAngleFighter(79, 30, 2, 114));
        StartCoroutine(SpawnFlyThruAngleFighter(99, 30, 2, 114));
        StartCoroutine(SpawnFlyThruAngleFighter(75, 30, 2, 116));
        StartCoroutine(SpawnFlyThruAngleFighter(65, 30, 2, 116));
        StartCoroutine(SpawnFlyThruAngleFighter(55, 30, 2, 116));
        StartCoroutine(SpawnFlyThruAngleFighter(45, -30, 2, 116));
        StartCoroutine(SpawnFlyThruAngleFighter(35, -30, 2, 116));
        StartCoroutine(SpawnPrePositionedTweensTI(14, false, 119f));
        StartCoroutine(SpawnPrePositionedTweensTF(16, false, 120f));
        StartCoroutine(SpawnPrePositionedTweensTB(7, false, 123f));
        StartCoroutine(SpawnDogFighter(SetPos(40), 8, 126f));
        StartCoroutine(SpawnDogFighter(SetPos(5), 8, 127f));
        StartCoroutine(SpawnDogFighter(SetPos(80), 8, 128f));

        StartCoroutine(SpawnBoss(SetPos(50), 0, 135));



    }
    private void Level2Players2()
    {

    }
    private void Level3Players2()
    {

    }
    private void Level4Players2()
    {

    }
    private void Level5Players2()
    {

    }
    private void Level6Players2()
    {

    }
    private void Level7Players2()
    {

    }
    private void Level8Players2()
    {

    }

    
}
