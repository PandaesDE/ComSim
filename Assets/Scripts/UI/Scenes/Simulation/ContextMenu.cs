/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Logical helper class for ContextMenu navigation
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;
using UnityEngine.UI;

public class ContextMenu
{
    private GameObject _content;

    private Button _btn_previous;

    private ContextMenu _previous;

    public ContextMenu(GameObject content)
    {
        this._content = content;
    }

    public void SetNext(Button btn_to_next, ContextMenu next)
    {
        btn_to_next.onClick.AddListener(delegate
        {
            next.Show();
            Hide();
        });
    }

    public void SetPrevious(Button btn_previous, ContextMenu previous)
    {
        this._previous = previous;
        this._btn_previous = btn_previous;
        this._btn_previous.onClick.AddListener(delegate
        {
            this._previous.Show();
            if (!this._previous.HasPrevious())
            {
                this._btn_previous.gameObject.SetActive(false);
            }
            Hide();
        });
    }

    public bool HasPrevious()
    {
        return this._previous != null;
    }

    public void Show()
    {
        if (this._previous != null)
        {
            this._btn_previous.gameObject.SetActive(true);
        }
        this._content.SetActive(true);
    }

    public void Hide()
    {
        this._content.SetActive(false);
    }
}
