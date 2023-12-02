/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Object Class to Store PerlinNoise Settings
 *  
 *  References:
 *      Scene:
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - https://www.youtube.com/watch?v=jbwjbbc5PjI
 */


[System.Serializable]
public class PerlinSettingsObject
{
    public int octaves;
    public float persistence;
    public float frequency;
    public float amplitude;
    public float xOrg;
    public float yOrg;
    public float zoom;

    public PerlinSettingsObject(float persistence, float frequency, int octaves, float amplitude, float xOrg, float yOrg, float zoom)
    {
        this.persistence = persistence;
        this.frequency = frequency;
        this.octaves = octaves;
        this.amplitude = amplitude;
        this.xOrg = xOrg;
        this.yOrg = yOrg;
        this.zoom = zoom;
    }

    public bool Equals(PerlinSettingsObject pso)
    {
        return  this.persistence == pso.persistence &&
                this.frequency == pso.frequency &&
                this.octaves == pso.octaves &&
                this.amplitude == pso.amplitude &&
                this.xOrg == pso.xOrg &&
                this.yOrg == pso.yOrg &&
                this.zoom == pso.zoom;
    }
}
