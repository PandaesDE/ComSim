using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu
{
    private GameObject content;

    private Button btn_previous;

    private ContextMenu previous;

    public ContextMenu(GameObject content)
    {
        this.content = content;
    }

    public void setNext(Button btn_to_next, ContextMenu next)
    {
        btn_to_next.onClick.AddListener(delegate
        {
            next.Show();
            Hide();
        });
    }

    public void setPrevious(Button btn_previous, ContextMenu previous)
    {
        this.previous = previous;
        this.btn_previous = btn_previous;
        this.btn_previous.onClick.AddListener(delegate
        {
            this.previous.Show();
            if (!this.previous.hasPrevious())
            {
                this.btn_previous.gameObject.SetActive(false);
            }
            Hide();
        });
    }

    public bool hasPrevious()
    {
        return this.previous != null;
    }

    public void Show()
    {
        if (this.previous != null)
        {
            this.btn_previous.gameObject.SetActive(true);
        }
        this.content.SetActive(true);
    }

    public void Hide()
    {
        this.content.SetActive(false);
    }
}
