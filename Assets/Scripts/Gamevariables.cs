/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - interface for all generic game variables
 *  
 *  References:
 *      Scene:
 *          - scene independent
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System;
using UnityEngine;

public class Gamevariables
{
    //init Settings
    public static readonly Vector2Int PLAYGROUND_SIZE = new Vector2Int(200, 100);
    public static readonly float TICKRATE = .5f;

    public static readonly int HUMAN_PREGNANCY_TIME_DAYS = 18;
    public static readonly float AGE_DAYS_AS_YEARS_CONVERISON = 3;

    public static int HumanAmountStart = 0;
    public static int LionAmountStart = 0;
    public static int BoarAmountStart = 0;
    public static int RabbitAmountStart = 0;
    public static bool GamePaused = false;

    //VISUALIZATION
    public static bool ShowTrail = true;
    public static int TrailLength = 15;
    public static Trail.ColorScheme TrailColor = Trail.ColorScheme.@default;

    //WORLD
    public static string Seed = "";
    public static PerlinSettingsObject PSO_Ground = new(-.5f, .6f, 8, 1, 0, 0, 100);
    public static PerlinSettingsObject PSO_Bush = new(1f, 80f, 1, 1, 0, 0, 100);

    //Time
    public static readonly int HOURS_PER_DAY = 24;
    public static readonly int MINUTES_PER_HOUR = 60;

    public static int MinutesPerTick = 1;
    public static int MinutesPassed = 0;

    //Light
    /* between 0 - 1*/
    public static float LightIntensity = 1f;

    //Creatures
    public static readonly Creature.Attributes HUMAN_ATTRIBUTES = new ()
        {
        /*  MaxAge:
         *      - https://www.wissenschaftsjahr.de/2013/rueckblicke/forschungsmuseen-erklaeren-den-wandel/senckenberg-naturmuseum.html
         *      - https://www.spiegel.de/wissenschaft/mensch/homo-sapiens-und-neandertaler-40-jahre-alte-greise-a-738722.html
         *  FertilityAge:
         *      - https://www.researchgate.net/figure/Age-of-menarche-and-the-first-reproduction-A-regression-line-and-95-confidence-limits_fig1_51589335
         */
            FertilityAge = 18,
            MaxAge = 30,
            Health = 80,
            Weight = 80,
            Damage = 10,
            Speed = .2f,
        };
    public static readonly Creature.Attributes LION_ATTRIBUTES = new()
    {
        /*  MaxAge:
         *      - https://www.discoveryuk.com/big-cats/how-long-do-lions-live/#:~:text=Overall%2C%20lions%20can%20expect%20to,to%20make%20it%20to%20adulthood.
         *  FertilityAge:
         *      - https://ypte.org.uk/factsheets/lion/breeding
         */
        FertilityAge = 2,
        MaxAge = 12,
        Health = 65,
        Weight = 80,
        Damage = 35,
        Speed = .2f,
    };
    public static readonly Creature.Attributes BOAR_ATTRIBUTES = new()
    {
        /*  MaxAge:
         *      - https://feralhogs.extension.org/feral-hog-population-biology/#:~:text=The%20maximum%20lifespan%20is%20estimated,9%2D26%20months%20of%20age.
         *  FertilityAge:
         *      - https://www.pigprogress.net/topic/boar-infertility/#:~:text=The%20production%20of%20normal%20sperm,of%20the%20testes%20and%20accessory
         */
        FertilityAge = .42f,
        MaxAge = 10,
        Health = 110,
        Weight = 130,
        Damage = 15,
        Speed = .2f,
    };
    public static readonly Creature.Attributes RABBIT_ATTRIBUTES = new()
    {
        /*  MaxAge:
         *      - https://agriculture.vic.gov.au/livestock-and-animals/animal-welfare-victoria/other-pets/rabbits/owning-a-rabbit#:~:text=Rabbits%20generally%20live%20for%205,care%20for%20them%20that%20long.
         *  FertlityAge:
         *      - https://www.msdvetmanual.com/all-other-pets/rabbits/breeding-and-reproduction-of-rabbits#:~:text=Rabbit%20breeds%20of%20medium%20to,of%20hormones%20as%20in%20humans.
         */
        FertilityAge = .3f,
        MaxAge = 7,
        Health = 45,
        Weight = 30,
        Damage = 1,
        Speed = .2f,
    };
    public static readonly Creature.Attributes NORMALIZED_ATTRIBUTES = new()
    {
        FertilityAge = 3,
        MaxAge = 10,
        Health = 100,
        Weight = 80,
        Damage = 10,
        Speed = .2f
    };


    //Error - Values
    public static readonly Vector2 ERROR_VECTOR2 = Vector2.negativeInfinity;

    public enum Z_layer
    {
        camera = -10,
        entity = -1,
        world = 0
    }

    //Debug
    public static readonly bool LOGGING_ENABLED = false;
}
