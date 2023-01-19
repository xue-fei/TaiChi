using FancyScrollView;
using System;

public class StoryContext : FancyScrollRectContext
{
    public int SelectedIndex = -1;
    public Action<int> OnCellClicked;
}